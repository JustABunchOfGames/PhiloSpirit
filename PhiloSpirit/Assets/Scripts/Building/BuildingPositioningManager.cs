using Core;
using Input;
using System.Collections;
using Terrain;
using UnityEngine;

namespace Building {
    public class BuildingPositioningManager : MonoBehaviour
    {
        [Header("Managers")]
        private BuildingManager _buildingManager;
        [SerializeField] private InputManager _inputManager;

        [Header("Indicator")]
        [SerializeField] private GameObject _indicatorGo;
        [SerializeField] private BuildingTileIndicator _indicatorPrefab;

        private bool _isChecking;
        private bool _allCheckOK;

        private BuildingData _currentData;

        private void Awake()
        {
            _buildingManager = GetComponent<BuildingManager>();
        }

        private void Start()
        {
            _buildingManager.buildingPositioningEvent.AddListener(StartIndicator);
        }

        public void StartIndicator(BuildingData data)
        {
            _currentData = data;

            // Subscribe to used events
            _inputManager.selectEvent.AddListener(Select);
            _inputManager.unselectEvent.AddListener(Unselect);
            _inputManager.scrollEvent.AddListener(RotateIndicator);

            _indicatorGo.transform.position = new Vector3(0,0,-1);

            foreach (BuildingTile tile in data.tiles)
            {
                BuildingTileIndicator tileIndicator = Instantiate(_indicatorPrefab,tile.coord, Quaternion.identity, _indicatorGo.transform);
                tileIndicator.Init(tile.neededTileType);
            }

            _isChecking = true;
            StartCoroutine(CheckCoroutine());
        }

        private void Select()
        {
            // Get Selected item
            GameObject terrain = _inputManager.GetHoveredObjectByTag(Tags.terrainTag);

            if (terrain == null)
                return;

            // Clicked with bad alignment
            if (!_allCheckOK)
                return;

            // Stop Checking
            _isChecking = false;

            RemoveListeners();

            DestroyIndicator();

            // Complete
            if (_allCheckOK)
            {
                // Confirm Placement
                _buildingManager.SetPosition(terrain.GetComponent<Tile>(), _indicatorGo.transform.rotation);
            }
        }

        private void Unselect()
        {
            RemoveListeners();

            DestroyIndicator();

            // Reshow BuilingUI
            _buildingManager.CancelPositioning();
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

        private void RemoveListeners()
        {
            // Remove listeners
            _inputManager.selectEvent.RemoveListener(Select);
            _inputManager.unselectEvent.RemoveListener(Unselect);
            _inputManager.scrollEvent.RemoveListener(RotateIndicator);
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

                terrain = _inputManager.GetHoveredObjectByTag(Tags.terrainTag);

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
    }
}