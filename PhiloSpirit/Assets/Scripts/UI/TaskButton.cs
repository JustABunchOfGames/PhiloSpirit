using Tasks;
using UnityEngine;

namespace UI
{

    public class TaskButton : MonoBehaviour
    {
        [SerializeField] private TaskDetailsUI _details;

        [SerializeField] private Task _task;

        public bool isAccepted { get; private set; }

        private void Start()
        {
            isAccepted = false;
        }

        public void Select()
        {
            _details.ShowDetails(_task, isAccepted);

            _details.acceptEvent.AddListener(Accept);
        }

        private void Accept()
        {
            isAccepted = !isAccepted;
        }
    }
}