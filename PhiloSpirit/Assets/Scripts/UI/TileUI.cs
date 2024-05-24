using Terrain;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TileUI : MonoBehaviour
    {
        [SerializeField] private TileManager _tileManager;

        [Header("UI Objects")]
        [SerializeField] private GameObject _tileUI;
        [SerializeField] private TileInventoryUI _tileInventoryUI;
        [SerializeField] private TileTransportUI _tileTransportUI;

        [Header("Text area")]
        [SerializeField] private Text _tileName;
        [SerializeField] private Text _tilePosition;

        private void Start()
        {
            _tileManager.tileChanged.AddListener(ShowTileUI);
        }

        private void ShowTileUI(Tile tile)
        {
            if (tile == null)
            {
                HideTileUI();
                return;
            }

            _tileUI.SetActive(true);

            _tileName.text = tile.GetName();
            _tilePosition.text = tile.transform.position.x + " / " + tile.transform.position.y;

            _tileInventoryUI.ShowTileInventory(tile);

            _tileTransportUI.gameObject.SetActive(false);
            _tileInventoryUI.gameObject.SetActive(true);
        }

        private void HideTileUI()
        {
            _tileUI.SetActive(false);
        }
    }
}