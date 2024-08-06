using Core;
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
        [SerializeField] private string _buildingTag;

        private TileType _requiredType;

        public void Init(TileType type)
        {
            _requiredType = type;
        }

        public bool Check()
        {
            // Get Tile under the indicator
            GameObject terrain = null;
            bool _isOk = true;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward * -1);
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == Tags.terrainTag)
                    terrain = hit.transform.gameObject;

                if (hit.transform.tag == Tags.fogOfWarTag)
                    _isOk = false;

                if (hit.transform.tag == Tags.buildingTag)
                    _isOk = false;
            }

            if (_isOk && terrain != null)
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