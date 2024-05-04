using Core;
using Resources;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class Task
    {
        public string name;

        public string description;

        public Inventory cost;
        
        [SerializeReference, SubclassPicker] public Reward reward;
    }
}