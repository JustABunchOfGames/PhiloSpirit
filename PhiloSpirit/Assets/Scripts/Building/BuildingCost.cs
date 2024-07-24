using Resources;
using Spirits;
using System.Collections.Generic;

namespace Building
{
    [System.Serializable]
    public class BuildingCost
    {
        public Inventory resourceCost;
        public List<SpiritCost> spiritCost;
    }

    [System.Serializable]
    public struct SpiritCost
    {
        public SpiritType type;
        public uint quantity;
    }
}