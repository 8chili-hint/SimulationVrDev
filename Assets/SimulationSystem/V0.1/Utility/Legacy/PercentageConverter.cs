using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Legacy
{
    public class PercentageConverter : MonoBehaviour
    {
        public UnityEvent<float> onUpdate;

        public void OnUpdateValue(float value)
        {
            onUpdate?.Invoke(Mathf.Lerp(0,100, value));
        }
    }
}
