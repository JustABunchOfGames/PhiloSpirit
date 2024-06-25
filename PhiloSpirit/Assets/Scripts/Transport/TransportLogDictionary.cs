using System.Collections.Generic;
using Terrain;

namespace Transport
{
    public class TransportLogDictionary
    {
        public Dictionary<Tile, TransportLogLists> dictionary { get; private set; }

        public TransportLogDictionary()
        {
            dictionary = new Dictionary<Tile, TransportLogLists>();
        }

        public void Add(Tile tile, TransportLog log)
        {
            if (dictionary.ContainsKey(tile))
                dictionary[tile].AddTransportLog(log);
            else
            {
                dictionary.Add(tile, new TransportLogLists(tile));
                dictionary[tile].AddTransportLog(log);
            }
        }

        public void Add(Tile tile)
        {
            if (!dictionary.ContainsKey(tile))
                dictionary.Add(tile, new TransportLogLists(tile));
        }

        public TransportLogLists GetLogs(Tile tile)
        {
            if (dictionary.ContainsKey(tile))
                return dictionary[tile];
            else
                return null;
        }
    }
}
