using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resources
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] private List<Resource> _inventory = new List<Resource>();
        public List<Resource> resources { get { return _inventory; } }

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
            item.quantity = -item.quantity;

            Add(item);
        }

        public void Copy(Inventory inventory)
        {
            _inventory.Clear();
            foreach(Resource res in inventory.resources)
                _inventory.Add(new Resource(res.type, res.quantity));
        }
    }

    public class InventoryUpdateEvent : UnityEvent { }
}