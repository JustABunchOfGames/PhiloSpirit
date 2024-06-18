using Spirits;
using System.Collections.Generic;
using UnityEngine;

namespace Transport
{
    [System.Serializable]
    public class TransportLogLists
    {
        public Vector3 tileCoord { get; private set; }

        public List<TransportLog> transportTo { get; private set; }
        public List<TransportLog> transportFrom { get; private set; }

        public int windSpiritUsed { get; private set; }

        public int totalCost { get; private set; }
        public int possibleCost { get; private set; }

        public TransportLogLists(Vector3 tileCoord)
        {
            this.tileCoord = tileCoord;

            transportTo = new List<TransportLog>();
            transportFrom = new List<TransportLog>();

            windSpiritUsed = 0;
            totalCost = 0;
            possibleCost = 0;
        }

        public void AddTransportLog(TransportLog log, int windSpiritUsed)
        {
            if (log.startTileCoord == tileCoord)
                transportTo.Add(log);
            if (log.endTileCoord == tileCoord)
                transportFrom.Add(log);

            this.windSpiritUsed += windSpiritUsed;
            totalCost += log.transportCost;
            possibleCost += windSpiritUsed * SpiritManager.transportCapacity;
        }
    }
}