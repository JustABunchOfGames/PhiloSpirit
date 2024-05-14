using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Spirits
{

    public class SpiritManager : MonoBehaviour
    {
        private Dictionary<SpiritType, SpiritData> _spirits;

        public UpdateSpiritEvent updateSpiritEvent = new UpdateSpiritEvent();

        private void Start()
        {
            _spirits = new Dictionary<SpiritType, SpiritData>
            {
                { SpiritType.Earth, new SpiritData() },
                { SpiritType.Wind, new SpiritData() },
                { SpiritType.Water, new SpiritData() },
                { SpiritType.Fire, new SpiritData() }
            };
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
            _spirits[spiritType].UseSpirit(quantity);
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