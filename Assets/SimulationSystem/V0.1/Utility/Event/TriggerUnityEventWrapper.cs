using System;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Event
{
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider>
    {
    }

    public class TriggerUnityEventWrapper : MonoBehaviour
    {
        public ColliderEvent onTriggerEnterEvent = new ColliderEvent();
        public ColliderEvent onTriggerExitEvent = new ColliderEvent();
        public ColliderEvent onTriggerStayEvent = new ColliderEvent();

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStayEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExitEvent?.Invoke(other);
        }
    }
}