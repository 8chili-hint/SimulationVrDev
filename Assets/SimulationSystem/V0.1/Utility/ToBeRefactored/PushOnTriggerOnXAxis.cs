using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class PushOnTriggerOnXAxis : MonoBehaviour
    {
        public bool CanPush { get; set; } = true;
    
        public Transform pushableObject;
        public float maxDistance = 0;

        public UnityEvent onDetectComplete;

        private Transform _originalTransform;

        private void UpdatePositionOnTrigger()
        {
            if (pushableObject.localPosition.x < maxDistance)
                pushableObject.localPosition += new Vector3(0.01f * Time.deltaTime, 0, 0);
            else if (pushableObject.localPosition.x > maxDistance && pushableObject.localPosition != Vector3.zero)
            {
                onDetectComplete?.Invoke();
                pushableObject.localPosition = new Vector3(0, 0, 0);
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if(!CanPush)
                return;
        
            if (other.transform.name.Equals("b_r_thumb3_CapsuleCollider") || other.transform.name.Equals("b_l_thumb3_CapsuleCollider"))
            {
                UpdatePositionOnTrigger();
            }
        }

        public void ResetPosition()
        {
            pushableObject.localPosition = new Vector3(-0.04731107f, 0, 0);
        }
    }
}
