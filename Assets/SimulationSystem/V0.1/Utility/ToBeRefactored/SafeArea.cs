using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class SafeArea : MonoBehaviour
    {
        public GameObject obstacle;
    
        public bool IsDetected
        {
            get;
            private set;
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == obstacle)
            {
                IsDetected = true;
            }
        }
    }
}