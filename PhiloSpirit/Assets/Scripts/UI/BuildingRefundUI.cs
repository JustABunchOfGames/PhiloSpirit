using Building;
using Resources;
using Terrain;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingRefundUI : MonoBehaviour
    {
        [Header("Building")]
        [SerializeField] private BuildingCostManager _costManager;

        [Header("Screen")]
        [SerializeField] private GameObject _screen;

        [Header("List")]
        [SerializeField] private GameObject _costList;
        [SerializeField] private GameObject _inventoryList;

        [Header("ResourceUI")]
        [SerializeField] private ResourceUI _resourceUIPrefab;

        [Header("ConfirmButton")]
        [SerializeField] private Button _confirmButton;

        private void Start()
        {
            _costManager.showRefundEvent.AddListener(StartScreen);
        }

        private void StartScreen(BuildingData data, Tile tile, bool isRefundPossible)
        {
            // Show screen
            _screen.SetActive(true);

            // Set button to interactable if confirm possible
            _confirmButton.interactable = isRefundPossible;

            // Reset List
            ClearList(_costList);
            ClearList(_inventoryList);

            // Populate lists
            foreach (Resource resource in data.output.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _costList.transform);
                resourceUI.Init(resource);
            }

            foreach (Resource resource in tile.inventory.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);
                resourceUI.Init(resource);
            }
        }

        private void ClearList(GameObject list)
        {
            foreach (Transform child in list.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Button Functions
        public void Confirm()
        {
            // Hide screen
            _screen.SetActive(false);

            _costManager.ConfirmRefund();
        }

        public void Cancel()
        {
            // Hide screen
            _screen.SetActive(false);

            _costManager.CancelRefund();
        }
    }
}