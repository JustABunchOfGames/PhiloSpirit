using Input;
using System.Collections;
using System.Collections.Generic;
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
            _inputManager.IsSelecting(false);
            _inputManager.tileClickedEvent.AddListener(TileClicked);

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

        private void TileClicked(Tile tile)
        {
            // Clicked with tile bad alignment
            if (tile != null && !_allCheckOK)
                return;

            /// Called either by clicking a tile or right-clicking to cancel
            DestroyIndicator();
            _inputManager.tileClickedEvent.RemoveListener(TileClicked);

            _inputManager.IsSelecting(true);

            // Cancelled
            if (tile == null)
            {
                // Reshow BuilingUI
                cancelEvent.Invoke();
            }
            // Complete
            else if (_allCheckOK)
            {
                // Show BuildCost screen to confirm building
                completeEvent.Invoke(_currentData);
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
            Tile tile;

            while (_isChecking)
            {
                yield return new WaitForFixedUpdate();

                tile = _inputManager.GetHoveredTile();

                if (tile != null)
                {
                    _indicatorGo.transform.position = tile.transform.position + transform.forward * -1;

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

        public class IndicatorComplete : UnityEvent<BuildingData> { }
        public class IndicatorCancelled : UnityEvent { }
    }
}