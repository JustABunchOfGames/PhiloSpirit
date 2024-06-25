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

        public TransportCost(int cost, int spirit)
        {
            distance = 0;
            resourcesToTransport = 0;
            transportCost = cost;
            transportCostBonus = 0;
            neededWindSpirit = spirit;
        }

        private void UpdateCost()
        {
            transportCost = (int)(resourcesToTransport * distance) - transportCostBonus;
            neededWindSpirit = GetWindSpiritUsed(transportCost);
        }

        public void AddResource(int quantity)
        {
            resourcesToTransport += quantity;
            UpdateCost();
        }

        public static int GetWindSpiritUsed(int cost)
        {
            int capacity = SpiritManager.transportCapacity;

            return cost / capacity + ((cost % capacity <= 0 || cost % capacity == capacity) ? 0 : 1);
        }
    }
}