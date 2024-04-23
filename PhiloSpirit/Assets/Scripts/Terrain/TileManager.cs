using UnityEngine;
using UnityEngine.Events;

namespace Terrain
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private Tile _selectedTile;

        public TileChanged tileChanged = new TileChanged();

        public void SetSelectedTile(Tile tile)
        {
            if (_selectedTile != tile && _selectedTile != null)
                _selectedTile.Unselect();

            _selectedTile = tile;
            _selectedTile.GetSelected();

            tileChanged.Invoke(_selectedTile);
        }

        public Tile GetSelectedTile()
        {
            return _selectedTile;
        }

        public class TileChanged : UnityEvent<Tile> { }
    }
}