using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.BoundDetect
{
    public class BoundsBoxCast : MonoBehaviour
    {
        public bool isDetect = false;

        [SerializeField] private Transform detectTransform;
        [SerializeField] private Vector3 cubeSize = new Vector3(0.04f, 0.04f, 0.04f);
        [SerializeField] private float maxDistance = 0.04f;
        [SerializeField] private Color cubeColor;
        [SerializeField] private LayerMask layerMask;
        [Space(10)] 
        [SerializeField] private UnityEvent<bool> onDetectUpdate;
    
        private RaycastHit _hit;
        private void FixedUpdate()
        {
            /*if (Physics.BoxCast(transform.position, cubeSize / 2, transform.forward, out _hit,
                transform.localRotation,
                maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            if (_hit.transform == detectTransform)
            {
                isDetect = true;
            }
            else
            {
                isDetect = false;
            }
        }
        else
        {
            isDetect = false;
        }*/
        
            DoBoxCast();
            onDetectUpdate?.Invoke(isDetect);
        }
    
        private void DoBoxCast()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.localRotation;
            Vector3 direction = transform.forward;
            var size = cubeSize / 2f;
    
            if (Physics.BoxCast(position, size, direction, out _hit, rotation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                isDetect = _hit.transform == detectTransform;
            }
            else
            {
                isDetect = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = cubeColor;
            Gizmos.DrawWireCube(this.transform.position + transform.forward * maxDistance,
                cubeSize);
        }
    }
}
