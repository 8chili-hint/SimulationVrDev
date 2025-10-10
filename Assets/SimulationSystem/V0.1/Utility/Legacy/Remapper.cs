using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Legacy
{
    public class Remapper : MonoBehaviour
    {
        public float minValue;
        public float maxValue;

        public FloatEvent onRemap;
        private float _time = 0f;

        public void UpdateValue(float value)
        {
            onRemap?.Invoke(Mathf.Lerp(minValue, maxValue, value));
        }
    }
}
