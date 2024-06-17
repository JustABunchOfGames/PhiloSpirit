using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TransportLogUI : MonoBehaviour
    {
        [SerializeField] private Text _transportCoord;

        public void Init(Vector3 coord)
        {
            _transportCoord.text = coord.x +  "/" + coord.y;
        }
    }
}