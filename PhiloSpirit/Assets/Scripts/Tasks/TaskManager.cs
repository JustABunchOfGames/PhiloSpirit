using Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<Task> _currentTasks = new List<Task>();

        [SerializeField] private Inventory _totalCost = new Inventory();

        [SerializeField] private List<Task> _completedTasks = new List<Task>();

        public void AddTask(Task task)
        {
            _currentTasks.Add(task);

            foreach(Resource resource in task.cost.resources)
            {
                _totalCost.Add(resource);
            }
        }

        public void RemoveTask(Task task)
        {
            _currentTasks.Remove(task);

            foreach (Resource resource in task.cost.resources)
            {
                _totalCost.Remove(resource);
            }
        }

        public void ApplyAll()
        {
            foreach (Task task in _currentTasks)
            {
                task.reward.Apply();
                _completedTasks.Add(task);
            }

            _currentTasks.Clear();
        }
    }
}