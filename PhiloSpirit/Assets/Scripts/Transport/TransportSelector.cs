using Input;
using Terrain;
using UnityEngine;

namespace Transport
{

    public class TransportSelector : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileManager _tileManager;

        [Header("Scriptable")]
        [SerializeField] private TransportScriptable _scriptable;

        [Header("LineRendering")]
        [SerializeField] private TransportLineRenderer _transportLineRenderer;

        // Tile for transport
        private Tile _fixedTile;

        private void Start()
        {
            _scriptable.selectionStartEvent.AddListener(StartSelection);
            _scriptable.screenConfirmEvent.AddListener(ConfirmTransport);
            _scriptable.screenStartEvent.AddListener(StopInputManager);
        }
        public void StartSelection(Tile tile, TransportWay way)
        {
            _fixedTile = tile;

            // Change behaviour of InputManager to stop selecting tiles
            _inputManager.IsSelecting(false);
            _inputManager.tileClickedEvent.AddListener(TileClicked);

            // Hide TileUI
            _tileManager.SetSelectedTile(null);

            // Fix all lines at the selected tile
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, true);
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, false);

            _transportLineRenderer.Show(true);
            StartCoroutine(_transportLineRenderer.LineRenderingTransport(_inputManager, way));
        }

        private void TileClicked(Tile tile)
        {
            /// Called either by clicking a tile or right-clicking to cancel

            _transportLineRenderer.Show(false);
            _inputManager.tileClickedEvent.RemoveListener(TileClicked);

            // Cancelled
            if (tile == null)
            {
                _inputManager.IsSelecting(true);

                // Reshow tile
                _tileManager.SetSelectedTile(_fixedTile);
            }
            else
            {
                _scriptable.ConfirmSelection(tile);
            }
        }

        private void StopInputManager()
        {
            _inputManager.IsSelecting(false);
        }

        private void ConfirmTransport()
        {
            _inputManager.IsSelecting(true);
            _tileManager.SetSelectedTile(null);

            _transportLineRenderer.Show(false);
        }
    }
}