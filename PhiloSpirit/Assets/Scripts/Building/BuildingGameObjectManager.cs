using System.Collections.Generic;
using Terrain;
using UnityEngine;

namespace Building
{
    public class BuildingGameObjectManager : MonoBehaviour
    {
        private BuildingManager _manager;

        [SerializeField] private Transform _parent;
        private Dictionary<BuildingGameObject, BuildingData> _buildingDictionary;

        private void Awake()
        {
            _manager = GetComponent<BuildingManager>();
        }

        private void Start()
        {
            _manager.completeBuildingEvent.AddListener(CompleteBuilding);

            _buildingDictionary = new Dictionary<BuildingGameObject, BuildingData>();
        }

        private void CompleteBuilding(BuildingData data, Tile tile, Quaternion rotation)
        {
            BuildingGameObject building = Instantiate(data.prefab, tile.transform.position, rotation, _parent);
            _buildingDictionary.Add(building, data);
        }

        public BuildingData GetData(BuildingGameObject building)
        {
            return _buildingDictionary[building];
        }

        public void DestroyBuilding(BuildingGameObject building)
        {
            _buildingDictionary.Remove(building);
            Destroy(building.gameObject);
        }
    }
}