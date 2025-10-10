using System;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect.Interface;
using SimulationSystem.V0._1.Modules.Detect.Legacy;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_
{
    [RequireComponent(typeof(Detectable))]
    public class DetectDistance : MonoBehaviour, IDetect
    {
        [Header("[Check in World Position]")] [SerializeField]
        private bool canWorld = false;
        
        [Space(10)]
        [SerializeField] 
        private Vector3 desiredDistance = Vector3.zero;

        private Transform objectToDetect;



        private Transform objectToDetectTo;

        [SerializeField] 
        private float precision = 1.999f;

        [FormerlySerializedAs("minCheck")] [SerializeField] private float toCheck = 0.15f;
        [FormerlySerializedAs("maxCheck")] [SerializeField] private float fromCheck = 0f;
        [SerializeField] private float minTravel = -0.05f;
        [SerializeField] private float maxTravel = -0.05f;

        public float ToCheck
        {
            get { return toCheck; }
            set { toCheck = value; }
        }

        public float FromCheck
        {
            get { return fromCheck; }
            set { fromCheck = value; }
        }

        [SerializeField] 
        private UITextHandler textHandler;

        [Space(10)] 
        public UnityEvent OnDetect;
        public UnityEvent OnUnDetect;
        public UnityEvent<float> OnDetectingEvent;
        public UnityEvent<float> OnDetectingFullTravel;
        public UnityEvent OnDetectComplete;

        private bool canDetect = true;
        private bool isFirstTriggerComplete = true;
        private DetectAudioHandler detectAudioHandler;
        private DetectStates _detectStates = DetectStates.Normal;
        public virtual void Initialize()
        {
            //precision = 1.9999f;

            if (objectToDetect != null)
            {
                desiredDistance = !canWorld ? objectToDetect.localPosition : objectToDetect.position;
            }

            if (objectToDetectTo == null)
                objectToDetectTo = this.transform;
            
            if (TryGetComponent<DetectAudioHandler>(out var audioHandler))
            {
                detectAudioHandler = audioHandler;
            }
        }

        private float distanceLeft = 0;
        public virtual void UpdateLoop()
        {
            if (canDetect)
            {
                var dist = !canWorld
                    ? objectToDetectTo.DistanceFrom(desiredDistance, precision, out distanceLeft)
                    : objectToDetectTo.DistanceFromAtWorld(desiredDistance, precision, out distanceLeft);
                
                switch (dist)
                {
                    case true when _detectStates == DetectStates.Normal:
                    {
                        OnDetect?.Invoke();
                    
                        if(detectAudioHandler)
                            detectAudioHandler.PlayDetectEffect();

                        _detectStates = DetectStates.Detect;
                        break;
                    }
                    case false when _detectStates == DetectStates.Detect:
                        OnUnDetect?.Invoke();
                        _detectStates = DetectStates.UnDetect;
                        break;
                    case false when _detectStates == DetectStates.UnDetect:
                    case true when _detectStates == DetectStates.UnDetect:
                        _detectStates = DetectStates.Normal;
                        break;
                }

                var percentage =
                    Mathf.Floor(Mathf.Lerp(0, 100, 1 - Mathf.InverseLerp(toCheck, fromCheck, objectToDetectTo.localPosition.z))); // convert again to 0-1
                
                OnDetectingFullTravel?.Invoke(1 - Mathf.InverseLerp(minTravel, maxTravel, objectToDetectTo.localPosition.z)); //remove
                
                if (percentage <= 100)
                {
                    OnDetectingEvent?.Invoke(1 - Mathf.InverseLerp(toCheck, fromCheck, distanceLeft));
                    if (textHandler)
                        textHandler.UpdateText(String.Format("Total Detect {0} %.", percentage)); //Remove
                    if (percentage >= 99 && isFirstTriggerComplete)
                    {
                        OnDetectComplete?.Invoke();
                        isFirstTriggerComplete = false;
                    }
                }
            }
        }

        public void OnDetectionStart(Collider other)
        {
            throw new NotImplementedException();
        }

        public void OnDetecting(Collider other)
        {
            throw new NotImplementedException();
        }

        public void OnDetectionEnd(Collider other)
        {
            throw new NotImplementedException();
        }

        public void SetDesiredDistanceX(float x)
        {
            desiredDistance.x = x;
        }
        
        public void SetDesiredDistanceY(float y)
        {
            desiredDistance.y = y;
        }
        
        public void SetDesiredDistanceZ(float z)
        {
            desiredDistance.z = z;
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

        public virtual void StartDetection()
        {
            canDetect = true;
        }

        public virtual void StopDetection()
        {
            canDetect = false;
            isFirstTriggerComplete = true;
        }
    }
}