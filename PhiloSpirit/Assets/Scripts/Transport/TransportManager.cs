using Input;
using System.Collections;
using Terrain;
using UnityEngine;

namespace Transport
{

    public class TransportManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private LineRenderer _lineRenderer;

        // Tile for transport
        private Tile _startTile;
        private Tile _endTile;
        private bool _startToEnd;

        public void StartTransport(Tile tile, bool startToEnd)
        {
            _startToEnd = startToEnd;

            if (_startToEnd)
                _startTile = tile;
            else
                _endTile = tile;

            _inputManager.IsSelecting(false);
            _inputManager.tileClickedEvent.AddListener(TileClicked);

            // _lineRenderer.positionCount = 0;

            if (_startToEnd)
                _lineRenderer.SetPosition(0, tile.transform.position);
            else
                _lineRenderer.SetPosition(1, tile.transform.position);

            _lineRenderer.gameObject.SetActive(true);

            StartCoroutine(LineRenderingTransport());
        }

        private void TileClicked(Tile tile)
        {
            StopCoroutine(LineRenderingTransport());
            _inputManager.tileClickedEvent.RemoveListener(TileClicked);
            _inputManager.IsSelecting(true);

            if (tile == null)
            {
                _lineRenderer.gameObject.SetActive(false);
            }
            else
            {
                if (_startToEnd)
                {
                    _endTile = tile;
                }
                else
                {
                    _startTile = tile;
                }
            }
        }

        private IEnumerator LineRenderingTransport()
        {
            Tile tile;
            Vector3 tilePos;
            while (true)
            {
                yield return new WaitForFixedUpdate();

                tile = _inputManager.GetHoveredTile();

                if (tile != null) {
                    tilePos = tile.transform.position;
                    
                    if (_startToEnd)
                    {
                        _lineRenderer.SetPosition(0, tilePos);
                    }
                    else
                    {
                        _lineRenderer.SetPosition(1, tilePos);
                    }
                }
            }
        }
    }
}