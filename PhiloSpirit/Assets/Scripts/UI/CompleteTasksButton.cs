using Resources;
using Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class CompleteTasksButton : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TaskManager _manager;

        [Header("UI")]
        [SerializeField] private Button _completeTasksButton;

        private void Start()
        {
            _completeTasksButton.interactable = false;

            _manager.costChanged.AddListener(ChangeState);
        }

        private void ChangeState(Inventory portalInventory, Inventory taskInventory)
        {

            if (taskInventory == null || taskInventory.resources.Count == 0)
            {
                _completeTasksButton.interactable = false;
                return;
            }

            
            foreach (Resource resource in taskInventory.resources)
            {
                if (!portalInventory.HasEnough(resource))
                {
                    _completeTasksButton.interactable = false;
                    return;
                }
            }

            _completeTasksButton.interactable = true;
        }
    }
}