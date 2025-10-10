using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect
{
    public class HandStatus : MonoBehaviour
    {
        public bool IsHandDetecting { get; private set; }
        public string DetectedObjectName { get; private set; }
        public GameObject HasObjectToDetect { get; private set; }
        public DetectStates State { get; private set; } = DetectStates.Normal;
        public DominantHand hand = DominantHand.None;
        public HandType handType = HandType.None;
        public UnityEvent<string> onDetectUpdate;

        private void Update()
        {
            if (HasObjectToDetect != null && State == DetectStates.Normal)
            {
                State = DetectStates.Detect;
                onDetectUpdate?.Invoke(DetectedObjectName);
            }
            else if (HasObjectToDetect == null && State == DetectStates.Detect)
            {
                State = DetectStates.UnDetect;
                HasObjectToDetect = null;
                IsHandDetecting = false;
                DetectedObjectName = "";
                onDetectUpdate?.Invoke("");
            }
            else if (HasObjectToDetect != null && State == DetectStates.UnDetect || HasObjectToDetect == null && State == DetectStates.UnDetect)
            {
                State = DetectStates.Normal;
                HasObjectToDetect = null;
                IsHandDetecting = false;
                DetectedObjectName = "";
                onDetectUpdate?.Invoke("");
            }

            if (HasObjectToDetect)
            {
                if(!HasObjectToDetect.activeInHierarchy)
                {
                    HasObjectToDetect = null;
                    IsHandDetecting = false;
                    DetectedObjectName = "";
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out DetectObject obj))
            {
                // if(!obj.ShowLabel) return;
                // HasObjectToDetect = obj.gameObject;
                // IsHandDetecting = true;
                // DetectedObjectName = obj.ObjectName;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out DetectObject obj))
            {
                // if(!obj.ShowLabel) return;
                // HasObjectToDetect = null;
                // IsHandDetecting = false;
                // DetectedObjectName = "";
            }
        }
    }
}
