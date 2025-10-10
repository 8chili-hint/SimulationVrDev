using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class Obstacle : MonoBehaviour
    {
        public GameObject obstacle;

        public Material red;
        public Material yellow;
    
        public bool IsDetected
        {
            get;
            private set;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == obstacle)
            {
                IsDetected = true;
                //GetComponent<MeshRenderer>().sharedMaterial = red;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == obstacle)
            {
                IsDetected = false;
                GetComponent<MeshRenderer>().sharedMaterial = yellow;
            }
        }

        private void OnDisable()
        {
            IsDetected = false;
        }
    }
}