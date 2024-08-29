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

        public override string GetDescription(string name)
        {
            return "Reveal the map for " + _nbTileRevealed + " tile of range";
        }
    }
}