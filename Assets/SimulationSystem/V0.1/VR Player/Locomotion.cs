using UnityEngine;

namespace SimulationSystem.V0._1.VR_Player
{
    public class Locomotion : MonoBehaviour
    {
        private CharacterController controller;
    
        [SerializeField] private float playerSpeed = 0.4f;
    
        private void Awake()
        {
            if (TryGetComponent(out CharacterController controller))
            {
                this.controller = controller;
            }
            else
            {
                this.controller = gameObject.AddComponent<CharacterController>();
                this.controller.center = new Vector3(0, 0.35f, 0);
                this.controller.radius = 0.05f;
                this.controller.height = 0.71f;
            }
        }

        void FixedUpdate()
        {/*
            float inputX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick,OVRInput.Controller.LTouch).x;
            float inputy = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick,OVRInput.Controller.LTouch).y;*/

            /*Vector3 move = new Vector3(inputX, 0, inputy);*/
           /* controller.Move(move * Time.deltaTime * playerSpeed);*/
        }
    }
}
