
using SimulationSystem.V0._1.Modules.Detect.Interface;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using SimulationSystem.V0._1.Utility.Extensions;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_
{
    public class DetectAngle : MonoBehaviour, IDetect
    {
        [SerializeField] 
        private Quaternion desiredAngle = Quaternion.identity;

        [SerializeField]
        private Transform objectToDetect;

        [SerializeField] 
        private float precision = 0.9f;

        [Space(10)] 
        public UnityEvent OnDetect;

        private bool canDetect = true;
        private bool isFirstTrigger = true;
        private DetectAudioHandler detectAudioHandler;
        
        public void Initialize()
        {
            //precision = 0.9999f;
            //desiredAngle = Quaternion.Euler(0, 90, 0);

            if (objectToDetect == null)
                objectToDetect = this.transform;
            
            if (TryGetComponent<DetectAudioHandler>(out var audioHandler))
            {
                detectAudioHandler = audioHandler;
            }
        }

        public void UpdateLoop()
        {
            if (canDetect)
            {
                bool value = objectToDetect.AngleFrom(desiredAngle, precision);
                if (value && isFirstTrigger)
                {
                    OnDetect?.Invoke();
                    
                    if(detectAudioHandler)
                        detectAudioHandler.PlayDetectEffect();

                    isFirstTrigger = false;
                }
            }
        }

        public void OnDetectionStart(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public void OnDetecting(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public void OnDetectionEnd(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public void StartDetection()
        {
            canDetect = true;
        }

        public void StopDetection()
        {
            canDetect = false;
            isFirstTrigger = true;
        }

        public void OnObjectEnter(Collider other)
        {
            
        }

        public void OnObjectStay(Collider other)
        {
            
        }

        public void OnObjectExit(Collider other)
        {
            
        }
    }
}
