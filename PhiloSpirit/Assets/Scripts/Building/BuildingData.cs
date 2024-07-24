using Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building/Data")]
    public class BuildingData : ScriptableObject
    {
        [SerializeField] private uint _id;
        public uint id { get { return _id; } private set { } }

        [SerializeField] private string _buildingName;
        public string buildingName { get { return _buildingName; } private set { } }

        [SerializeField] private List<BuildingTile> _tiles;
        public List<BuildingTile> tiles { get { return _tiles; } private set { } }

        [SerializeField] private BuildingCost _cost;
        public BuildingCost cost { get { return _cost; } private set { } }

        [SerializeField] private Inventory _output;
        public Inventory output { get { return _output; } private set { } }
    }
}