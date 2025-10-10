using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class PushOnTrigger : MonoBehaviour
    {
        public Transform pushableObject;
        public float maxDistance = 0;

        public UnityEvent onDetectComplete;
        private void UpdatePositionOnTrigger()
        {
            if (pushableObject.localPosition.z > maxDistance)
                pushableObject.localPosition -= new Vector3(0, 0, 0.01f * Time.deltaTime);
            else if (pushableObject.localPosition.z < maxDistance && pushableObject.localPosition != Vector3.zero)
            {
                onDetectComplete?.Invoke();
                pushableObject.localPosition = new Vector3(0, 0, 0);
            }

        }

        public void OnTriggerStay(Collider other)
        {
            if (other.transform.name.Equals("b_r_thumb3_CapsuleCollider") || other.transform.name.Equals("b_l_thumb3_CapsuleCollider"))
            {
                UpdatePositionOnTrigger();
            }
        }
    }
}