using Core;
using Input;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Terrain
{
    public class TileManager : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputManager _inputManager;

        [Header("Indicator")]
        [SerializeField] private GameObject _selectIndicatorPrefab;
        private GameObject _selectIndicator;

        private bool _canSelect;
        private Tile _selectedTile;

        [Header("SpawnLand")]
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Vector2 _mapsize;

        public static TileChanged tileChanged = new TileChanged();

        private void Start()
        {
            _canSelect = true;
            _inputManager.selectEvent.AddListener(SelectTile);
            _inputManager.unselectEvent.AddListener(UnselectTile);

            CleanManager.cleanCycleEvent.AddListener(CleanInventory);
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

        public void CleanInventory()
        {
            foreach (Transform child in transform)
            {
                Tile tile = child.GetComponent<Tile>();

                if (tile != null)
                {
                    tile.CleanInventory();
                }
            }
        }

        public void SpawnLand()
        {
            for(float i = -_mapsize.x; i <= _mapsize.x; i++)
            {
                for (float j = -_mapsize.y; j <= _mapsize.y; j++)
                {
                    GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(_tilePrefab, transform);
                    tile.transform.position = new Vector3(i, j);
                    tile.name = "Land " + i + "_" + j;
                }
            }
        }

        public class TileChanged : UnityEvent<Tile> { }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(TileManager))]
    public class TileManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TileManager manager = (TileManager)target;
            if (GUILayout.Button("SpawnLand"))
            {
                manager.SpawnLand();
            }
        }
    }
#endif
}