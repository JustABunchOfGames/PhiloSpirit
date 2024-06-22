using Transport;
using UnityEngine;

namespace UI
{

    public class TransportCreationButton : MonoBehaviour
    {
        [SerializeField] private TileTransportUI _tileTransportUI;
        [SerializeField] private TransportWay _way;

        public void StartTransportCreation()
        {
            _tileTransportUI.StartTransportCreation(_way);
        }
    }
}