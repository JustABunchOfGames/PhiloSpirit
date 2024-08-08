using Core;
using Resources;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class Task
    {
        [TextArea] public string name;

        [TextArea]  public string description;

        public Inventory cost;
        
        [SerializeReference, SubclassPicker] public Reward reward;
    }
}