using Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TaskButton : MonoBehaviour
    {
        [SerializeField] private TaskScriptable _scriptable;

        [SerializeField] private Task _task;

        [SerializeField] private TaskButton _nextTask;

        private Button _button;
        private GameObject _indicator;

        private void Start()
        {
            _button = GetComponent<Button>();

            ShowState();

            _task.stateChangeEvent.AddListener(ShowState);
        }

        public void Select()
        {
            _scriptable.ShowTask(_task);
        }

        public void Unlock()
        {
            _button.interactable = true;

            _task.state = TaskState.Available;
            ShowState();
        }

        public void ShowState()
        {
            // Clean old indicator
            if (_indicator != null)
                Destroy(_indicator);

            // Get state from task
            TaskState state = _task.state;

            // Lock self or unlock next if necessary
            if (state == TaskState.Locked)
                GetComponent<Button>().interactable = false;
            else if (state == TaskState.Completed && _nextTask != null)
                _nextTask.Unlock();

            // Show indicator
            TaskScriptable.TaskStateIndicator indicator = _scriptable.GetStateIndicator(state);

            _indicator = Instantiate(indicator.indicator, transform);
            _indicator.transform.localPosition = indicator.position;
        }
    }
}