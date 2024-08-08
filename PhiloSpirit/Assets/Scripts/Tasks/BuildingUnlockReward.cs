using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class BuildingUnlockReward : Reward
    {
        [SerializeField] private Button _buildingButton;

        public override void Apply()
        {
            _buildingButton.interactable = true;
        }
    }
}