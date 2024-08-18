using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{
    [CreateAssetMenu(fileName = "Scriptable", menuName = "Task/Scriptable")]
    public class TaskScriptable : ScriptableObject
    {
        #region - TaskState -
        [SerializeField] private List<TaskStateIndicator> _stateIndicatorList;

        private Dictionary<TaskState, TaskStateIndicator> _stateIndicatorDictionary;

        public void Init()
        {
            _stateIndicatorDictionary = new Dictionary<TaskState, TaskStateIndicator>();

            foreach (TaskStateIndicator indicator in _stateIndicatorList)
            {
                _stateIndicatorDictionary.Add(indicator.state, indicator);
            }
        }

        public TaskStateIndicator GetStateIndicator(TaskState state)
        {
            return _stateIndicatorDictionary[state];
        }

        [Serializable]
        public struct TaskStateIndicator
        {
            public TaskState state;
            public GameObject indicator;
            public Vector3 position;
        }

        #endregion

        #region - Events -
        private Task _currentTask;


        #region - Show -
        public ShowTaskEvent showEvent = new ShowTaskEvent();

        public void ShowTask(Task task)
        {
            _currentTask = task;
            showEvent.Invoke(task);
        }

        public class ShowTaskEvent : UnityEvent<Task> { }
        #endregion

        #region - Accept -
        public AcceptTaskEvent acceptEvent = new AcceptTaskEvent();
        
        public void AcceptTask()
        {
            _currentTask.state =
                _currentTask.state == TaskState.Available ? TaskState.Accepted : TaskState.Available ;

            acceptEvent.Invoke(_currentTask);
        }

        public class AcceptTaskEvent : UnityEvent<Task> { }
        #endregion

        #region - Complete -
        public CompleteTasksEvent completeEvent = new CompleteTasksEvent();

        public void CompleteTasks()
        {
            completeEvent.Invoke();
        }

        public class CompleteTasksEvent : UnityEvent { }
        #endregion

        #endregion
    }
}