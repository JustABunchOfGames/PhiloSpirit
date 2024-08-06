using Resources;
using Spirits;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Building
{
    public class BuildingCostManager : MonoBehaviour
    {
        // Managers
        private BuildingManager _manager;
        private BuildingUndoManager _undoManager;
        private BuildingGameObjectManager _gameObjectManager;

        // Building Cost data
        private BuildingData _currentData;

        // Building Refund data
        private BuildingGameObject _buildingGO;

        // Tile currently worked on
        private Tile _currentTile;

        public ShowCostEvent showCostEvent = new ShowCostEvent();
        public ShowRefundEvent showRefundEvent = new ShowRefundEvent();

        private void Awake()
        {
            _manager = GetComponent<BuildingManager>();
            _undoManager = GetComponent<BuildingUndoManager>();
            _gameObjectManager = GetComponent<BuildingGameObjectManager>();
        }

        private void Start()
        {
            _manager.buildingCostEvent.AddListener(StartBuildingCost);

            _undoManager.selectEvent.AddListener(StartBuildingRefund);
        }

        private void StartBuildingCost(BuildingData data, Tile tile)
        {
            _currentData = data;
            _currentTile = tile;

            bool isCostPayable = true;
            foreach (Resource res in data.cost.resourceCost.resources)
            {
                if (!tile.inventory.HasEnough(res))
                {
                    isCostPayable = false;
                    break;
                }
            }

            showCostEvent.Invoke(data, tile, isCostPayable);
        }

        public void ConfirmCost()
        { 
            foreach (SpiritCost spiritCost in _currentData.cost.spiritCost)
            {
                for (uint i = spiritCost.quantity; i > 0; i--)
                {
                    if (!SpiritManager.CanUseSpirit(spiritCost.type, 1))
                        SpiritManager.AddSpirit(spiritCost.type);

                    SpiritManager.UseSpirit(spiritCost.type, 1);
                }
            }
            
            foreach (Resource res in _currentData.cost.resourceCost.resources)
            {
                _currentTile.inventory.Remove(res);
            }
            
            foreach(Resource res in _currentData.output.resources)
            {
                _currentTile.inventory.Add(res);
            }
            

            _manager.CompleteBuiding();
        }

        public void CancelCost()
        {
            _manager.Reselect();
        }

        private void StartBuildingRefund(BuildingGameObject building, Tile tile)
        {
            _buildingGO = building;
            _currentTile = tile;

            BuildingData data = _gameObjectManager.GetData(building);

            _currentData = data;

            bool isRefundPossible = true;
            foreach (Resource res in data.output.resources)
            {
                if (!tile.inventory.HasEnough(res))
                {
                    isRefundPossible = false;
                    break;
                }
            }

            showRefundEvent.Invoke(data, tile, isRefundPossible);
        }

        public void ConfirmRefund()
        {
            // Stop undo selection and restart input "normal" behaviour
            _undoManager.Stop();

            foreach(SpiritCost spiritCost in _currentData.cost.spiritCost)
            {
                int quantity = (int) spiritCost.quantity;
                SpiritManager.UseSpirit(spiritCost.type, -quantity);
            }

            foreach(Resource res in _currentData.output.resources)
            {
                _currentTile.inventory.Remove(res);
            }

            foreach(Resource res in _currentData.cost.resourceCost.resources)
            {
                _currentTile.inventory.Add(res);
            }

            _gameObjectManager.DestroyBuilding(_buildingGO);
        }

        public void CancelRefund()
        {
            _undoManager.StartUndoSelection();
        }

        public class ShowCostEvent : UnityEvent<BuildingData, Tile, bool> { }

        public class ShowRefundEvent : UnityEvent<BuildingData, Tile, bool> { }
    }
}