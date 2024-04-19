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
    }
}