using Resources;
using Spirits;
using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;

namespace UI
{
    public class TaskCompleteUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TaskManager _manager;

        [Header("UI")]
        [SerializeField] private GameObject _screen;
        
        // Resources
        [SerializeField] private GameObject _inventoryList;
        [SerializeField] private TaskResourceUI _resourceUIPrefab;

        // Task
        [SerializeField] private GameObject _taskList;
        [SerializeField] private TextUI _textUIPrefab;

        // Button
        [SerializeField] private GameObject _button;

        // Saved data
        private Inventory _portalInventory;
        private List<Task> _taskToComplete;

        // Saved TaskResourceUI
        private Dictionary<ResourceType, TaskResourceUI> _resourceDictionary;

        private void Start()
        {
            _manager.completeEvent.AddListener(StartUI);
        }

        private void StartUI(Inventory portalInventory, List<Task> taskToComplete)
        {
            _screen.SetActive(true);
            _button.SetActive(false);

            _portalInventory = portalInventory;
            _taskToComplete = taskToComplete;

            _resourceDictionary = new Dictionary<ResourceType, TaskResourceUI>();

            // Clean UI List for resources
            foreach (Transform child in _inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            // Clean UI List for tasks
            foreach (Transform child in _taskList.transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(NextStep()); 
        }

        private IEnumerator NextStep()
        {
            yield return StartPortalInventory();

            yield return StartSpiritCostTask();

            yield return StartTasks();

            yield return new WaitForSeconds(0.5f);

            _button.SetActive(true);
        }

        private IEnumerator StartPortalInventory()
        {
            // Show every portal Resources and stock them in a dictionary to find them easily later
            foreach (Resource resource in _portalInventory.resources)
            {
                yield return new WaitForSeconds(0.5f);

                // Instantiate resource UI
                TaskResourceUI resourceUI = Instantiate(_resourceUIPrefab, _inventoryList.transform);

                // Set type and quantities
                resourceUI.Init(resource);

                _resourceDictionary.Add(resource.type, resourceUI);
            }
        }

        private IEnumerator StartSpiritCostTask()
        {
            int spiritCost = SpiritManager.RecalculateCost();

            if (spiritCost > 0)
            {
                Inventory spiritInventory = new Inventory();
                spiritInventory.Add(new Resource(ResourceType.Food, spiritCost));

                yield return ShowTask(spiritInventory, "Recruitment Cost");
            }
        }

        private IEnumerator StartTasks()
        {
            foreach(Task task in _taskToComplete)
            {
                yield return ShowTask(task.cost, task.description);
            }
        }

        private IEnumerator ShowTask(Inventory taskCost, string description)
        {
            yield return new WaitForSeconds(0.5f);

            // Show used Resources
            foreach (Resource resource in taskCost.resources)
            {
                _resourceDictionary[resource.type].RemoveQuantity(resource.quantity);
            }

            yield return new WaitForSeconds(1f);

            // Instantiate Text
            TextUI textUI = Instantiate(_textUIPrefab, _taskList.transform);

            // Set description of task
            textUI.SetText(description);
        }
    }
}