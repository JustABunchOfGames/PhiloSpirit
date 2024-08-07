using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Building
{

    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private TileManager _tileManager;

        // Currently selected building
        private BuildingData _data;
        private Tile _tile;
        private Quaternion _rotation;

        public BuildingPositioningEvent buildingPositioningEvent = new BuildingPositioningEvent();

        public CanceledPositioningEvent canceledPositioningEvent = new CanceledPositioningEvent();

        public BuildingCostEvent buildingCostEvent = new BuildingCostEvent();

        public CompleteBuildingEvent completeBuildingEvent = new CompleteBuildingEvent();

        public void SelectBuilding(BuildingData data)
        {
            _data = data;
            _tile = null;
            _rotation = Quaternion.identity;

            // Change behaviour to forbid tile selection
            _tileManager.CanSelect(false);

            buildingPositioningEvent.Invoke(_data);
        }

        public void Reselect()
        {
            SelectBuilding(_data);
        }

        public void SetPosition(Tile tile, Quaternion rotation)
        {
            _tile = tile;
            _rotation = rotation;

            buildingCostEvent.Invoke(_data, _tile);
        }

        public void CancelPositioning()
        {
            // Change behaviour to allow tile selection
            _tileManager.CanSelect(true);

            canceledPositioningEvent.Invoke();
        }

        public void CompleteBuiding()
        {
            // Change behaviour to allow tile selection
            _tileManager.CanSelect(true);

            completeBuildingEvent.Invoke(_data, _tile, _rotation);
        }

        public class BuildingPositioningEvent : UnityEvent<BuildingData> { }

        public class CanceledPositioningEvent : UnityEvent { }

        public class BuildingCostEvent : UnityEvent<BuildingData, Tile> { }

        public class CompleteBuildingEvent : UnityEvent<BuildingData, Tile, Quaternion> { }
    }
}