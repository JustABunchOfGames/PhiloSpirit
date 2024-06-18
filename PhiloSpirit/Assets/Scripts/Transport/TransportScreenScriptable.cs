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
        public TransportScreenConfirmEvent screenConfirmEvent = new TransportScreenConfirmEvent();

        // Log
        private TransportLogLists _log;

        // Cost
        private float _distance;

        private int _resourcesToTransport;
        private int _transportCost;
        private int _transportCostBonus;

        private int _neededWindSpirit;

        public void InitScreen(Tile startTile, Tile endTile, TransportLogLists log)
        {
            this.startTile = startTile;
            this.endTile = endTile;
            _log = log;

            _distance = Vector3.Distance(startTile.transform.position, endTile.transform.position);
            _resourcesToTransport = 0;
            _neededWindSpirit = 0;
            _transportCostBonus = _log.possibleCost - _log.totalCost; // Cost overpaid by other transport
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
            int capacity = SpiritManager.transportCapacity;

            _transportCost = (int)(_resourcesToTransport * _distance) - _transportCostBonus;
            _neededWindSpirit = _transportCost / capacity +
                ((_transportCost % capacity <= 0 || _transportCost % capacity == capacity) ? 0 : 1);
        }

        public void ConfirmTransport()
        {
            for (int i=0; i<_neededWindSpirit; i++)
            {
                if (!SpiritManager.CanUseSpirit(SpiritType.Wind, 1))
                    SpiritManager.AddSpirit(SpiritType.Wind);
                
                SpiritManager.UsePirit(SpiritType.Wind, 1);
            }

            startTile.inventory.Copy(tileInventory);

            foreach(Resource res in transportInventory.resources)
            {
                endTile.inventory.Add(res);
            }

            // Log & without applying the "bonus" previewed
            _log.AddTransportLog(new TransportLog(startTile.transform.position, endTile.transform.position, _transportCost + _transportCostBonus), _neededWindSpirit);

            screenConfirmEvent.Invoke();
        }

        public class TransportScreenStartedEvent : UnityEvent { }

        public class TransportScreenUpdateEvent : UnityEvent<ResourceType, bool> { }

        public class TransportScreenConfirmEvent : UnityEvent { }
    }
}