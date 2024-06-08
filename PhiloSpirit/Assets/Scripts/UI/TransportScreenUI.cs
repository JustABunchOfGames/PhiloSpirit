using Terrain;
using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransportScreenUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TransportManager _transportManager;

        [Header("Screen")]
        [SerializeField] private GameObject _screenGo;

        [Header("Text")]
        [SerializeField] private Text _startTileName;
        [SerializeField] private Text _startTileCoord;
        [SerializeField] private Text _endTileName;
        [SerializeField] private Text _endTileCoord;

        private void Start()
        {
            _transportManager.screenEvent.AddListener(StartTransportScreen);
        }

        private void StartTransportScreen(TransportScreen screen)
        {
            _screenGo.gameObject.SetActive(true);

            Tile startTile = screen.startTile;
            Tile endTile = screen.endTile;

            _startTileName.text = startTile.GetName();
            _startTileCoord.text = startTile.transform.position.x + " / " + startTile.transform.position.y;

            _endTileName.text = endTile.GetName();
            _endTileCoord.text = endTile.transform.position.x + " / " + endTile.transform.position.y;
        }

        public void CancelTransport()
        {
            _transportManager.EndTransport(false);
            _screenGo.gameObject.SetActive(false);
        }
    }
}