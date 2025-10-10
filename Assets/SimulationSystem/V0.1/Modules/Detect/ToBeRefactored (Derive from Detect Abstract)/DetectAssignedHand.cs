using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_
{
    [RequireComponent(typeof(DetectHand))]
    public class DetectAssignedHand : MonoBehaviour
    {
        public HandType assignedHand = HandType.None;
    }
    
    [System.Serializable]
    public enum HandType
    {
        DominantHand,
        NonDominantHand,
        None
    }
}