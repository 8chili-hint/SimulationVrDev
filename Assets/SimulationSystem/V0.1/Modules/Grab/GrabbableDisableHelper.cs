
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Grab
{
    public class GrabbableDisableHelper : MonoBehaviour
    {
        private void Awake()
        {
           /* var grabs = GetComponentsInChildren<Grabbable>();

            foreach (Grabbable grab in grabs)
            {
                var gr = grab.GetComponentsInChildren<HandGrabInteractable>();
                foreach (var g in gr)
                {
                    g.enabled = false;
                }
            }*/
        }
    }
}