using Tasks;
using UnityEngine;

namespace UI
{

    public class TaskButton : MonoBehaviour
    {
        [SerializeField] private TaskManager _manager;

        [SerializeField] private Task _task;

        private bool _isAccepted = false;

        public void AcceptTask()
        {
            if (_isAccepted)
            {
                _manager.RemoveTask(_task);
                _isAccepted = false;
            }
            else
            {
                _manager.AddTask(_task);
                _isAccepted = true;
            }
        }
    }
}