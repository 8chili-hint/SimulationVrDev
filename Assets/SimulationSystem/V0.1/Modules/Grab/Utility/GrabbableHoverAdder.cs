using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Grab.Utility
{
    public class GrabbableHoverAdder : MonoBehaviour
    {
        public void BuildObject()
        {
          /*  var grabbables = FindObjectsOfType(typeof(Grabbable),true);
        
            foreach (var grabbableObject in grabbables)
            {
                var collider = grabbableObject.GetComponentInChildren<Collider>();
            
                if (collider != null)
                {
                    var grabvisues = collider.GetComponents<GrabVisualizationHandler>();
                    foreach (var grabvisu in grabvisues)
                    {
                        DestroyImmediate(grabvisu);
                    }
                    var grabVisualizationHoverEvent = collider.gameObject.AddComponent(typeof(GrabVisualizationHandler)) as GrabVisualizationHandler;
                    // grabVisualizationHoverEvent. = grabbableObject.GetComponent<Grabbable>();
                }
            }*/
        }
    }
}