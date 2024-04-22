using UnityEngine;

namespace Terrain
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void GetSelected()
        {
            _spriteRenderer.color = Color.grey;
        }

        public void Unselect()
        {
            _spriteRenderer.color = Color.white;
        }
    }
}