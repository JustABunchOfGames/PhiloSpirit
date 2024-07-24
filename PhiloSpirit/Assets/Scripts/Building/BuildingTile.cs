using Terrain;
using UnityEngine;

namespace Building
{
    [System.Serializable]
    public class BuildingTile
    {
        public Vector3 coord;
        public TileType neededTileType;
    }
}