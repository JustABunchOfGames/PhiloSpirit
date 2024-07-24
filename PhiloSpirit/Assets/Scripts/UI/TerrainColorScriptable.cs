using System.Collections.Generic;
using Terrain;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "Colors", menuName = "UI/Terrain/Colors")]
    public class TerrainColorScriptable : ScriptableObject
    {
        [SerializeField] private List<TerrainColor> _terrainList;

        public Color GetTerrainColor(TileType tileType)
        {
            foreach (TerrainColor terrainColor in _terrainList)
            {
                if (terrainColor.type == tileType)
                    return terrainColor.color;
            }
            return Color.white;
        }
    }

    [System.Serializable]
    public struct TerrainColor
    {
        public TileType type;
        public Color color;
    }
}