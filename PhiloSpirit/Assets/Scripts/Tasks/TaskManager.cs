using Resources;
using Spirits;
using System;
using System.Collections.Generic;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private TaskScriptable _scriptable;

        [SerializeField] private Tile _portalTile;

        // List of accepted Tasks
        [SerializeField] private List<Task> _currentTasks = new List<Task>();

         // Inventory for the cost of accepted tasks
        [SerializeField] private Inventory _totalCost;
        public Inventory totalCost { get { return _totalCost; } private set { } } 

        // Stock completed tasks for saving purposes
        [SerializeField] private List<Task> _completedTasks = new List<Task>();

        // Event to notify a change in cost for UI
        public TaskCostChangeEvent costChanged = new TaskCostChangeEvent();

        // Event to start CompleteTaskUI
        public TaskCompleteEvent completeEvent = new TaskCompleteEvent();


        private void Awake()
        {
            SpiritManager.updateSpiritEvent.AddListener(UpdateCostForSpirit);

            _scriptable.Init();
        }

        private void Start()
        {
            _totalCost = new Inventory();

            _scriptable.acceptEvent.AddListener(AcceptTask);
            _scriptable.completeEvent.AddListener(CompleteTaskEvent);

            _portalTile.inventoryChangedEvent.AddListener(ChangeEvent);
        }

        private void ChangeEvent()
        {
            costChanged.Invoke(_portalTile.inventory, _totalCost);
        }

        private void AcceptTask(Task task)
        {
            // Changed from available to accepted
            if (task.state == TaskState.Accepted)
                AddTask(task);
            // Changed from accepted to available
            else if (task.state == TaskState.Available)
                RemoveTask(task);
        }

        private void AddTask(Task task)
        {
            _currentTasks.Add(task);

            foreach(Resource resource in task.cost.resources)
            {
                _totalCost.Add(new Resource(resource.type, resource.quantity));
            }

            ChangeEvent();
        }

        private void RemoveTask(Task task)
        {
            _currentTasks.Remove(task);

            foreach (Resource resource in task.cost.resources)
            {
                _totalCost.Remove(new Resource(resource.type, resource.quantity));
            }

            ChangeEvent();
        }

        private void CompleteTaskEvent()
        {
            completeEvent.Invoke(_portalTile.inventory, _currentTasks);
        }

        public void CompleteTasks()
        {
            foreach (Task task in _currentTasks)
            {
                task.reward.Apply();
                task.state = TaskState.Completed;
                _completedTasks.Add(task);
            }

            _currentTasks.Clear();
            _totalCost.Clear();

            int spiritCost = SpiritManager.RecalculateCost();

            if (spiritCost > 0)
                _totalCost.Add(new Resource(ResourceType.Food, spiritCost));

            ChangeEvent();
        }

        private void UpdateCostForSpirit(SpiritData spiritData, int quantity)
        {
            // If quantity > 0, Added a spirit
            // Add the "removeCost" because it's the cost of the last added spirit

            // If quantity < 0, Removed a spirit
            // Substract the "addCost" because it's the refunded cost of the last removed spirit

            // If quantity = 0, no cost update involved
            if (quantity == 0)
                return;

            for (int i = 0; i < MathF.Abs(quantity); i++)
            {
                _totalCost.Add(new Resource(ResourceType.Food,
                quantity > 0 ? spiritData.removeCost : -spiritData.addCost));
            }

            ChangeEvent();
        }
    }

    public class TaskCostChangeEvent : UnityEvent<Inventory, Inventory> { }

    public class TaskCompleteEvent : UnityEvent<Inventory, List<Task>> { }
}