using Resources;
using Terrain;
using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransportScreenUI : MonoBehaviour
    {
        [Header("Scriptable")]
        [SerializeField] private TransportScriptable _scriptable;

        [Header("Screen")]
        [SerializeField] private GameObject _screenGo;
        [SerializeField] private TransportScreenInventoryUI _startTileInventory;
        [SerializeField] private TransportScreenInventoryUI _transportInventory;
        [SerializeField] private TransportScreenInventoryUI _endTileInventory;

        [Header("TileText")]
        [SerializeField] private Text _startTileName;
        [SerializeField] private Text _startTileCoord;
        [SerializeField] private Text _endTileName;
        [SerializeField] private Text _endTileCoord;

        [Header("StateUI")]
        [SerializeField] private TransportScreenStateUI _stateUI;

        private void Start()
        {
            _scriptable.screenStartEvent.AddListener(StartTransportScreen);
            _scriptable.screenUpdateEvent.AddListener(UpdateTransportScreen);
            _scriptable.screenConfirmEvent.AddListener(HideScreen);
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

            // State Specific UI
            if (_scriptable.state == TransportState.Show)
                _stateUI.Init(_scriptable.state, true, _scriptable.IsDeletePossible());
            else
                _stateUI.Init(_scriptable.state, false, false);
            UpdateStateScreen();

            // Inventories for transport, copied to not affect tile inventory (yet)
            _startTileInventory.ShowTransportInventory(_scriptable.tileInventory);
            _transportInventory.ShowTransportInventory(_scriptable.transportInventory);

            // Not a copy, we just show it but don't touch it
            _endTileInventory.ShowTransportInventory(_scriptable.endTile.inventory);
        }

        private void UpdateTransportScreen(ResourceType resourceType, bool transport)
        {
            _startTileInventory.UpdateInventoryUI(resourceType, !transport);
            _transportInventory.UpdateInventoryUI(resourceType, transport);

            UpdateStateScreen();
        }

        // Called from a button
        public void QuitTransportScreen()
        {
            _scriptable.CancelTransport();
            HideScreen();
        }

        public void HideScreen()
        {
            _screenGo.gameObject.SetActive(false);
        }

        private void UpdateStateScreen()
        {
            _stateUI.UpdateText(_scriptable.cost.transportCost.ToString("0.0"), _scriptable.cost.neededWindSpirit.ToString());
        }
    }
}