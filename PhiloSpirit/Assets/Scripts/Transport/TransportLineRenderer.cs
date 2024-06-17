using Input;
using System.Collections;
using Terrain;
using UnityEngine;

namespace Transport
{
    public class TransportLineRenderer : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private bool _isRendering;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void Show(bool show)
        {
            _lineRenderer.enabled = show;
            _isRendering = show;
        }

        public IEnumerator LineRenderingTransport(InputManager inputManager, bool startToEnd)
        {
            Tile tile;
            Vector3 tilePos;
            while (_isRendering)
            {
                yield return new WaitForFixedUpdate();

                tile = inputManager.GetHoveredTile();

                if (tile != null)
                {
                    tilePos = tile.transform.position;

                    if (startToEnd)
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
        public void DrawLineRenderer(Vector3 position, bool start)
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

            points = new Vector3[5] { startPos, endPos, u, v, endPos };
            _lineRenderer.SetPositions(points);
        }
    }
}