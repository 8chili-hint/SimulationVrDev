using SimulationSystem.V0._1.Utility.Miscellanous;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class SyringeWarning : MonoBehaviour
    {
        public GameObject needleDetect;
        public AngleCalculator needleAngle;

        private bool _inRange;
        private bool _inDetecttion;
        private bool _firstCall1 = true;
        private bool _firstCall2 = true;

        public UnityEvent onDetect;
        public UnityEvent onUnDetect;
        private void Start()
        {
            needleAngle.inRange.AddListener( g => _inRange = g);
        }

        private void Update()
        {
            if (_inDetecttion)
            {
                if (!_inRange && _firstCall1)
                {
                    onDetect?.Invoke();
                    _firstCall1 = false;
                    _firstCall2 = true;
                }
                else if(_inRange && !_firstCall1)
                {
                    onUnDetect?.Invoke();
                    _firstCall1 = true;
                }
            }
            else
            {
                if (_firstCall2)
                {
                    onUnDetect?.Invoke();
                    _firstCall1 = true;
                    _firstCall2 = false;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == needleDetect)
            {
                _inDetecttion = true;
            }
        
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == needleDetect)
            {
                _inDetecttion = false;
            }
        }
    }
}
