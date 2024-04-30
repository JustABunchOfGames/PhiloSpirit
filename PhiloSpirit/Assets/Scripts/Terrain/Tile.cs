using UnityEngine;
using Resources;

namespace Terrain
{
    public enum TileType
    {
        Land,
        Portal,
        Forest,
        Mountain
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType _tileType;

        public Inventory inventory;

        public string GetName()
        {
            return _tileType.ToString();
        }
    }
}