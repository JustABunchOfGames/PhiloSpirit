using Terrain;
using UnityEngine;

namespace Building
{
    public class BuildingTileIndicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _okColor;
        [SerializeField] private Color _koColor;

        private TileType _requiredType;

        public void Init(TileType type)
        {
            _requiredType = type;
        }

        public bool Check()
        {
            // Get Tile under the indicator
            Ray ray = new Ray(transform.position, transform.forward * -1);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit && hit.transform.tag == "Terrain")
            {
                if (hit.collider.GetComponent<Tile>().tileType == _requiredType)
                    return ChangeColor(true);

                return ChangeColor(false);
            }
            else
            {
                return ChangeColor(false);
            }
        }

        private bool ChangeColor(bool isOk)
        {
            _renderer.color = isOk ? _okColor : _koColor;
            return isOk;
        }
    }
}