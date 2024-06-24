using UnityEngine;

namespace Transport
{

    public class TransportManager : MonoBehaviour
    {
        [SerializeField] private TransportLogger _logger;

        void Start()
        {
            _logger.logDictionary.dictionary.Clear();
        }
    }
}