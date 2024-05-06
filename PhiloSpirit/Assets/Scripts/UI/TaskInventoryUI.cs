using Terrain;
using Resources;
using UnityEngine;
using Tasks;

namespace UI
{

    public class TaskInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryList;

        [SerializeField] private ResourceUI _resourceUIPrefab;

        [SerializeField] private Tile _portalTile;

        [SerializeField] private TaskManager _taskManager;

        private void Start()
        {
            _taskManager.costChanged.AddListener(ShowTaskInventory);
        }

        public void ShowTaskInventory()
        {
            foreach (Transform child in _inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            bool resourceFound;

            // For every resoure in the task cost
            foreach (Resource resource in _taskManager.totalCost.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);

                // Search if a resource of the same type is on the portal tile
                resourceFound = false;
                foreach (Resource portalResource in _portalTile.inventory.resources)
                {
                    if (resource.type == portalResource.type)
                    {
                        // If there is, show it
                        resourceUI.Init(resource, portalResource);
                        resourceFound = true;
                    }
                }

                // Else, show a 0 for portal resources
                if (!resourceFound)
                {
                    resourceUI.Init(resource, new Resource(resource.type));
                }
            }
        }

        public bool IsTaskCompletionPossible()
        {
            if (_taskManager == null || _taskManager.totalCost.resources.Count == 0)
                return false;

            bool resourceFound;

            // For every resoure in the task cost
            foreach (Resource resource in _taskManager.totalCost.resources)
            {
                resourceFound = false;
                // Search if a resource of the same type is on the portal tile
                foreach (Resource portalResource  in _portalTile.inventory.resources)
               {
                    if (resource.type == portalResource.type) {
                        // And check if there is enough quantity to "pay" the task cost
                        // If its enough, OK
                        // If it isn't, we can stop and say it's not OK
                        if (portalResource.quantity >= resource.quantity)
                            resourceFound = true;
                        else
                            return false;
                    }
               }

                // If the resource wasn't found at all, not OK
                if (!resourceFound)
                    return false;
            }

            return true;
        }

        public void CompleteTasks()
        {
            _taskManager.CompleteTasks();
        }
    }
}