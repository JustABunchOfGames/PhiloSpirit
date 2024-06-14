using log4net.Util;
using Resources;
using Spirits;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Transport
{
    [CreateAssetMenu(fileName ="Screen/ScreenScriptable")]
    public class TransportScreenScriptable : ScriptableObject
    {
        // Tile for transport
        public Tile startTile { get; private set; }
        public Tile endTile { get; private set; }

        // Temporary inventories
        public Inventory tileInventory {  get; private set; }

        public Inventory transportInventory { get; private set; }

        // Events
        public TransportScreenStartedEvent screenStartEvent = new TransportScreenStartedEvent();
        public TransportScreenUpdateEvent screenUpdateEvent = new TransportScreenUpdateEvent();

        // Cost
        private float _distance;

        private int _resourcesToTransport;
        private int _transportCost;

        private int _neededWindSpirit;

        [SerializeField] private int _windSpiritCapacity;

        public void InitScreen(Tile startTile, Tile endTile)
        {
            this.startTile = startTile;
            this.endTile = endTile;

            _distance = Vector3.Distance(startTile.transform.position, endTile.transform.position);
            _resourcesToTransport = 0;
            _neededWindSpirit = 0;
            UpdateCost();

            // Make a copy of inventory to have a temporary one
            tileInventory = new Inventory();
            tileInventory.Copy(this.startTile.inventory);
            transportInventory = new Inventory();

            screenStartEvent.Invoke();
        }

        public void Transport(ResourceType resourceType, bool transport)
        {
            // Update temporary lists
            tileInventory.Add(new Resource(resourceType, transport ? -1 : 1));
            transportInventory.Add(new Resource(resourceType, transport ? 1 : -1));

            // Cost Update
            _resourcesToTransport = _resourcesToTransport + (transport ? 1 : -1);
            UpdateCost();

            // Update UI
            screenUpdateEvent.Invoke(resourceType, transport);
        }

        public int GetCost()
        {
            return _transportCost; 
        }

        public int GetSpiritCost()
        {
            return _neededWindSpirit;
        }

        private void UpdateCost()
        {
            _transportCost = (int)(_resourcesToTransport * _distance);
            _neededWindSpirit = _transportCost / _windSpiritCapacity +
                ((_transportCost % _windSpiritCapacity == 0 || _transportCost % _windSpiritCapacity == _windSpiritCapacity) ? 0 : 1);
        }

        public void ConfirmTransport()
        {
            for (int i=0; i<_neededWindSpirit; i++)
            {
                SpiritManager.AddSpirit(SpiritType.Wind);
            }
            SpiritManager.UsePirit(SpiritType.Wind, _neededWindSpirit);

            startTile.inventory.Copy(tileInventory);

            foreach(Resource res in transportInventory.resources)
            {
                endTile.inventory.Add(res);
            }
        }

        public class TransportScreenStartedEvent : UnityEvent { }

        public class TransportScreenUpdateEvent : UnityEvent<ResourceType, bool> { }
    }
}