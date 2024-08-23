using Resources;
using UnityEngine;
using Tasks;

namespace UI
{
    public class TaskInventoryUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TaskManager _taskManager;

        [Header("UI")]
        [SerializeField] private GameObject _inventoryList;
        [SerializeField] private ResourceUI _resourceUIPrefab;

        private void Start()
        {
            _taskManager.costChanged.AddListener(ShowTaskInventory);
        }

        public void ShowTaskInventory(Inventory portalInventory, Inventory taskInventory)
        {
            // Clean UI List
            foreach (Transform child in _inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            // For every resoure in the task cost
            foreach (Resource resource in _taskManager.totalCost.resources)
            {
                // Instantiate resource UI
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);

                // Set type and quantities
                resourceUI.Init(resource, new Resource(resource.type, portalInventory.GetQuantity(resource.type)));
            }
        }
    }
}