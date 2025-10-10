
using SimulationSystem.V0._1.Assessment.Assessment_Types;
using SimulationSystem.V0._1.Assessment.Utility;
using SimulationSystem.V0._1.Modules.Grab.Utility;
using SimulationSystem.V0._1.Simulation.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

using static SimulationSystem.V0._1.Simulation.SimulationState;

namespace SimulationSystem.V0._1.Simulation
{
    // Handles State Grabbables
    public partial class SimulationState
    {
       
        #region Configure grabbable

        public void ConfigureGrabbable()
        {
            onStateComplete.AddListener(() => EnableResetOnRelease());
            foreach (var stateGrabbable in stateGrabbables)
            {
                if (SimulationStateGrabbableManager.grabbableComponents.ContainsKey(stateGrabbable)) continue;

                var grabbableComponent = new GrabbableSubComponents();

                grabbableComponent.xrGrabInteractable.Add(stateGrabbable);

                var grabVisualisationList = stateGrabbable.transform.GetComponentsInChildren<GrabVisualizationHandler>(true);
                grabbableComponent.grabVisualizationHoverEvents.AddRange(grabVisualisationList);
                
                foreach (var grabVisualisation in grabVisualisationList)
                {
                    //dont need outline as of now
                  /*  grabVisualisation.AddOutlineComponents();*/
                }

                var ghostVisualisationParent = stateGrabbable.transform.Find("Grab Visualisation");
                foreach (Transform child in ghostVisualisationParent)
                {
                    grabbableComponent.ghostVisualisations.Add(child.gameObject);
                }

                var pointableUnityEventWrapper = stateGrabbable.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

                pointableUnityEventWrapper.selectEntered.AddListener(arg0 =>
                {
                    foreach (var visualisation in grabbableComponent.ghostVisualisations)
                    {
                        visualisation.gameObject.SetActive(false);
                    }

                    grabbableComponent.isGrabbed = true;

                    if (SimulationManager.instance.currentState.GetComponent<GrabAssessment>()) GrabAssessmentManager.CheckForGrabError(stateGrabbable);
                });

                pointableUnityEventWrapper.selectExited.AddListener(arg0 =>
                {
                    grabbableComponent.isGrabbed = false;
                });

                SimulationStateGrabbableManager.grabbableComponents.Add(stateGrabbable, grabbableComponent);

                if (!SimulationManager.instance.isAssessmentMode) SimulationStateGrabbableManager.DisableGrabbable(stateGrabbable);
            }
        }

        #endregion

        #region Enable Grabbable Functions

        private void EnableStateGrabbables(bool shouldEnable)
        {


            if (stateType == StateType.Grab)
            {
                onStateComplete.AddListener(() =>
                {
                    foreach (UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable atmep in stateGrabbables)
                    {
                        atmep.firstSelectEntered.RemoveListener(arg0 => { CheckForNextStateOnGrab(); });
                    }
                });
            }

            foreach (var stateGrabbable in stateGrabbables)
            {
                if (!shouldEnable)
                {
                    if (SimulationManager.instance.currentStateIndex == SimulationManager.instance.simulationStates.Count - 1) continue;

                    var nextState = SimulationManager.instance.simulationStates[SimulationManager.instance.currentStateIndex + 1];
                    var isGrabbableInNextState = nextState.stateGrabbables.Contains(stateGrabbable);

                    if (!isGrabbableInNextState)
                    {
                        SimulationStateGrabbableManager.DisableGrabbable(stateGrabbable);
                    }

                    stateGrabbable.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(arg0 => { CheckForNextStateOnGrab(); });
                }
                else
                {
                    if (SimulationManager.instance.currentStateIndex > 0)
                    {
                        var previousState = SimulationManager.instance.simulationStates[SimulationManager.instance.currentStateIndex - 1];

                        var isGrabbableInPreviousState = previousState.stateGrabbables.Contains(stateGrabbable);

                        if (!isGrabbableInPreviousState) SimulationStateGrabbableManager.EnableGrabbable(stateGrabbable, "SimStateGrabbable,Line96");

                        stateGrabbable.firstSelectEntered.AddListener(arg0 => { CheckForNextStateOnGrab(); });
                    }
                    else if (SimulationManager.instance.currentStateIndex == 0)
                    {
                        SimulationStateGrabbableManager.EnableGrabbable(stateGrabbable, "SimStateGrabbable,Line96");

                        stateGrabbable.firstSelectEntered.AddListener(arg0 => { CheckForNextStateOnGrab(); });
                    }
                }
            }
        }


        private void CheckForNextStateOnGrab()
        {
            if (nextStateOnObjectGrab)
            {
                foreach (UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable atmep in stateGrabbables)                   
                {
                    atmep.firstSelectEntered.RemoveListener(arg0 => { CheckForNextStateOnGrab(); });
                    if (CanGoToNextStateOnGrab)
                    {
                        SimulationManager.instance.NextState();
                        CanGoToNextStateOnGrab = false;

                        Debug.Log("Grab next state called");
                    }
                }
            }
        }
        #endregion


        public void EnableResetOnRelease()
        {
            if (SimulationManager.instance.GrabbableShouldNotResetInItsRespectiveStep)
            {
                foreach (var StateGrabbable in stateGrabbables)
                {
                    StateGrabbable.GetComponent<ObjectMovementHelper>().ResetThisObjectOnRelease = true;
                }
            }
        }

        public void EnableGrabbable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable)
        {
            SimulationStateGrabbableManager.EnableGrabbable(grabbable, "EnableGrabbableLine140,SimState_Grabbable");
        }

        public void DisableGrabbable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable)
        {
            SimulationStateGrabbableManager.ForceDisableGrabbable(grabbable);
        }

        public void setGrabbableHelperEvents()
        {
            foreach (var GrabbableHelper in GrabbableHelper)
            {

                if (GrabbableHelper.SelectBehaviour == GrabbableHelperStateBehaviourContainer.Enable)
                {

                    if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.OnStateStart)
                    {
                        onStateStart.AddListener
                        (() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                EnableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    EnableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                    else if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.OnstateEnd)
                    {
                        onStateComplete.AddListener
                        (() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                EnableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    EnableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                    else if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.Both)
                    {
                        onStateStart.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                EnableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    EnableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );

                        onStateComplete.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                EnableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    EnableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                }
                else if (GrabbableHelper.SelectBehaviour == GrabbableHelperStateBehaviourContainer.Disable)
                {
                    if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.OnStateStart)
                    {
                        onStateStart.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                DisableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    DisableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                    else if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.OnstateEnd)
                    {
                        onStateComplete.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                DisableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    DisableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                    else if (GrabbableHelper.SelectEventTrigger == GrabbableHelperStateEventsContainer.Both)
                    {
                        onStateStart.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                DisableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    DisableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );

                        onStateComplete.AddListener(() =>
                        {
                            if (GrabbableHelper.ApplyInAssessmentModeToo)
                                DisableGrabbable(GrabbableHelper.Grabbable);
                            else
                            {
                                if (!SimulationManager.instance.isAssessmentMode)
                                    DisableGrabbable(GrabbableHelper.Grabbable);
                                else return;
                            }
                        }
                        );
                    }
                }
            }
        }
        public bool IsGrabbableUsedInNextState(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable a)
        {
            if (stateGrabbables.Contains(a))
                return true;

            else return false;
        }

        public void OnGrabbedHintDisable()
        {
            foreach (var grabbable in stateGrabbables)
            {
                grabbable.selectEntered.AddListener((arg0) =>
                {
                    if (SimulationManager.instance.currentState.stateType == StateType.Grab)
                    {
                        DisableInteraction(false);
                    }
                });
                grabbable.selectEntered.RemoveListener((arg0) =>
                {
                    DisableInteraction(false);
                });
            }
        }

        private void DisableInteraction(bool shouldDisable)
        {
            SimulationManager.instance._hintButtonComponent.interactable = shouldDisable;
        }
    }


    
}