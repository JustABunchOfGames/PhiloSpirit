using System.Collections.Generic;
using UnityEngine;

namespace Transport
{
    public class TransportLogDictionary
    {
        public Dictionary<Vector3, TransportLogLists> dictionary { get; private set; }

        public TransportLogDictionary()
        {
            dictionary = new Dictionary<Vector3, TransportLogLists>();
        }

        public void Add(Vector3 coord, TransportLog log, int windSpiritUsed)
        {
            if (dictionary.ContainsKey(coord))
                dictionary[coord].AddTransportLog(log, windSpiritUsed);
            else
            {
                dictionary.Add(coord, new TransportLogLists(coord));
                dictionary[coord].AddTransportLog(log, windSpiritUsed);
            }
        }

        public TransportLogLists GetLogs(Vector3 coord)
        {
            if (dictionary.ContainsKey(coord))
                return dictionary[coord];
            else
                return null;
        }
    }
}
