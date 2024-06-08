using Terrain;
using Resources;
using UnityEngine;

namespace UI
{

    public class TransportScreenInventory : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryList;

        [SerializeField] private ResourceUI _resourceUIPrefab;

        public void ShowTransportInventory(Tile tile)
        {
            foreach (Transform child in _inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Resource resource in tile.inventory.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);
                resourceUI.Init(resource);
            }
        }
    }
}