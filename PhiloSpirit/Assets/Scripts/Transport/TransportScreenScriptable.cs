using Resources;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Transport
{
    [CreateAssetMenu(fileName ="Screen/ScreenScriptable")]
    public class TransportScreenScriptable : ScriptableObject
    {
        public Tile startTile { get; private set; }
        public Tile endTile { get; private set; }

        private Inventory _tileInventory;
        private Inventory _transportInventory;

        public TransportScreenStartedEvent screenStartEvent = new TransportScreenStartedEvent();
        public TransportScreenUpdateEvent screenUpdateEvent = new TransportScreenUpdateEvent();

        public void InitScreen(Tile startTile, Tile endTile)
        {
            this.startTile = startTile;
            this.endTile = endTile;

            // Make a copy of inventory to have a temporary one
            _tileInventory = new Inventory();
            _tileInventory.Copy(this.startTile.inventory);
            _transportInventory = new Inventory();

            screenStartEvent.Invoke();
        }

        public Inventory GetInventory(bool transport)
        {
            return transport ? _transportInventory : _tileInventory;
        }

        public void Transport(ResourceType resourceType, bool transport)
        {
            // Update temporary lists
            _tileInventory.Add(new Resource(resourceType, transport ? -1 : 1));
            _transportInventory.Add(new Resource(resourceType, transport ? 1 : -1));

            // Update UI
            screenUpdateEvent.Invoke(resourceType, transport);
        }

        public class TransportScreenStartedEvent : UnityEvent { }

        public class TransportScreenUpdateEvent : UnityEvent<ResourceType, bool> { }
    }
}