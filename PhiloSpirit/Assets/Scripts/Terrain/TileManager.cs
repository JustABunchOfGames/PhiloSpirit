using UnityEngine;
using UnityEngine.Events;

namespace Terrain
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private GameObject _selectIndicatorPrefab;
        private GameObject _selectIndicator;

        private Tile _selectedTile;

        public TileChanged tileChanged = new TileChanged();

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