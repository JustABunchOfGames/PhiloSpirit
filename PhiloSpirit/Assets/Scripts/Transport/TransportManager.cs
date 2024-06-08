using Input;
using System.Collections;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Transport
{

    public class TransportManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileManager _tileManager;

        [Header("LineRendering")]
        [SerializeField] private LineRenderer _lineRenderer;

        // Tile for transport
        private Tile _startTile;
        private Tile _endTile;

        // Bool to know the direction of the transport
        private bool _startToEnd;

        // Stop Rendering on the coroutine
        private bool _isRendering;

        // Event for UI
        public TransportScreenEvent screenEvent = new TransportScreenEvent();

        public void StartTransport(Tile tile, bool startToEnd)
        {
            _startToEnd = startToEnd;

            if (_startToEnd)
                _startTile = tile;
            else
                _endTile = tile;

            // Change behaviour of InputManager to stop selecting tiles
            _inputManager.IsSelecting(false);
            _inputManager.tileClickedEvent.AddListener(TileClicked);

            // Hide TileUI
            _tileManager.SetSelectedTile(null);

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

                // Reshow tile
                _tileManager.SetSelectedTile(_startToEnd ? _startTile : _endTile);
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

                screenEvent.Invoke(new TransportScreen(_startTile, _endTile));
            }
        }

        public void EndTransport(bool confirmed)
        {
            if (!confirmed)
            {
                if (_startToEnd)
                    StartTransport(_startTile, _startToEnd);
                else
                    StartTransport(_endTile, _startToEnd);
            }
            else
            {
                Debug.Log("Confirm Transport");
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

        public class TransportScreenEvent : UnityEvent<TransportScreen> { }
    }
}