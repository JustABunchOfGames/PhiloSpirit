using Spirits;

namespace Transport
{

    [System.Serializable]
    public class TransportCost
    {
        // Cost
        public float distance {  get; private set; }

        public int resourcesToTransport { get; private set; }
        public int transportCost { get; private set; }
        public int transportCostBonus { get; private set; }

        public int neededWindSpirit { get; private set; }

        public TransportCost(float dist, int costBonus)
        {
            distance = dist;
            resourcesToTransport = 0;
            neededWindSpirit = 0;
            transportCostBonus = costBonus;

            UpdateCost();
        }

        private void UpdateCost()
        {
            int capacity = SpiritManager.transportCapacity;

            transportCost = (int)(resourcesToTransport * distance) - transportCostBonus;
            neededWindSpirit = transportCost / capacity +
                ((transportCost % capacity <= 0 || transportCost % capacity == capacity) ? 0 : 1);
        }

        public void AddResource(int quantity)
        {
            resourcesToTransport += quantity;
            UpdateCost();
        }
    }
}