using Terrain;
using Resources;
using UnityEngine;
using Tasks;
using UnityEngine.Events;

namespace UI
{

    public class TaskInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryList;

        [SerializeField] private ResourceUI _resourceUIPrefab;

        [SerializeField] private Tile _portalTile;

        [SerializeField] private TaskManager _taskManager;

        public CostChangedEvent costChangedEvent = new CostChangedEvent();

        private void Start()
        {
            _taskManager.costChanged.AddListener(ShowTaskInventory);
            _portalTile.inventoryChangedEvent.AddListener(ShowTaskInventory);
        }

        public void ShowTaskInventory()
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
                resourceUI.Init(resource, new Resource(resource.type, _portalTile.inventory.GetQuantity(resource.type)));
            }

            costChangedEvent.Invoke();
        }

        public bool IsTaskCompletionPossible()
        {
            if (_taskManager == null || _taskManager.totalCost.resources.Count == 0)
                return false;

            // For every resoure in the task cost
            foreach (Resource resource in _taskManager.totalCost.resources)
            {
                if (!_portalTile.inventory.HasEnough(resource))
                    return false;
            }

            return true;
        }

        public void CompleteTasks()
        {
            _taskManager.CompleteTasks();
        }

        public class CostChangedEvent : UnityEvent { }
    }
}