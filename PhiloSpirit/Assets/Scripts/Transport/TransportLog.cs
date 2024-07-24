using Resources;
using Terrain;

namespace Transport
{
    [System.Serializable]
    public class TransportLog
    {
        public Tile startTile { get; private set; }
        public Tile endTile { get; private set; }

        public Inventory transportedResources {  get; private set; }
        public float transportCost { get; private set; }

        public TransportLog(Tile startTile, Tile endTile, Inventory inventory, float cost) 
        { 
            transportedResources = new Inventory();

            this.startTile = startTile;
            this.endTile = endTile;

            UpdateLog(inventory, cost);
        }

        public void UpdateLog(Inventory transportedResources, float cost)
        {
            this.transportedResources.Copy(transportedResources);

            transportCost = cost;
        }
    }
}