using System;
using UnityEngine;

namespace SimulationSystem.V0._1.Warnings
{
    public sealed class DetectWarningTrigger : DetectWarning
    {
        [SerializeField] private GameObject detectObject;
        
        private bool isFirstDetect = true;

        private void Start()
        {
            CanCheck = true;
        }

        private void Update()
        {
            UpdateState();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == detectObject)
            {
                hasDetected = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == detectObject)
            {
                hasDetected = false;
            }
        }
        
        public void UpdateOnce(Action action)
        {
            if (!isFirstDetect)
            {
                action?.Invoke();
                isFirstDetect = false;
            }
        }
    }
}