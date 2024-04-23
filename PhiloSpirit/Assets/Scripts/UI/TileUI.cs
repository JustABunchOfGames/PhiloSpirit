using Terrain;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TileUI : MonoBehaviour
    {
        [SerializeField] private TileManager _tileManager;

        // UI Element
        [SerializeField] private GameObject _tileUI;

        // Text Area
        [SerializeField] private Text _tileName;
        [SerializeField] private Text _tilePosition;

        private void Start()
        {
            _tileManager.tileChanged.AddListener(ShowTileUI);
        }

        private void ShowTileUI(Tile tile)
        {
            _tileUI.SetActive(true);

            _tileName.text = tile.name;
            _tilePosition.text = tile.transform.position.x + " / " + tile.transform.position.y;
        }
    }
}