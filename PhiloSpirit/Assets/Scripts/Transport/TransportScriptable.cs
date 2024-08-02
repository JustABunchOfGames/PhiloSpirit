using Resources;
using Spirits;
using System.Collections.Generic;
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
        private TransportLogLists _logLists;
        private TransportLog _log;

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
            if (state == TransportState.Show || state == TransportState.Modify)
            {
                screenConfirmEvent.Invoke();
            }
        }

        private void StartCreationScreen()
        {
            // Creation of logs for this tile + saving it locally
            Tile tile = IsTransportingTo(_way) ? startTile : endTile;
            _logger.logDictionary.Add(tile);
            _logLists = _logger.logDictionary.GetLogs(tile);

            // Cost initialization
            cost = new TransportCost(Vector3.Distance(startTile.transform.position, endTile.transform.position), _logLists.possibleCost - _logLists.totalCost);

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
            _logLists = _logger.logDictionary.dictionary[tile];
            if (IsTransportingTo(_way))
                _log = _logLists.transportTo[index];
            else
                _log = _logLists.transportFrom[index];

            // Setting Tiles
            startTile = _log.startTile;
            endTile = _log.endTile;

            // Setting the cost for UI
            cost = new TransportCost(_log.transportCost, SpiritManager.transportCapacity);

            // Getting Log Inventory
            tileInventory.Copy(startTile.inventory);
            transportInventory.Copy(_log.transportedResources);

            screenStartEvent.Invoke();
        }

        public void StartModifyScreen()
        {
            state = TransportState.Modify;

            // Repopulate list with only refundable resources
            transportInventory = new Inventory();

            foreach (Resource res in _log.transportedResources.resources)
            {
                int quantity = endTile.inventory.GetQuantity(res.type);
                if (0 < quantity) {

                    if (quantity < res.quantity)
                        transportInventory.Add(new Resource(res.type, quantity));
                    else
                        transportInventory.Add(new Resource(res.type, res.quantity));
                }
            }

            cost = new TransportCost(Vector3.Distance(startTile.transform.position, endTile.transform.position), 0f);

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

        public void CreateTransport()
        {
            if (cost.resourcesToTransport == 0)
                return;

            for (int i=0; i < cost.neededWindSpirit; i++)
            {
                if (!SpiritManager.CanUseSpirit(SpiritType.Wind, 1))
                    SpiritManager.AddSpirit(SpiritType.Wind);

                SpiritManager.UseSpirit(SpiritType.Wind, 1);
            }

            startTile.inventory.Copy(tileInventory);

            foreach(Resource res in transportInventory.resources)
            {
                endTile.inventory.Add(res);
            }

            // Log & without applying the "bonus" previewed
            _logLists.AddTransportLog(new TransportLog(startTile, endTile, transportInventory,
                cost.transportCost + cost.transportCostBonus));

            screenConfirmEvent.Invoke();
        }

        public void ModifyTransport()
        {
            // Update Inventories
            startTile.inventory.Copy(tileInventory);

            List<ResourceType> types = new List<ResourceType>(); // List of resource types already updated

            foreach (Resource res in transportInventory.resources)
            {
                endTile.inventory.Add(new Resource(res.type, res.quantity - _log.transportedResources.GetQuantity(res.type)));
                types.Add(res.type);
            }
            foreach (Resource res in _log.transportedResources.resources)
            {
                if (!types.Contains(res.type))
                {
                    endTile.inventory.Add(new Resource(res.type, transportInventory.GetQuantity(res.type) - res.quantity));
                }
            }

            // Save old wind spirit cost
            int windSpiritModif = _logLists.windSpiritUsed;

            // Update Logs
            _log.UpdateLog(transportInventory, _log.transportCost + cost.transportCost);
            if (_log.transportCost <= 0)
                _logLists.RemoveTransportLog(_log);

            _logLists.UpdateCost(cost.transportCost);

            // Update usage of Wind Spirit
            int windSpirit = _logLists.windSpiritUsed - windSpiritModif;
            if (windSpiritModif <= 0)
                SpiritManager.UseSpirit(SpiritType.Wind, windSpiritModif);
            else
            {
                while(windSpiritModif > 0)
                {
                    SpiritManager.AddSpirit(SpiritType.Wind);
                    SpiritManager.UseSpirit(SpiritType.Wind, 1);
                    windSpiritModif--; ;
                }
            }

            // Close screen
            screenConfirmEvent.Invoke();
        }

        public void DeleteTransport()
        {
            if (!IsDeletePossible())
                return;

            state = TransportState.Delete;

            foreach(Resource res in transportInventory.resources)
            {
                startTile.inventory.Add(res);
                endTile.inventory.Remove(res);
            }

            int oldWindSpiritCost = _logLists.windSpiritUsed;

            _logLists.RemoveTransportLog(_log);

            SpiritManager.UseSpirit(SpiritType.Wind, _logLists.windSpiritUsed - oldWindSpiritCost);

            screenConfirmEvent.Invoke();
        }

        public bool IsDeletePossible()
        {
            foreach(Resource res in transportInventory.resources)
            {
                if (!endTile.inventory.HasEnough(res))
                    return false;
            }

            return true;
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