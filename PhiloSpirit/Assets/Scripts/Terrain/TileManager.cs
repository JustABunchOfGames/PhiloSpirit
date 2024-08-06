using Core;
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

        private bool _canSelect;
        private Tile _selectedTile;

        public static TileChanged tileChanged = new TileChanged();

        private void Start()
        {
            _canSelect = true;
            _inputManager.selectEvent.AddListener(SelectTile);
            _inputManager.unselectEvent.AddListener(UnselectTile);
        }

        private void SelectTile()
        {
            if (!_canSelect)
                return;

            GameObject terrain = _inputManager.GetHoveredObjectByTag(Tags.terrainTag);
            if (terrain == null)
                return;

            SetSelectedTile(terrain.GetComponent<Tile>());
        }

        public void UnselectTile()
        {
            _selectedTile = null;
            Destroy(_selectIndicator);
            _selectIndicator = null;
            tileChanged.Invoke(_selectedTile);
        }

        public void SetSelectedTile(Tile tile)
        {
            _selectedTile = tile;
            tileChanged.Invoke(_selectedTile);

            if (_selectIndicator == null)
                _selectIndicator = Instantiate(_selectIndicatorPrefab, transform);

            _selectIndicator.transform.position = _selectedTile.transform.position;
        }

        public void CanSelect(bool canSelect)
        {
            _canSelect = canSelect;

            if (!_canSelect)
                UnselectTile();
        }

        public class TileChanged : UnityEvent<Tile> { }
    }
}