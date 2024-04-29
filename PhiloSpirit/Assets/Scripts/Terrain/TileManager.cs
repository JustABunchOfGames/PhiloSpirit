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
            _selectedTile = tile;
            tileChanged.Invoke(_selectedTile);

            if (_selectIndicator == null)
                _selectIndicator = Instantiate(_selectIndicatorPrefab, transform);
            
            _selectIndicator.transform.position = _selectedTile.transform.position;
        }

        public Tile GetSelectedTile()
        {
            return _selectedTile;
        }

        public class TileChanged : UnityEvent<Tile> { }
    }
}