using System;
using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;


public static class SimulationStateAutomation
{
    public static void AutomateState(SimulationState state)
    {
        switch (state.stateType)
        {
            case SimulationState.StateType.DetectWithGrab:
                foreach (var objectToDetect in SimulationManager.instance.currentState.objectToDetectList)
                {
                    if(objectToDetect.shouldMoveToNextState) objectToDetect.detectObject.onDetectionComplete.Invoke();
                }
                break;
            case SimulationState.StateType.DetectWithHand:
                foreach (var objectToDetect in SimulationManager.instance.currentState.objectToDetectList)
                {
                    if (objectToDetect.shouldMoveToNextState) objectToDetect.detectObject.onDetectionComplete.Invoke();
                }
                break;
            case SimulationState.StateType.Prompt:
                SimulationManager.instance.NextState();
                break;
            case SimulationState.StateType.Grab:
                state.stateGrabbables[0].GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.Invoke(default);
                SimulationManager.instance.NextState();
                break;
            case SimulationState.StateType.UI:
                state.UIButtonComponent.onClick.Invoke();
                state.UIStateExit();
                break;
            case SimulationState.StateType.Gaze:
                foreach (var objectToDetect in SimulationManager.instance.currentState.objectToDetectList)
                {
                    if (objectToDetect.shouldMoveToNextState) objectToDetect.detectObject.onDetectionComplete.Invoke();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}