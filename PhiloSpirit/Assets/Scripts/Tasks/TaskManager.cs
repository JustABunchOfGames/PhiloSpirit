using Resources;
using Spirits;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{

    public class TaskManager : MonoBehaviour
    {
        // List of accepted Tasks
        [SerializeField] private List<Task> _currentTasks = new List<Task>();

         // Inventory for the cost of accepted tasks
        [SerializeField] private Inventory _totalCost;
        public Inventory totalCost { get { return _totalCost; } private set { } } 

        // Event to notify a change in cost for UI
        public TaskCostChangeEvent costChanged = new TaskCostChangeEvent();

        // Stock completed tasks for saving purposes
        [SerializeField] private List<Task> _completedTasks = new List<Task>();

        private void Awake()
        {
            SpiritManager.updateSpiritEvent.AddListener(UpdateCostForSpirit);
        }

        private void Start()
        {
            _totalCost = new Inventory();
        }

        public void AddTask(Task task)
        {
            _currentTasks.Add(task);

            foreach(Resource resource in task.cost.resources)
            {
                _totalCost.Add(new Resource(resource.type, resource.quantity));
            }

            costChanged.Invoke();
        }

        public void RemoveTask(Task task)
        {
            _currentTasks.Remove(task);

            foreach (Resource resource in task.cost.resources)
            {
                _totalCost.Remove(new Resource(resource.type, resource.quantity));
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

            _totalCost.Add(new Resource(ResourceType.Food, SpiritManager.RecalculateCost()));

            costChanged.Invoke();
        }

        private void UpdateCostForSpirit(SpiritData spiritData, int quantity)
        {
            // If quantity = 1, Added a spirit
            // Add the "removeCost" because it's the cost of the last added spirit

            // If quantity = -1, Removed a spirit
            // Substract the "addCost" because it's the refunded cost of the last removed spirit

            // If quantity = 0, no cost update involved
            if (quantity == 0)
                return;

            for (int i = 0; i < MathF.Abs(quantity); i++)
            {
                _totalCost.Add(new Resource(ResourceType.Food,
                quantity > 0 ? spiritData.removeCost : -spiritData.addCost));
            }

            costChanged.Invoke();
        }
    }

    public class TaskCostChangeEvent : UnityEvent { }
}