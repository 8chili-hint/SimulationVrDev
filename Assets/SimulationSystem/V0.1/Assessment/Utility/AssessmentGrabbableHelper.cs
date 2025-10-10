using System;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine;


namespace SimulationSystem.V0._1.Assessment.Utility
{
    public class AssessmentGrabbableHelper : MonoBehaviour
    {
        [SerializeField] public List<GrabbableCollection> grabbableCollections;
    
        public void EnableGrabbableCollection(int index)
        {
            if (!SimulationManager.instance.isAssessmentMode) return;
        
            foreach (var grabbable in grabbableCollections[index].grabbableCollection)
            {
                SimulationStateGrabbableManager.EnableGrabbable(grabbable, "AssessmentGrabbableHelper");
            }
        }
    
        public void DisableGrabbableCollection(int index)
        {
            if (!SimulationManager.instance.isAssessmentMode) return;
        
            foreach (var grabbable in grabbableCollections[index].grabbableCollection)
            {
                SimulationStateGrabbableManager.DisableGrabbable(grabbable);
                SimulationStateGrabbableManager.grabbableComponents[grabbable].shouldEnable = false;
            }
        }
    
        public void DisableAllGrabbableCollections()
        {
            if (!SimulationManager.instance.isAssessmentMode) return;

            foreach (var collection in grabbableCollections)
            {
                foreach (var grabbable in collection.grabbableCollection)
                {
                    SimulationStateGrabbableManager.DisableGrabbable(grabbable);
                    SimulationStateGrabbableManager.grabbableComponents[grabbable].shouldEnable = false;
                }
            }
        }

        
    }

    [Serializable]
    public class GrabbableCollection
    {
        [SerializeField] public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> grabbableCollection;
    }
}