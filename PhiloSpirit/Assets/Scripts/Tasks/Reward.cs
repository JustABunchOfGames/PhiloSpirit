using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public abstract class Reward
    {
        public abstract void Apply();

        public abstract string GetDescription(string name);
    }
}