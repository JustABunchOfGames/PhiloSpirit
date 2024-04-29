using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] private List<Resource> _inventory = new List<Resource>();

        public void Add(Resource item)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (item.type == _inventory[i].type)
                {
                    _inventory[i].quantity += item.quantity;

                    if (_inventory[i].quantity <= 0)
                        _inventory.RemoveAt(i);

                    return;
                }
            }

            _inventory.Add(item);
        }
    }
}