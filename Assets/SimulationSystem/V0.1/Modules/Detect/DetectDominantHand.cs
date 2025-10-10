using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect
{
    public class DetectDominantHand : MonoBehaviour
    {
        [SerializeField] private UnityEvent onDominantHandDetected;
        [SerializeField] private UnityEvent onNonDominantHandDetected;
    
        [SerializeField] private UnityEvent onDominantHandUnDetected;
        [SerializeField] private UnityEvent onNonDominantHandUnDetected;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out HandStatus status))
            {
                if (status.handType == HandType.DominantHand)
                {
                    onDominantHandDetected?.Invoke();
                }

                if (status.handType == HandType.NonDominantHand)
                {
                    onNonDominantHandDetected?.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out HandStatus status))
            {
                if (status.handType == HandType.DominantHand)
                {
                    onDominantHandUnDetected?.Invoke();
                }

                if (status.handType == HandType.NonDominantHand)
                {
                    onNonDominantHandUnDetected?.Invoke();
                }
            }
        }
    }
}
