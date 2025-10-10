using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class SyringeInjectingPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InjectingPoint"))
            {
                if (other.GetComponentInParent<SyringeController>())
                {
                    other.GetComponentInParent<SyringeController>().injectionPointContact = true;
                    other.GetComponentInParent<SyringeController>().mode = SyringeController.Mode.Out;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InjectingPoint"))
            {
                if(other.GetComponentInParent<SyringeController>())
                    other.GetComponentInParent<SyringeController>().injectionPointContact = false;
            }
        }
    }
}
