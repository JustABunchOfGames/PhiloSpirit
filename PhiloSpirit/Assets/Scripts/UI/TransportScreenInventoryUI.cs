using Resources;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.DefaultControls;

namespace UI
{

    public class TransportScreenInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryList;

        [SerializeField] private TransportResourceUI _resourceUIPrefab;

        private Dictionary<ResourceType, TransportResourceUI> _inventoryDictionary = new Dictionary<ResourceType, TransportResourceUI>();

        public void ShowTransportInventory(Inventory inventory)
        {
            foreach (Transform child in _inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            _inventoryDictionary.Clear();

            foreach(Resource resource in inventory.resources)
            {
                TransportResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);
                resourceUI.Init(resource);

                _inventoryDictionary.Add(resource.type, resourceUI);
            }
        }

        public void UpdateInventoryUI(ResourceType resourceType, bool add)
        {
            if (add)
            {
                if (_inventoryDictionary.ContainsKey(resourceType))
                {
                    _inventoryDictionary[resourceType].Add();
                }
                else
                {
                    TransportResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);
                    Resource resource = new Resource(resourceType, 1);
                    resourceUI.Init(resource);

                    _inventoryDictionary.Add(resource.type, resourceUI);
                }
            }
            else
            {
                bool mustDelete = _inventoryDictionary[resourceType].Substract();

                if (mustDelete)
                {
                    Destroy(_inventoryDictionary[resourceType].gameObject);
                    _inventoryDictionary.Remove(resourceType);
                    
                }
            }
        }
    }
}