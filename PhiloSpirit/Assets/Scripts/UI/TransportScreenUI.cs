using Resources;
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
        [SerializeField] private TransportScreenScriptable _scriptable;
        [SerializeField] private GameObject _screenGo;
        [SerializeField] private TransportScreenInventoryUI _tileInventory;
        [SerializeField] private TransportScreenInventoryUI _transportInventory;

        [Header("Text")]
        [SerializeField] private Text _startTileName;
        [SerializeField] private Text _startTileCoord;
        [SerializeField] private Text _endTileName;
        [SerializeField] private Text _endTileCoord;

        private void Start()
        {
            _scriptable.screenStartEvent.AddListener(StartTransportScreen);
            _scriptable.screenUpdateEvent.AddListener(UpdateTransportScreen);
        }

        private void StartTransportScreen()
        {
            // Show Screen
            _screenGo.gameObject.SetActive(true);

            Tile startTile = _scriptable.startTile;
            Tile endTile = _scriptable.endTile;

            // Name and coords of tiles
            _startTileName.text = startTile.GetName();
            _startTileCoord.text = startTile.transform.position.x + " / " + startTile.transform.position.y;

            _endTileName.text = endTile.GetName();
            _endTileCoord.text = endTile.transform.position.x + " / " + endTile.transform.position.y;

            // Inventories for transport
            _tileInventory.ShowTransportInventory(_scriptable.GetInventory(false));
            _transportInventory.ShowTransportInventory(_scriptable.GetInventory(true));
        }

        private void UpdateTransportScreen(ResourceType resourceType, bool transport)
        {
            _tileInventory.UpdateInventoryUI(resourceType, !transport);
            _transportInventory.UpdateInventoryUI(resourceType, transport);
        }

        // Called from a button
        public void CancelTransport()
        {
            _transportManager.EndTransport(false);
            _screenGo.gameObject.SetActive(false);
        }
    }
}