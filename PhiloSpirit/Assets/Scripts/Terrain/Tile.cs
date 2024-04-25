using UnityEngine;
using Resources;

namespace Terrain
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Inventory _inventory;

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