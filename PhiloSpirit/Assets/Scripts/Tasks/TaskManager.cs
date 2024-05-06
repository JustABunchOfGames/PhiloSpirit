using Resources;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<Task> _currentTasks = new List<Task>();

         // Inventory for the cost of accepted tasks
        [SerializeField] private Inventory _totalCost = new Inventory();
        public Inventory totalCost { get { return _totalCost; } private set { } } 

        // Event to notify a change in cost for UI
        public TaskCostChangeEvent costChanged = new TaskCostChangeEvent();

        [SerializeField] private List<Task> _completedTasks = new List<Task>();

        public void AddTask(Task task)
        {
            _currentTasks.Add(task);

            foreach(Resource resource in task.cost.resources)
            {
                _totalCost.Add(resource);
            }

            costChanged.Invoke();
        }

        public void RemoveTask(Task task)
        {
            _currentTasks.Remove(task);

            foreach (Resource resource in task.cost.resources)
            {
                _totalCost.Remove(resource);
            }

            costChanged.Invoke();
        }

        public void CompleteTasks()
        {
            foreach (Task task in _currentTasks)
            {
                task.reward.Apply();
                _completedTasks.Add(task);
            }

            _currentTasks.Clear();
            _totalCost.resources.Clear();

            costChanged.Invoke();
        }
    }

    public class TaskCostChangeEvent : UnityEvent { }
}