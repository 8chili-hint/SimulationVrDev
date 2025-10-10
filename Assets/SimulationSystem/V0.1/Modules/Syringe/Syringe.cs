using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class Syringe : MonoBehaviour /*, IHandGrabUseDelegate*/
    {
      /*  public enum SyringeType
        {
            Continues,
            FlipFlop
        }

        [Header("Syringe Type")]
        public SyringeType type = SyringeType.Continues;
    
        [Header("Input")]
        [SerializeField]
        public Transform trigger;
        [SerializeField]
        [Range(0f, 1f)]
        private float _releaseThresold = 0.3f;
        [SerializeField]
        [Range(0f, 1f)]
        private float _fireThresold = 0.9f;
        [SerializeField]
        [Range(0f, 1f)]
        private float _pressedThresold = 0.9f;
        [SerializeField]
        private float _triggerSpeed = 3f;
        [SerializeField]
        private AnimationCurve _strengthCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    
        [SerializeField]
        private FloatEvent WhenInjecting;
        [SerializeField]
        private FloatEvent WhenFilling;
        [SerializeField]
        private FloatEvent WhenInjectingCompleted;
        [SerializeField]
        private FloatEvent WhenFillingCompleted;

        private bool _wasFired = false;
        private bool _wasRefilled = true;
        private float _dampedUseStrength = 0;
        private float _lastUseTime;

        private void Injecting(float value)
        {
            WhenInjecting?.Invoke(value);
        }
    
        private void Refilling(float value)
        {
            WhenFilling?.Invoke(value);
        }
    
        public void BeginUse()
        {
            _dampedUseStrength = 0f;
            _lastUseTime = Time.realtimeSinceStartup;
        }

        public void EndUse()
        {
        
        }

        public float ComputeUseStrength(float strength)
        {
            float delta = Time.realtimeSinceStartup - _lastUseTime;
            _lastUseTime = Time.realtimeSinceStartup;
            if (strength > _dampedUseStrength)
            {
                _dampedUseStrength = Mathf.Lerp(_dampedUseStrength, strength, _triggerSpeed * delta);
            }
            else
            {
                _dampedUseStrength = strength;
            }
            float progress = _strengthCurve.Evaluate(_dampedUseStrength);
            UpdateTriggerProgress(progress);
        
            return progress;
        }
    
        private void UpdateTriggerProgress(float progress)
        {
            UpdateTriggerRotation(progress);
        
            if (progress >= _fireThresold && !_wasFired)
            {
                Injecting(progress);
                if (progress > 0.9f)
                {
                    WhenInjectingCompleted?.Invoke(1);
                    _wasFired = true;
                }
            }
            else if (progress <= _releaseThresold)
            {
                _wasFired = false;
            }

            if (progress <= _pressedThresold && !_wasRefilled)
            {
                Refilling(progress);
                if (progress < 0.1f)
                {
                    WhenFillingCompleted?.Invoke(0);
                    _wasRefilled = true;
                }
            }
            else if(progress >= _releaseThresold)
            {
                if(progress >= 0.9f)
                    _wasRefilled = false;
            }
        }
    
    
        private void UpdateTriggerRotation(float progress)
        {
            if(type == SyringeType.Continues)
                trigger.localPosition = Vector3.Lerp(new Vector3(0, 0, 0.00511f), new Vector3(0, 0, 0), progress);
        }
*/    }
}
