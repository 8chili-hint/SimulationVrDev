using System;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class AngleCalculator : MonoBehaviour
    {
        public bool checkAngle = true;
        public GameObject contactPlane;
    
        [SerializeField] private float currentAngle;
        [SerializeField] private float minCheckAngle = 35f;
        [SerializeField] private float maxCheckAngle = 60f;
        [SerializeField] private Vector3 normalAxis = Vector3.up;
        [SerializeField] private LayerMask layerMask;
        [Space(10)]
        [SerializeField] public UnityEvent<bool> inRange;
        [SerializeField] private UnityEvent<double> onUpdateAngle;
        [SerializeField] private UnityEvent<bool> inRangeOnce;

        private bool _firstCall1 = true;
        private bool _firstCall2 = true;
        private void FixedUpdate()
        {
            if (!checkAngle)
                return;
        
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward * -1f, out hit, 1f, layerMask))
            {
                Vector3 normal = new Vector3();
                normal = contactPlane.transform.TransformDirection(normalAxis);
                Vector3 rayHit = new Vector3();
                rayHit = transform.TransformDirection(Vector3.forward);
            
                currentAngle = 90f - Vector3.Angle(normal, rayHit);
                
                if (currentAngle < maxCheckAngle && currentAngle > minCheckAngle)
                {
                    inRange?.Invoke(true);

                    if (_firstCall1)
                    {
                        inRangeOnce?.Invoke(true);
                        _firstCall1 = false;
                        _firstCall2 = true;
                    }
                }
                else
                {
                    inRange?.Invoke(false);

                    if (_firstCall2)
                    {
                        inRangeOnce?.Invoke(false);
                        _firstCall1 = true;
                        _firstCall2 = false;
                    }
                }

                onUpdateAngle?.Invoke(currentAngle >= 0
                    ? Math.Round(currentAngle, 0)
                    : Math.Round((360 - Math.Abs(currentAngle)) % 360, 0));
            }
        }
    }
}
