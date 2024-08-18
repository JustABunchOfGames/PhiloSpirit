using Resources;
using Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TaskDetailsUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TaskScriptable _scriptable;

        [Header("Unselected Details")]
        [SerializeField] private Button _button;
        [SerializeField, TextArea] private string _startText;

        [Header("Texts")]
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Text _buttonText;
        [SerializeField, TextArea] private string _acceptText;
        [SerializeField, TextArea] private string _abandonText;

        [Header("List")]
        [SerializeField] private ResourceUI _resourceUIPrefab;
        [SerializeField] private GameObject _costList;

        private void Start()
        {
            CleanDetails();

            _scriptable.showEvent.AddListener(ShowDetails);
            _scriptable.acceptEvent.AddListener(ShowState);
            _scriptable.completeEvent.AddListener(CleanDetails);
        }

        public void CleanDetails()
        {
            // Show "Clean" Text
            _nameText.text = "";
            _descriptionText.text = _startText;

            // Clean UI List
            foreach (Transform child in _costList.transform)
            {
                Destroy(child.gameObject);
            }

            // Hide button
            _button.gameObject.SetActive(false);
        }

        public void ShowDetails(Task task)
        {
            // Set Texts & Button State
            _nameText.text = task.name;
            _descriptionText.text = task.description;

            ShowState(task);

            // Set List

            // Clean UI List
            foreach (Transform child in _costList.transform)
            {
                Destroy(child.gameObject);
            }

            // For every resoure in the task cost
            foreach (Resource resource in task.cost.resources)
            {
                // Instantiate resource UI
                ResourceUI resourceUI = Instantiate(_resourceUIPrefab, _costList.transform);

                // Set type and quantity
                resourceUI.Init(resource);
            }
        }

        private void ShowState(Task task)
        {
            switch (task.state)
            {
                case TaskState.Available:
                    _button.gameObject.SetActive(true);
                    _buttonText.text = _acceptText;
                    break;

                case TaskState.Accepted:
                    _buttonText.gameObject.SetActive(true);
                    _buttonText.text = _abandonText;
                    break;

                case TaskState.Completed:
                    _button.gameObject.SetActive(false);
                    break;
            }
        }
    }
}