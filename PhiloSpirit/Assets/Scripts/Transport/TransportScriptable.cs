using Resources;
using Spirits;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Transport
{
    [CreateAssetMenu(fileName ="Scriptable", menuName ="Transport/Scriptable")]
    public class TransportScriptable : ScriptableObject
    {
        // Tile for transport
        public Tile startTile { get; private set; }
        public Tile endTile { get; private set; }

        // Enums
        private TransportWay _way;
        public TransportState state { get; private set; }

        // Temporary inventories
        public Inventory tileInventory {  get; private set; }

        public Inventory transportInventory { get; private set; }

        // Events
        public TransportSelectionStartEvent selectionStartEvent = new TransportSelectionStartEvent();
        public TransportScreenStartedEvent screenStartEvent = new TransportScreenStartedEvent();
        public TransportScreenUpdateEvent screenUpdateEvent = new TransportScreenUpdateEvent();
        public TransportScreenConfirmEvent screenConfirmEvent = new TransportScreenConfirmEvent();

        // Log
        [SerializeField] private TransportLogger _logger;
        private TransportLogLists _log;

        // Cost
        public TransportCost cost;

        private bool IsTransportingTo(TransportWay way)
        {
            return way == TransportWay.TransportTo;
        }

        public void StartTransportCreation(Tile tile, TransportWay way)
        {
            _way = way;
            state = TransportState.Create;
            
            if (IsTransportingTo(_way))
                startTile = tile;
            else
                endTile = tile;

            selectionStartEvent.Invoke(tile, _way);
        }

        public void ConfirmSelection(Tile tile)
        {
            if (IsTransportingTo(_way))
                endTile = tile;
            else
                startTile = tile;

            StartCreationScreen();
        }

        public void CancelTransport()
        {
            if (state == TransportState.Create)
            {
                selectionStartEvent.Invoke(IsTransportingTo(_way) ? startTile : endTile , _way);
            }
            if (state == TransportState.Show)
            {
                screenConfirmEvent.Invoke();
            }
        }

        private void StartCreationScreen()
        {
            // Creation of logs for this tile + saving it locally
            Tile tile = IsTransportingTo(_way) ? startTile : endTile;
            _logger.logDictionary.Add(tile);
            _log = _logger.logDictionary.GetLogs(tile);

            // Cost initialization
            cost = new TransportCost(Vector3.Distance(startTile.transform.position, endTile.transform.position), _log.possibleCost - _log.totalCost);

            // Inventory initialization by copy
            tileInventory = new Inventory();
            tileInventory.Copy(startTile.inventory);
            transportInventory = new Inventory();

            screenStartEvent.Invoke();
        }

        public void StartShowScreen(Tile tile, TransportWay way, int index)
        {
            state = TransportState.Show;
            _way = way;

            // Getting the logs
            _log = _logger.logDictionary.dictionary[tile];
            TransportLog log;
            if (IsTransportingTo(_way))
                log = _log.transportTo[index];
            else
                log = _log.transportFrom[index];

            // Setting Tiles
            startTile = log.startTile;
            endTile = log.endTile;

            // Setting the cost for UI
            cost = new TransportCost(log.transportCost, SpiritManager.transportCapacity);

            // Getting Log Inventory
            tileInventory.Copy(startTile.inventory);
            transportInventory.Copy(log.transportedResources);

            screenStartEvent.Invoke();
        }

        public void Transport(ResourceType resourceType, bool transport)
        {
            // Update temporary lists
            tileInventory.Add(new Resource(resourceType, transport ? -1 : 1));
            transportInventory.Add(new Resource(resourceType, transport ? 1 : -1));

            // Cost Update
            cost.AddResource(transport ? 1 : -1);

            // Update UI
            screenUpdateEvent.Invoke(resourceType, transport);
        }

        public void ConfirmTransport()
        {
            for (int i=0; i < cost.neededWindSpirit; i++)
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
            _log.AddTransportLog(new TransportLog(startTile, endTile, transportInventory,
                cost.transportCost + cost.transportCostBonus), cost.neededWindSpirit);

            screenConfirmEvent.Invoke();
        }

        public class TransportSelectionStartEvent : UnityEvent<Tile, TransportWay> { }

        public class TransportScreenStartedEvent : UnityEvent { }

        public class TransportScreenUpdateEvent : UnityEvent<ResourceType, bool> { }

        public class TransportScreenConfirmEvent : UnityEvent { }
    }

    [System.Serializable]
    public enum TransportWay
    {
        TransportTo,
        TransportFrom
    }

    public enum TransportState
    {
        Create,
        Show,
        Modify,
        Delete
    }
}