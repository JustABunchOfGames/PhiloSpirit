using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Spirits
{

    public class SpiritManager : MonoBehaviour
    {
        private Dictionary<SpiritType, SpiritData> _spirits;

        public UpdateSpiritEvent updateSpiritEvent = new UpdateSpiritEvent();

        private void Awake()
        {
            _spirits = new Dictionary<SpiritType, SpiritData>
            {
                { SpiritType.Earth, new SpiritData(SpiritType.Earth) },
                { SpiritType.Wind, new SpiritData(SpiritType.Wind) },
                { SpiritType.Water, new SpiritData(SpiritType.Water) },
                { SpiritType.Fire, new SpiritData(SpiritType.Fire) }
            };
        }

        private void Start()
        {
            foreach (SpiritData spiritData in _spirits.Values)
            {
                updateSpiritEvent.Invoke(spiritData, 0);
            }
        }

        public void AddSpirit(SpiritType spiritType)
        {
            // Add 1 to max spirit and usable spirit // Update costs
            _spirits[spiritType].AddSpirit();

            // Event for UI and Task cost change
            updateSpiritEvent.Invoke(_spirits[spiritType], 1);
        }

        public void RemoveSpirit(SpiritType spiritType)
        {
            // Check if we can remove a spirit from the usableSpirit pool
            if (_spirits[spiritType].usableSpirit < 0)
                return;

            // Substract 1 to max spirit and usable spirit // Update costs
            _spirits[spiritType].RemoveSpirit();

            // Event for UI and Task cost change
            updateSpiritEvent.Invoke(_spirits[spiritType], -1);
        }

        public void UsePirit(SpiritType spiritType, int quantity)
        {
            if (CanUseSpirit(spiritType, quantity))
            {
                _spirits[spiritType].UseSpirit(quantity);
                updateSpiritEvent.Invoke(_spirits[spiritType], 0);
            }
        }

        public bool CanUseSpirit(SpiritType spiritType, int quantity)
        {
            return _spirits[spiritType].usableSpirit > quantity;
        }

        public int RecalculateCost()
        {
            int cost = 0;

            foreach (SpiritData spiritData in _spirits.Values)
            {
                cost += spiritData.GetTotalCost();
            }

            return cost;
        }
    }

    public class UpdateSpiritEvent : UnityEvent<SpiritData, int> { }
}