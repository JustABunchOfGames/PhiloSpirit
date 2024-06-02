using Input;
using System.Collections;
using System.Xml.Linq;
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

        private bool _isRendering;

        public void StartTransport(Tile tile, bool startToEnd)
        {
            _startToEnd = startToEnd;

            if (_startToEnd)
                _startTile = tile;
            else
                _endTile = tile;

            _inputManager.IsSelecting(false);
            _inputManager.tileClickedEvent.AddListener(TileClicked);

            // Fix all lines at the selected tile
            DrawLineRenderer(tile.transform.position, true);
            DrawLineRenderer(tile.transform.position, false);

            _lineRenderer.gameObject.SetActive(true);

            _isRendering = true;
            StartCoroutine(LineRenderingTransport());
            
        }

        private void TileClicked(Tile tile)
        {
            _isRendering = false;
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
            while (_isRendering)
            {
                yield return new WaitForFixedUpdate();

                tile = _inputManager.GetHoveredTile();

                if (tile != null)
                {
                    tilePos = tile.transform.position;

                    if (_startToEnd)
                    {
                        // Modifying end position
                        DrawLineRenderer(tilePos, false);
                    }
                    else
                    {
                        // Modifying start position
                        DrawLineRenderer(tilePos, true);
                    }
                }
            }
        }

        /// <summary> Modify start/end of lineRenderer while drawing an arrow </summary>
        private void DrawLineRenderer(Vector3 position, bool start)
        {
            
            
            Vector3[] points;
            Vector3 startPos;
            Vector3 endPos;

            // Modifying start position
            if (start)
            {
                startPos = position;
                endPos = _lineRenderer.GetPosition(1);
            }
            // Modifying end position
            else
            {
                startPos = _lineRenderer.GetPosition(0);
                endPos = position;
            }

            int angle = 20;
            float dist = 0.2f;

            Vector3 vector = (endPos - startPos) * dist;
            Vector3 u = endPos - (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.Normalize(vector) * dist);
            Vector3 v = endPos - (Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.Normalize(vector) * dist);

            points = new Vector3[5] { position, endPos, u, v, endPos };
            _lineRenderer.SetPositions(points);
        }
    }
}