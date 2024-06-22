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

        [Header("Cost")]
        [SerializeField] private Text _transportCostQuantity;
        [SerializeField] private Text _neededSpiritQuantity;
        [SerializeField] private Button _confirmButton;

        [Header("Confirm")]
        [SerializeField] private GameObject _confirmBox;
        [SerializeField] private Toggle _ignoreConfirmBoxToggle;

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
            _confirmBox.gameObject.SetActive(false);

            Tile startTile = _scriptable.startTile;
            Tile endTile = _scriptable.endTile;

            // Name and coords of tiles
            _startTileName.text = startTile.GetName();
            _startTileCoord.text = startTile.transform.position.x + " / " + startTile.transform.position.y;

            _endTileName.text = endTile.GetName();
            _endTileCoord.text = endTile.transform.position.x + " / " + endTile.transform.position.y;

            // Cost
            UpdateCost();

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

            UpdateCost();
        }

        // Called from a button
        public void CancelTransport()
        {
            _scriptable.CancelTransport();
            HideScreen();
        }

        public void HideScreen()
        {
            _screenGo.gameObject.SetActive(false);
        }

        // Called form a button
        public void ShowConfirmBox()
        {
            if (_ignoreConfirmBoxToggle.isOn)
                _scriptable.ConfirmTransport();
            else
                _confirmBox.gameObject.SetActive(true);
        }        

        private void UpdateCost()
        {
            _transportCostQuantity.text = _scriptable.cost.transportCost.ToString();

            _neededSpiritQuantity.text = _scriptable.cost.neededWindSpirit.ToString();
        }
    }
}