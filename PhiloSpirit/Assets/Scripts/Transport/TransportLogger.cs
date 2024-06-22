using UnityEngine;

namespace Transport
{

    [CreateAssetMenu(fileName = "Logger", menuName = "Transport/Logger")]
    public class TransportLogger : ScriptableObject
    {
        public TransportLogDictionary logDictionary = new TransportLogDictionary();
    }
}