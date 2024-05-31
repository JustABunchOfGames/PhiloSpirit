using Terrain;
using Transport;
using UnityEngine;

namespace UI
{

    public class TileTransportUI : MonoBehaviour
    {
        [SerializeField] private TransportManager _transportManager;

        private Tile _currentTile;

        public void InitTransport(Tile tile)
        {
            _currentTile = tile;
        }

        public void StartTransport(bool startToEnd)
        {
            _transportManager.StartTransport(_currentTile, startToEnd);
        }
    }
}