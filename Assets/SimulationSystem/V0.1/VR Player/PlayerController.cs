using System.Collections;
using SimulationSystem.V0._1.Utility.Miscellanous;
using UnityEngine;
using UnityEngine.Events;

//using UnityEngine.InputSystem; ////Build Crash issue with new Input system

namespace SimulationSystem.V0._1.VR_Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private FadeInOut fader;
        [SerializeField] private Transform newTeleportPosition;

        [SerializeField] private UnityEvent onTeleportationComplete;

        private void Update()
        {
            //if (joyStickRight.action.ReadValue<Vector2>().y > 0.9)
           /* if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.9)
            {
                this.transform.localPosition += new Vector3(0, 0.3f, 0) * Time.deltaTime;
            }
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.9)
            {
                this.transform.localPosition -= new Vector3(0, 0.3f, 0) * Time.deltaTime;
            }
            
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.9)
            {
                this.transform.localPosition += new Vector3(0, 0, 0.3f) * Time.deltaTime;
            }
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.9)
            {
                this.transform.localPosition -= new Vector3(0, 0, 0.3f) * Time.deltaTime;
            }
            
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x > 0.9)
            {
                this.transform.localPosition += new Vector3(0.3f, 0, 0) * Time.deltaTime;
            }
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x < -0.9)
            {
                this.transform.localPosition -= new Vector3(0.3f, 0, 0) * Time.deltaTime;
            }*/

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TeleportThePlayer(newTeleportPosition);
            }
        }

        public void TeleportThePlayer(Transform newPos)
        {
            StartCoroutine(Teleport(newPos));
        }
        
        public void TeleportThePlayer()
        {
            StartCoroutine(Teleport(newTeleportPosition));
        }

        private IEnumerator Teleport(Transform newPos)
        {
            yield return fader.FadeIn();
            yield return new WaitForSeconds(1);
            this.transform.localPosition = newPos.localPosition;
            this.transform.localRotation = newPos.localRotation;
            yield return new WaitForSeconds(1);
            onTeleportationComplete?.Invoke();
            yield return fader.FadeOut();
        }

        private void OnDisable()
        {
            /*joyStickRight.action.Disable();
            joyStickLeft.action.Disable();*/
        }
    }
}
