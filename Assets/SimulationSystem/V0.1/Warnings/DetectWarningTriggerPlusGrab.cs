using SimulationSystem.V0._1.Modules.Detect.Legacy;
using UnityEngine;

namespace SimulationSystem.V0._1.Warnings
{
    [RequireComponent(typeof(DetectObjectDynamic))]
    public class DetectWarningTriggerPlusGrab : DetectWarning
    {
        public void Start()
        {
            if (!TryGetComponent(out DetectObjectDynamic detectObject)) return;
            detectObject.onDetected.AddListener(OnGrab);
            detectObject.onUnDetect.AddListener(OnUnDetect);
            detectObject.OnDetectingWithGrab.AddListener(delegate { OnUnDetect(); });
        }
        
        private void Update()
        {
            UpdateState();
        }
        
        public void OnGrab()
        {
            hasDetected = true;
            CanCheck = true;
        }

        public void OnUnDetect()
        {
            hasDetected = false;
        }
    }
}
