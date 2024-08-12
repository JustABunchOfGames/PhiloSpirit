using UnityEngine;
using UnityEngine.Events;

namespace Core
{

    public class CleanManager : MonoBehaviour
    {
        public static CleanCycleEvent cleanCycleEvent = new CleanCycleEvent();

        public void CleanCycle()
        {
            cleanCycleEvent.Invoke();
        }

        public class CleanCycleEvent : UnityEvent { }
    }
}