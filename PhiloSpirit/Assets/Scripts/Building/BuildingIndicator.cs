using Input;
using System.Collections;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Building {
    public class BuildingIndicator : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileManager _tileManager;

        [Header("Indicator")]
        [SerializeField] private GameObject _indicatorGo;
        [SerializeField] private BuildingTileIndicator _indicatorPrefab;

        private bool _isChecking;
        private bool _allCheckOK;

        private BuildingData _currentData;

        public IndicatorComplete completeEvent = new IndicatorComplete();
        public IndicatorCancelled cancelEvent = new IndicatorCancelled();

        public void StartIndicator(BuildingData data)
        {
            _currentData = data;

            // Change behaviour of InputManager to stop selecting tiles
            _inputManager.CanSelectTile(false);
            _inputManager.terrainClickedEvent.AddListener(TerrainClicked);
            _inputManager.scrollEvent.AddListener(RotateIndicator);

            // Hide TileUI
            _tileManager.SetSelectedTile(null);

            _indicatorGo.transform.position = new Vector3(0,0,-1);

            foreach (BuildingTile tile in data.tiles)
            {
                BuildingTileIndicator tileIndicator = Instantiate(_indicatorPrefab,tile.coord, Quaternion.identity, _indicatorGo.transform);
                tileIndicator.Init(tile.neededTileType);
            }

            _isChecking = true;
            StartCoroutine(CheckCoroutine());
        }

        private void TerrainClicked(GameObject terrain)
        {
            // Clicked with tile bad alignment
            if (terrain != null && !_allCheckOK)
                return;

            // Called either by clicking a tile or right-clicking to cancel
            _isChecking = false;
            _inputManager.terrainClickedEvent.RemoveListener(TerrainClicked);

            DestroyIndicator();

            // Cancelled
            if (terrain == null)
            {
                // Reshow BuilingUI
                cancelEvent.Invoke();

                _inputManager.CanSelectTile(true);
            }
            // Complete
            else if (_allCheckOK)
            {
                // Show BuildCost screen to confirm building
                completeEvent.Invoke(_currentData, terrain.GetComponent<Tile>());
            }
        }

        private void RotateIndicator(float direction)
        {
            if (direction == 0)
                return;

            if (direction < 0)
            {
                _indicatorGo.transform.Rotate(transform.forward, -90f);
            }
            else if (direction > 0)
            {
                _indicatorGo.transform.Rotate(transform.forward, 90f);
            }
        }

        private void DestroyIndicator()
        {
            foreach (Transform tile in _indicatorGo.transform)
            {
                Destroy(tile.gameObject);
            }
        }

        private IEnumerator CheckCoroutine()
        {
            GameObject terrain;

            while (_isChecking)
            {
                yield return new WaitForFixedUpdate();

                terrain = _inputManager.GetHoveredTerrain();

                if (terrain != null)
                {
                    _indicatorGo.transform.position = terrain.transform.position + transform.forward * -1;

                    Check();
                }
            }
        }

        public void Check()
        {
            _allCheckOK = true;
            foreach (Transform tile in _indicatorGo.transform)
            {
                bool check = tile.GetComponent<BuildingTileIndicator>().Check();

                if (!check)
                    _allCheckOK = false;
            }
        }

        public void BuildingComplete()
        {
            _inputManager.CanSelectTile(true);
        }

        public class IndicatorComplete : UnityEvent<BuildingData, Tile> { }
        public class IndicatorCancelled : UnityEvent { }
    }
}