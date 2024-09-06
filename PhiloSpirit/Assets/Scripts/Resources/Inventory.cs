using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resources
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] private List<Resource> _inventory = new List<Resource>();
        public List<Resource> resources { get { return GetResources(); } }

        public InventoryUpdateEvent updateEvent = new InventoryUpdateEvent();

        public void Add(Resource item)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (item.type == _inventory[i].type)
                {
                    _inventory[i].quantity += item.quantity;

                    if (_inventory[i].quantity <= 0)
                        _inventory.RemoveAt(i);

                    updateEvent.Invoke();
                    return;
                }
            }

            _inventory.Add(item);
            updateEvent.Invoke();
        }

        public void Remove(Resource item)
        {
            Add(new Resource(item.type, -item.quantity));
        }

        public void Clear()
        {
            _inventory.Clear();
        }

        public void Copy(Inventory inventory)
        {
            _inventory.Clear();
            foreach(Resource res in inventory._inventory)
                _inventory.Add(new Resource(res.type, res.quantity));

            updateEvent.Invoke();
        }

        public bool HasEnough(Resource item)
        {
            foreach(Resource res in resources)
            {
                if (res.type == item.type && res.quantity >= item.quantity)
                    return true;
            }
            return false;
        }

        public bool Contains(ResourceType type)
        {
            foreach(Resource res in resources)
            {
                if (res.type == type)
                    return true;
            }
            return false;
        }

        public int GetQuantity(ResourceType type)
        {
            foreach(Resource res in resources)
            {
                if (res.type == type)
                    return res.quantity;
            }
            return 0;
        }

        public List<Resource> GetResources()
        {
            Inventory inv = new Inventory();
            inv.Copy(this);
            return inv._inventory;
        }
    }

    public class InventoryUpdateEvent : UnityEvent { }
}