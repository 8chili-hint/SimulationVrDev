#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Simulation;
using UnityEditor;
using UnityEngine;


namespace SimulationSystem.V0._1.Modules.Grab.Utility
{
    public class findunusedGrabbales:MonoBehaviour
    {

        public List<SimulationState> AllStatesHere;
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> StateGrabbables;
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> UnusedGrabbables;
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> UsedGrabbables;
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> AllGrabbables;
        public List<StateWiseGrabbables> StateWiseGrabbables = new List<StateWiseGrabbables>();

   
 

        [ContextMenu("findUnusedGRabbables")]
        public void FindUnusedGrabbbales()
        {

            StateWiseGrabbables.Clear();
            var tempArray= FindObjectsOfType(typeof(SimulationState),true);
            AllStatesHere.Clear();
            foreach (var obj in tempArray)
            {
                AllStatesHere.Add((SimulationState)obj);
            }
        
            StateGrabbables.Clear();
       
            foreach(var state in AllStatesHere)
            {
        
                foreach (var Grabbable in state.stateGrabbables)
                {
                    if(!StateGrabbables.Contains(Grabbable))
                        StateGrabbables.Add(Grabbable);
             
                }
                StateWiseGrabbables stateWiseGrabbables = new StateWiseGrabbables();
                stateWiseGrabbables.SimulationState = state;
                stateWiseGrabbables.Grabbables = state.stateGrabbables;
                StateWiseGrabbables.Add(stateWiseGrabbables);
          
            }

       
        }

        [ContextMenu("selectUnusedGRabbables")]
        public void selectAllUnused()
        {
            var AllGrabbbales = FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(true);
            UnusedGrabbables = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnusedGrabbables.Clear();
            UsedGrabbables = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UsedGrabbables.Clear();
            foreach(var Grabbable in AllGrabbbales)
            {
                bool thisGrabbaleIsused = false;
                foreach(var GrabbableList in StateWiseGrabbables)
                {
                    if (GrabbableList.Grabbables.Contains(Grabbable))
                    {
                        thisGrabbaleIsused = true;
                        break;

                    }
                    else continue;
                }
                if (thisGrabbaleIsused)
                {
                    UsedGrabbables.Add(Grabbable);
                }
                else
                {
                    UnusedGrabbables.Add(Grabbable);
                }
                AllGrabbables.Add(Grabbable);
/*
            if (!StateWiseGrabbables.Contains(Grabbable))
            {
                UnusedGrabbables.Add(Grabbable);
            }
            else
            {
                UsedGrabbables.Add(Grabbable);
            }*/
            }

            if(UnusedGrabbables.Count > 0)
            {
                var tempGameObjects = new List<GameObject>();
                foreach(var tempUnused in UnusedGrabbables)
                {
                    tempGameObjects.Add(tempUnused.gameObject);
                }

                Selection.objects = tempGameObjects.ToArray();
            }
       
        }
    }


    [Serializable]
    public struct StateWiseGrabbables
    {
        //key
        public SimulationState SimulationState;

        //value
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> Grabbables;
    }

}
#endif