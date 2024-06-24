using Spirits;
using System.Collections.Generic;
using Terrain;
using UnityEngine;

namespace Transport
{
    [System.Serializable]
    public class TransportLogLists
    {
        public Tile tile { get; private set; }

        public List<TransportLog> transportTo { get; private set; }
        public List<TransportLog> transportFrom { get; private set; }

        public int windSpiritUsed { get; private set; }

        public int totalCost { get; private set; }
        public int possibleCost { get; private set; }

        public TransportLogLists(Tile tile)
        {
            this.tile = tile;

            transportTo = new List<TransportLog>();
            transportFrom = new List<TransportLog>();

            windSpiritUsed = 0;
            totalCost = 0;
            possibleCost = 0;
        }

        public void AddTransportLog(TransportLog log, int windSpiritUsed)
        {
            if (log.startTile == tile)
                transportTo.Add(log);
            if (log.endTile == tile)
                transportFrom.Add(log);

            this.windSpiritUsed += windSpiritUsed;
            totalCost += log.transportCost;
            possibleCost += windSpiritUsed * SpiritManager.transportCapacity;
        }
    }
}