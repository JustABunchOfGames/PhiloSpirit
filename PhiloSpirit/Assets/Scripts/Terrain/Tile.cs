using UnityEngine;
using Resources;
using UnityEngine.Events;

namespace Terrain
{
    public enum TileType
    {
        Land,
        Portal,
        Forest,
        Mountain
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType _tileType;
        public TileType tileType { get { return _tileType; } private set { } }

        public Inventory inventory;

        public InventoryChangedEvent inventoryChangedEvent = new InventoryChangedEvent();

        public string GetName()
        {
            return _tileType.ToString();
        }

        private void Start()
        {
            inventory.updateEvent.AddListener(InventoryEvent);
        }

        private void InventoryEvent()
        {
            inventoryChangedEvent.Invoke();
        }

        public void CleanInventory()
        {
            inventory.resources.Clear();

            InventoryEvent();
        }
    }

    public class InventoryChangedEvent : UnityEvent { }
}