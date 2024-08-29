using Core;
using Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{
    [System.Serializable]
    public class Task
    {
        [TextArea] public string name;

        public string description { get { return reward.GetDescription(name); } private set { } }

        public StateChangeEvent stateChangeEvent = new StateChangeEvent();

        [SerializeField] private TaskState _state;
        public TaskState state { 
            get { return _state; } 
            set { _state = value; stateChangeEvent.Invoke(); } }

        public Inventory cost;
        
        [SerializeReference, SubclassPicker] public Reward reward;

        public class StateChangeEvent : UnityEvent { }
    }
}