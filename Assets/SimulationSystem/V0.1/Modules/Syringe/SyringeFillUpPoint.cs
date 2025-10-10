using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class SyringeFillUpPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InjectingPoint"))
            {
                if (other.GetComponentInParent<SyringeController>())
                {
                    other.GetComponentInParent<SyringeController>().veilTouch = true;
                    other.GetComponentInParent<SyringeController>().mode = SyringeController.Mode.In;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InjectingPoint"))
            {
                if(other.GetComponentInParent<SyringeController>())
                    other.GetComponentInParent<SyringeController>().veilTouch = false;
            }
        }
    }
}
