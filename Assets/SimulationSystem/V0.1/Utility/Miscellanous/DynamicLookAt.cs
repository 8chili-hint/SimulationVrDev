using SimulationSystem.V0._1.Manager;
using UnityEngine;
namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class DynamicLookAt : MonoBehaviour
    {
        public Transform overrideFollowTransform;
        public bool rotateAlongY;
        private Transform _followTransform;

        private void Start()
        {
            if (overrideFollowTransform)
                _followTransform = overrideFollowTransform;
            else
                _followTransform = PlayerManager.Instance.mainCamera;
        }

        private void LateUpdate()
        {
            DynamicRotate();
        }

        private void DynamicRotate()
        {
            Vector3 direction = (_followTransform.transform.position - transform.position).normalized;
            if (rotateAlongY)
            {
                direction = new Vector3(direction.x, 0f, direction.z);
            }
            Quaternion rotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
        }
    }
}