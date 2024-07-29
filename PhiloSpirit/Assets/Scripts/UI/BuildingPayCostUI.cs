using Building;
using Resources;
using Terrain;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingPayCostUI : MonoBehaviour
    {
        [Header("Building")]
        [SerializeField] private BuildingIndicator _indicator;

        [Header("Screen")]
        [SerializeField] private GameObject _screen;

        [Header("List")]
        [SerializeField] private GameObject _costList;
        [SerializeField] private GameObject _inventoryList;

        [Header("ResourceUI")]
        [SerializeField] private ResourceUI _resourceUIPrefab;

        [Header("ConfirmButton")]
        [SerializeField] private Button _confirmButton;

        private BuildingData _currentData;
        private Tile _currentTile;

        private void Start()
        {
            _indicator.completeEvent.AddListener(StartScreen);
        }

        private void StartScreen(BuildingData data, Tile tile)
        {
            // Save data for building later
            _currentData = data;
            _currentTile = tile;

            // Show screen
            _screen.SetActive(true);

            // Set button to interactable (for now)
            _confirmButton.interactable = true;

            // Populate lists
            foreach (Resource resource in data.cost.resourceCost.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _costList.transform);
                resourceUI.Init(resource);

                // Test at the same time if we can complete building
                if (_confirmButton.interactable && !tile.inventory.HasEnough(resource))
                    _confirmButton.interactable = false;
            }

            foreach(Resource resource in tile.inventory.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);
                resourceUI.Init(resource);
            }
        }

        // Button Functions
        public void Confirm()
        {
            // Hide screen
            _screen.SetActive(false);

            _indicator.BuildingComplete();
        }

        public void Cancel()
        {
            // Hide screen
            _screen.SetActive(false);

            _indicator.StartIndicator(_currentData);
        }
    }
}