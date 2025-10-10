using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class FakeChild : MonoBehaviour
    {
        public bool lockY;
        public Transform Parent;//Remember to assign the parent transform 
        private Vector3 pos, fw, up;
 
        void Start()
        {
        
        }

        private void OnEnable()
        {
            pos = Parent.transform.InverseTransformPoint(transform.position);
            fw = Parent.transform.InverseTransformDirection(transform.forward);
            up = Parent.transform.InverseTransformDirection(transform.up);
        }

        void Update()
        {
            var newpos = Parent.transform.TransformPoint(pos);
            var newfw = Parent.transform.TransformDirection(fw);
            var newup = Parent.transform.TransformDirection(up);
            var newrot = Quaternion.LookRotation(newfw, newup);
            transform.position = lockY ? new Vector3(newpos.x, transform.position.y, newpos.z) : newpos;
            transform.rotation = newrot;
        }
    }
}