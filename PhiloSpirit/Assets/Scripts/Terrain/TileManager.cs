using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Terrain
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private GameObject _selectIndicatorPrefab;
        private GameObject _selectIndicator;

        private Tile _selectedTile;

        public static TileChanged tileChanged = new TileChanged();

        private void Start()
        {
            _inputManager.terrainSelectEvent.AddListener(TerrainSelect);
        }

        private void TerrainSelect(GameObject terrain)
        {
            if (terrain == null)
                SetSelectedTile(null);
            else
                SetSelectedTile(terrain.GetComponent<Tile>());
        }

        public void SetSelectedTile(Tile tile)
        {
            if (tile == null)
            {
                _selectedTile = null;
                Destroy(_selectIndicator);
                _selectIndicator = null;
                tileChanged.Invoke(_selectedTile);
                return;
            }

            _selectedTile = tile;
            tileChanged.Invoke(_selectedTile);

            if (_selectIndicator == null)
                _selectIndicator = Instantiate(_selectIndicatorPrefab, transform);
            
            _selectIndicator.transform.position = _selectedTile.transform.position;
        }

        public class TileChanged : UnityEvent<Tile> { }
    }
}