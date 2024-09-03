using Terrain;
using UnityEngine;

namespace UI
{
    public class TutorialUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TileManager _manager;

        private void Start()
        {
            BlockSelection(true);
        }

        public void BlockSelection(bool block)
        {
            _manager.CanSelect(!block);
        }
    }
}