using Resources;
using Terrain;
using UnityEngine;

namespace Transport
{
    [System.Serializable]
    public class TransportLog
    {
        public Tile startTile { get; private set; }
        public Tile endTile { get; private set; }

        public Inventory transportedResources {  get; private set; }
        public int transportCost { get; private set; }

        public TransportLog(Tile startTile, Tile endTile, Inventory inventory, int cost) 
        { 
            transportedResources = new Inventory();

            this.startTile = startTile;
            this.endTile = endTile;

            UpdateLog(inventory, cost);
        }

        public void UpdateLog(Inventory transportedResources, int cost)
        {
            this.transportedResources.Copy(transportedResources);

            transportCost = cost;
        }
    }
}