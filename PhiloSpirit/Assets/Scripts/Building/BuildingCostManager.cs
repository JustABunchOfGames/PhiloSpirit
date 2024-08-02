using Resources;
using Spirits;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Building
{
    public class BuildingCostManager : MonoBehaviour
    {
        private BuildingManager _manager;

        private BuildingData _currentData;
        private Tile _currentTile;

        public ShowCostEvent showCostEvent = new ShowCostEvent();

        private void Awake()
        {
            _manager = GetComponent<BuildingManager>();
        }

        private void Start()
        {
            _manager.buildingCostEvent.AddListener(StartBuildingCost);
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

        public class ShowCostEvent : UnityEvent<BuildingData, Tile, bool> { }
    }
}