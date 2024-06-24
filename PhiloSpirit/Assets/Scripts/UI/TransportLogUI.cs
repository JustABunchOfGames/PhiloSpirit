using Terrain;
using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TransportLogUI : MonoBehaviour
    {
        [SerializeField] private TransportScriptable _scriptable;

        [SerializeField] private Text _transportCoord;

        private Tile _tile;
        private TransportWay _way;
        private int _index;

        public void Init(Vector3 coord, Tile tile, TransportWay way, int index)
        {
            _transportCoord.text = coord.x +  "/" + coord.y;

            _tile = tile;
            _way = way;
            _index = index;
        }

        public void StartShowScreen()
        {
            _scriptable.StartShowScreen(_tile, _way, _index);
        }
    }
}