using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Event
{
    public class EnableDisableEvent : MonoBehaviour
    {
        public UnityEvent onEnable;
        public UnityEvent onDisable;
        public void OnEnable()
        {
            onEnable?.Invoke();
        }

        public void OnDisable()
        {
            onDisable?.Invoke();
        }
    }
}
