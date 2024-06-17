using Resources;
using UnityEngine;

namespace Transport
{
    [System.Serializable]
    public class TransportLog
    {
        public Vector3 startTileCoord { get; private set; }
        public Vector3 endTileCoord { get; private set; }

        public Inventory transportedResources {  get; private set; }
        public int transportCost { get; private set; }

        public TransportLog(Vector3 startTileCoord, Vector3 endTileCoord, int cost) 
        { 
            transportedResources = new Inventory();

            this.startTileCoord = startTileCoord;
            this.endTileCoord = endTileCoord;

            transportCost = cost;
        }

        public void UpdateLog(Inventory transportedResources, int cost)
        {
            this.transportedResources.Copy(transportedResources);

            transportCost = cost;
        }
    }
}