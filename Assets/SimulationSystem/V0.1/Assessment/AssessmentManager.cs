using System;
using SimulationSystem.V0._1.Assessment.Assessment_Types;
using SimulationSystem.V0._1.Assessment.Interface;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine;
namespace SimulationSystem.V0._1.Assessment
{

    [Serializable]
    public enum AssessmentType
    {
        Hint,
        Grab,
        WrongHand,
        UI,
        Detect
    }

    public static class AssessmentManager
    {
        public static void DeductScore(AssessmentType type)
        {
            IAssessment assessment = null;
            var currentState = SimulationManager.instance.currentState;
            currentState.AssessmentStatePromptDisabe();
            switch (type)
            {
                case AssessmentType.Hint:
                    assessment = currentState.GetComponent<AssessmentController>();
                    SimulationManager.instance.currentState.GetComponent<AssessmentController>().OnHintTaken?.Invoke();
                    SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.hintAssessmentPrompt, SimulationStatePromptManager.DisplayPrompts, true);
                    if (currentState.GetComponent<AssessmentController>().assessmentText != "")
                    {
                        SimulationStatePromptManager.promptText.text = currentState.GetComponent<AssessmentController>().assessmentText;
                    }
                    else SimulationStatePromptManager.promptText.text = "Hint was taken.";
                    foreach(var objectToDetect in currentState.objectToDetectList)
                    {
                        objectToDetect.detectObject.hintIsTaken = true;
                    }

                    break;
                case AssessmentType.Grab:
                    assessment = currentState.GetComponent<GrabAssessment>();
                    SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.grabsAssessmentPrompt, SimulationStatePromptManager.DisplayPrompts, true);
                    if (currentState.GetComponent<GrabAssessment>().thisAssessmentText != "")
                    {
                        SimulationStatePromptManager.promptText.text = currentState.GetComponent<GrabAssessment>().thisAssessmentText;
                    }
                    else SimulationStatePromptManager.promptText.text = "Wrong object was grabbed.";
                    break;
                case AssessmentType.WrongHand:
                    assessment = currentState.GetComponent<WrongHandAssessment>();
                    SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.wrongHandAssessmentPrompt, SimulationStatePromptManager.DisplayPrompts, true);
                    SimulationStatePromptManager.promptText.text = "Wrong hand was used.";
                    break;
                case AssessmentType.UI:
                    assessment = currentState.GetComponent<UIAssessment>();
                    SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.UIAssessmentPrompt, SimulationStatePromptManager.DisplayPrompts, true);
                    if (currentState.GetComponent<UIAssessment>().thisAssessmentText != "")
                    {
                        SimulationStatePromptManager.promptText.text = currentState.GetComponent<UIAssessment>().thisAssessmentText;
                    }
                    else SimulationStatePromptManager.promptText.text = "Wrong option was selected.";
                    break;
                case AssessmentType.Detect:
                    assessment = currentState.GetComponent<DetectAssessment>();
                    SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.detectAssessmentPrompt, SimulationStatePromptManager.DisplayPrompts, true);
                    if (currentState.GetComponent<DetectAssessment>().thisAssessmentText != "")
                    {
                        SimulationStatePromptManager.promptText.text = currentState.GetComponent<DetectAssessment>().thisAssessmentText;
                    }
                    else SimulationStatePromptManager.promptText.text = "Wrong object was detected.";
                    //turn on DetectViz
                    break;
            }

            if (currentState.GetComponent<AssessmentController>().turnOffDetectVizThisStep)
            {
                foreach (var objectToDetect in currentState.objectToDetectList)
                {
                    objectToDetect.detectObject.SwitchDetectHintVisuals(true);
                }
            }
            if (assessment == null || currentState.assessmentStatus != AssessmentStatus.Success) return;
            if (assessment.AssessmentResultStatus == AssessmentStatus.Failure)
            {
                SimulationStatePromptManager.PlayAudioPrompt(SimulationManager.instance.failureAssessmentPrompt, SimulationManager.instance.RestartScene, true);
                SimulationStatePromptManager.promptText.text = "You have failed the simulation.";
                //SimulationManager.instance.currentStateIndex;
                int _tempIndex = SimulationManager.instance.BufferedStates.IndexOf(SimulationManager.instance.currentState);
                AssessmentMessageGenerator.GenerateAssessmentStepResult(_tempIndex, 0, AssessmentStatus.Failure, "Failure");
                SimulationStatePromptManager.isFailure = true;
            }

            // var currentScore = Mathf.Clamp(currentState.currentScore - assessment.NegativeMarks, 0, currentState.currentScore);
            var currentScore = currentState.maxScore - assessment.NegativeMarks;
            currentState.currentScore = currentScore;
            currentState.assessmentStatus = assessment.AssessmentResultStatus;
            if (!string.IsNullOrEmpty(currentState.errorMessage)) currentState.errorMessage += "";
            currentState.errorMessage += assessment.ErrorMessage;
        }
    }
}









