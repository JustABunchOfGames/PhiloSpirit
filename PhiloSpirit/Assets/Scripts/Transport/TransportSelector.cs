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
            _inputManager.CanSelectTile(false);
            _inputManager.terrainClickedEvent.AddListener(TerrainClicked);

            // Hide TileUI
            _tileManager.SetSelectedTile(null);

            // Fix all lines at the selected tile
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, true);
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, false);

            _transportLineRenderer.Show(true);
            StartCoroutine(_transportLineRenderer.LineRenderingTransport(_inputManager, way));
        }

        private void TerrainClicked(GameObject terrain)
        {
            /// Called either by clicking a tile or right-clicking to cancel

            _transportLineRenderer.Show(false);
            _inputManager.terrainClickedEvent.RemoveListener(TerrainClicked);

            // Cancelled
            if (terrain == null)
            {
                _inputManager.CanSelectTile(true);

                // Reshow tile
                _tileManager.SetSelectedTile(_fixedTile);
            }
            else
            {
                _scriptable.ConfirmSelection(terrain.GetComponent<Tile>());
            }
        }

        private void StopInputManager()
        {
            _inputManager.CanSelectTile(false);
        }

        private void ConfirmTransport()
        {
            _inputManager.CanSelectTile(true);
            _tileManager.SetSelectedTile(null);

            _transportLineRenderer.Show(false);
        }
    }
}