using System.Collections.Generic;
using Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{

    public class TaskButton : MonoBehaviour
    {
        [SerializeField] private TaskScriptable _scriptable;

        [SerializeField] private Text _buttonName;

        [SerializeField] private Task _task;

        // Unlocks
        [SerializeField] private List<TaskButton> _taskToUnlock;
        public CompleteEvent completeEvent = new CompleteEvent();

        private Button _button;
        private GameObject _indicator;

        private void Start()
        {
            _button = GetComponent<Button>();

            ShowState();

            _task.stateChangeEvent.AddListener(ShowState);

            Transform parent = transform.GetComponentInParent<Transform>();
            foreach (TaskButton button in _taskToUnlock)
            {
                button.completeEvent.AddListener(Unlock);                
                DrawLineBetweenObject.DrawLine(button.transform, transform, Color.black);
            }
        }

        public void Select()
        {
            _scriptable.ShowTask(_task);
        }

        public void Unlock()
        {
            // CHeck if every prior task is completed
            foreach(TaskButton button in _taskToUnlock)
            {
                if (button._task.state != TaskState.Completed)
                    return;
            }

            // if it is, unlock this task
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
            else if (state == TaskState.Completed)
                completeEvent.Invoke();

            // Show indicator
            TaskScriptable.TaskStateIndicator indicator = _scriptable.GetStateIndicator(state);

            _indicator = Instantiate(indicator.indicator, transform);
            _indicator.transform.localPosition = indicator.position;
        }

        public void ApplyName()
        {
            name = _task.name;
            _buttonName.text = name;
        }

        public class CompleteEvent : UnityEvent { }

#if UNITY_EDITOR
        [CustomEditor(typeof(TaskButton))]
        public class TaskButtonEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                TaskButton taskButton = (TaskButton)target;
                if (GUILayout.Button("ApplyName"))
                {
                    taskButton.ApplyName();
                }
            }
        }
#endif
    }
}