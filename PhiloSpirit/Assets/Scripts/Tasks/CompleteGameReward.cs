using UnityEngine;

namespace Tasks
{

    public class CompleteGameReward : Reward
    {
        [SerializeField] private GameObject _completeScreen;

        public override void Apply()
        {
            _completeScreen.SetActive(true);
        }

        public override string GetDescription(string name)
        {
            return "Be the next Spirit King";
        }
    }
}