using Core;
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

            // Subscribe to used events
            _inputManager.selectEvent.AddListener(Select);
            _inputManager.unselectEvent.AddListener(Unselect);

            // Change behaviour to forbid tile selection
            _tileManager.CanSelect(false);

            // Fix all lines at the selected tile
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, true);
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, false);

            _transportLineRenderer.Show(true);
            StartCoroutine(_transportLineRenderer.LineRenderingTransport(_inputManager, way));
        }

        private void Select()
        {
            // Get Selected item
            GameObject terrain = _inputManager.GetHoveredObjectByTag(Tags.terrainTag);

            if (terrain == null)
                return;

            _transportLineRenderer.Show(false);

            RemoveListeners();

            _scriptable.ConfirmSelection(terrain.GetComponent<Tile>());
        }

        private void Unselect()
        {
            RemoveListeners();

            _transportLineRenderer.Show(false);

            // Change behaviour to allow tile selection
            _tileManager.CanSelect(true);

            // Reshow tile
            _tileManager.SetSelectedTile(_fixedTile);
        }

        private void RemoveListeners()
        {
            _inputManager.selectEvent.RemoveListener(Select);
            _inputManager.unselectEvent.RemoveListener(Unselect);
        }

        private void ConfirmTransport()
        {
            // Change behaviour to allow tile selection
            _tileManager.CanSelect(true);
        }

        private void StopInputManager()
        {
            // Change behaviour to forbid tile selection
            _tileManager.CanSelect(false);
        }
    }
}