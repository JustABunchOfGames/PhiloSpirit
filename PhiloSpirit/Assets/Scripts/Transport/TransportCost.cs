using Spirits;

namespace Transport
{

    [System.Serializable]
    public class TransportCost
    {
        // Cost
        public float distance {  get; private set; }

        public int resourcesToTransport { get; private set; }
        public float transportCost { get; private set; }
        public float transportCostBonus { get; private set; }

        public int neededWindSpirit { get; private set; }

        public TransportCost(float dist, float costBonus)
        {
            distance = dist;
            resourcesToTransport = 0;
            neededWindSpirit = 0;
            transportCostBonus = costBonus;

            UpdateCost();
        }

        public TransportCost(float cost, int spirit)
        {
            distance = 0f;
            resourcesToTransport = 0;
            transportCost = cost;
            transportCostBonus = 0f;
            neededWindSpirit = spirit;
        }

        private void UpdateCost()
        {
            transportCost = (resourcesToTransport * distance) - transportCostBonus;
            neededWindSpirit = GetWindSpiritUsed(transportCost);
        }

        public void AddResource(int quantity)
        {
            resourcesToTransport += quantity;
            UpdateCost();
        }

        public static int GetWindSpiritUsed(float cost)
        {
            int capacity = SpiritManager.transportCapacity;

            return (int) cost / capacity + ((cost % capacity <= 0 || cost % capacity == capacity) ? 0 : 1);
        }
    }
}