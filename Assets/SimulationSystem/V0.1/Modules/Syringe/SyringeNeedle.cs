using System;
using SimulationSystem.V0._1.Simulation;
using TMPro;
using UnityEngine;

//using LeTai;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class SyringeNeedle : MonoBehaviour
    {
        private SyringeController _syringeController;
        public GameObject veil, contactPlane;
        public LayerMask layerMask;
        private bool checkAngle = false;
        public bool shouldCheckAngle, changeStateOnContact = true;
        public TextMeshProUGUI angleText;
        public SimulationManager simulationManager;
        private float angle;
        public GameObject ray;
    
        private void Start()
        {
            _syringeController = this.transform.parent.GetComponent<SyringeController>();
        }

        private void FixedUpdate()
        {
            if (checkAngle)
            {
                if(ray) ray.SetActive(true);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * -1f, out hit, 1f, layerMask))
                {
                    Vector3 normal = new Vector3();
                    normal = contactPlane.transform.TransformDirection(Vector3.up);
                    Vector3 rayHit = new Vector3();
                    rayHit = transform.TransformDirection(Vector3.forward);
                    angle = 90f - Vector3.Angle(normal, rayHit);
                    angleText.text = Math.Round(angle,0).ToString() + "°";
                }
            }
            else
            {
                if(ray) ray.SetActive(false);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == veil)
            {
                _syringeController.veilTouch = false;
             /*   OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);*/
            }
            else if (other.gameObject.name == "EntryCollider")
            {
                checkAngle = false;
            }
            else if (other.gameObject.name == "ContactPoint")
            {
                _syringeController.injectionPointContact = false;
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == veil)
            {
                if(_syringeController == null) _syringeController = this.transform.parent.GetComponent<SyringeController>();
                _syringeController.veilTouch = true;
            }
            else if (other.gameObject.name == "ContactPoint")
            {
                checkAngle = false;
                angleText.text = "First Contact: " + Math.Round(angle,0).ToString() + "°";

                _syringeController.mode = SyringeController.Mode.Out;

                if (!_syringeController.injectionPointContact && changeStateOnContact)
                {
                    simulationManager.NextState();
                    changeStateOnContact = false;
                }
            
                _syringeController.injectionPointContact = true;
            
                //HapticManager.Instance.Haptic(0.2f, FreeGrabBehavior.HandSide.Right);
            }
            else if (other.gameObject.name == "EntryCollider" && shouldCheckAngle)
            {
                if(!_syringeController.injectionPointContact) checkAngle = true;
            }
        }
    }
}
