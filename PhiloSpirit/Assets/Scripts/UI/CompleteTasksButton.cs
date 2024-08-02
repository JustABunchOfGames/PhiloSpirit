using Resources;
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
            ChangeState();
        }

        private void Start()
        {
            _taskInventory.costChangedEvent.AddListener(ChangeState);
        }

        private void ChangeState()
        {
            _completeTasksButton.interactable = _taskInventory.IsTaskCompletionPossible();
        }

        public void CompleteTasks()
        {
            _taskInventory.CompleteTasks();
        }
    }
}