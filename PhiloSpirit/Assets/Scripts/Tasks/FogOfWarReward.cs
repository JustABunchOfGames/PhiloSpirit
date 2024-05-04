using FogOfWar;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class FogOfWarReward : Reward
    {
        [SerializeField] private FoWManager _fowManager;

        [SerializeField] private int _nbTileRevealed;

        public override void Apply()
        {
            _fowManager.ReduceFoW(_nbTileRevealed);
        }
    }
}