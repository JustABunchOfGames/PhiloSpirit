namespace Spirits
{

    public enum SpiritType
    {
        Earth,
        Wind,
        Water,
        Fire
    }

    [System.Serializable]
    public class SpiritData
    {
        public int maxSpirit { get; private set; }
        public int usableSpirit { get; private set; }
        public int addCost { get; private set; }
        public int removeCost { get; private set; }

        public SpiritData()
        {
            maxSpirit = 0;
            usableSpirit = 0;

            addCost = 1;
            removeCost = 1;

        }

        public void AddSpirit()
        {
            maxSpirit++;
            usableSpirit++;

            removeCost = addCost;
            addCost = 1 + maxSpirit / 5;
        }

        public void RemoveSpirit()
        {
            if (usableSpirit > 0)
            {
                maxSpirit--;
                usableSpirit--;


                addCost = 1 + maxSpirit / 5;

                if (maxSpirit % 5 == 0)
                    removeCost = addCost - 1;
                else
                    removeCost = addCost;
            }
        }

        public void UseSpirit(int quantity)
        {
            if (usableSpirit - quantity > 0)
            {
                usableSpirit -= quantity;
            }
        }

        public int GetTotalCost()
        {
            int cost = 0;
            int i = 1 + maxSpirit / 5;

            cost += (maxSpirit % 5) * i--;

            for (; i > 0; i--)
            {
                cost += 5 * i;
            }

            return cost;
        }
    }
}