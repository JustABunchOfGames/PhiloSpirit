using Resources;
using Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class TaskDetailsUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TaskManager _manager;

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

        private Task _currentTask;
        private bool _accepted;

        public AcceptEvent acceptEvent = new AcceptEvent();

        private void Start()
        {
            CleanDetails();
        }

        public void CleanDetails()
        {
            _currentTask = null;
            acceptEvent.RemoveAllListeners();

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

        public void ShowDetails(Task task, bool accepted)
        {
            // Clear event listener
            acceptEvent.RemoveAllListeners();

            // Show button
            _button.gameObject.SetActive(true);

            // Save for later button click
            _currentTask = task;
            _accepted = accepted;

            // Set Texts
            _nameText.text = task.name;
            _descriptionText.text = task.description;
            _buttonText.text = accepted ? _abandonText : _acceptText;

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

        public void AcceptTask()
        {
            if (_currentTask == null)
                return;

            // if the task for not already accepted
            if (!_accepted)
            {
                _manager.AddTask(_currentTask);

                _buttonText.text = _abandonText;
            }
            else
            {
                _manager.RemoveTask(_currentTask);

                _buttonText.text = _acceptText;
            }
            

            _accepted = !_accepted;
            acceptEvent.Invoke();
        }

        public class AcceptEvent : UnityEvent { }

    }
}