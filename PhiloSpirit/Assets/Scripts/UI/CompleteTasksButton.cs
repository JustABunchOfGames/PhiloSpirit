using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class CompleteTasksButton : MonoBehaviour
    {
        [SerializeField] private Button _completeTasksButton;

        [SerializeField] private TaskInventoryUI _taskInventory;

        private void OnEnable()
        {
            _completeTasksButton.interactable = _taskInventory.IsTaskCompletionPossible();
        }

        public void CompleteTasks()
        {
            _taskInventory.CompleteTasks();
        }
    }
}