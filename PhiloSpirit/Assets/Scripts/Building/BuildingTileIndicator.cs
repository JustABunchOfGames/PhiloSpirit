using Terrain;
using UnityEngine;

namespace Building
{
    public class BuildingTileIndicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _okColor;
        [SerializeField] private Color _koColor;

        [SerializeField] private string _terrainTag;
        [SerializeField] private string _fowTag;

        private TileType _requiredType;

        public void Init(TileType type)
        {
            _requiredType = type;
        }

        public bool Check()
        {
            // Get Tile under the indicator
            GameObject terrain = null;
            bool _inFogOfWar = false;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward * -1);
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == _terrainTag)
                    terrain = hit.transform.gameObject;

                if (hit.transform.tag == _fowTag)
                    _inFogOfWar = true;
            }

            if (!_inFogOfWar && terrain != null)
            {
                if (terrain.GetComponent<Tile>().tileType == _requiredType)
                    return ChangeColor(true);
            }

            return ChangeColor(false);
        }

        private bool ChangeColor(bool isOk)
        {
            _renderer.color = isOk ? _okColor : _koColor;
            return isOk;
        }
    }
}