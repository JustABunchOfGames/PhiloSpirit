using Input;
using Terrain;
using UnityEngine;

namespace Transport
{

    public class TransportManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileManager _tileManager;

        [Header("Scriptable")]
        [SerializeField] private TransportScreenScriptable _scriptable;

        [Header("LineRendering")]
        [SerializeField] private TransportLineRenderer _transportLineRenderer;

        // Tile for transport
        private Tile _startTile;
        private Tile _endTile;

        // Bool to know the direction of the transport
        private bool _startToEnd;

        // Transport Logs
        public TransportLogDictionary logDictionary { get; private set; }

        private void Start()
        {
            logDictionary = new TransportLogDictionary();

            _scriptable.screenConfirmEvent.AddListener(ConfirmTransport);
        }
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
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, true);
            _transportLineRenderer.DrawLineRenderer(tile.transform.position, false);

            _transportLineRenderer.Show(true);
            StartCoroutine(_transportLineRenderer.LineRenderingTransport(_inputManager, _startToEnd));
        }

        private void TileClicked(Tile tile)
        {
            _transportLineRenderer.Show(false);
            _inputManager.tileClickedEvent.RemoveListener(TileClicked);

            if (tile == null)
            {
                _inputManager.IsSelecting(true);

                // Reshow tile
                _tileManager.SetSelectedTile(_startToEnd ? _startTile : _endTile);
            }
            else
            {
                TransportLogLists logLists;
                if (_startToEnd)
                {
                    _endTile = tile;
                    logDictionary.Add(_startTile.transform.position);
                    logLists = logDictionary.GetLogs(_startTile.transform.position);
                }
                else
                {
                    _startTile = tile;
                    logDictionary.Add(_endTile.transform.position);
                    logLists = logDictionary.GetLogs(_endTile.transform.position);
                }
                _scriptable.InitScreen(_startTile, _endTile, logLists);
            }
        }

        public void CancelTransport()
        {
            if (_startToEnd)
                StartTransport(_startTile, _startToEnd);
            else
                StartTransport(_endTile, _startToEnd);
        }

        public void ConfirmTransport()
        {
            _inputManager.IsSelecting(true);

            _transportLineRenderer.Show(false);
        }
    }
}