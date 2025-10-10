using System;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.Utility.APIs;
using UnityEngine;
namespace SimulationSystem.V0._1.Assessment
{
    public static class AssessmentMessageGenerator
    {
        static List<SimulationState> simulationState = new List<SimulationState>();

        public static void GenerateAssessmentSheet(int id)
        {
            var assessmentSheet = new AssessmentSheet
            {
                steps = new List<AssessmentStep>()
            };

            foreach (var state in SimulationManager.instance.simulationStates)
            {
                var step = new AssessmentStep
                {
                    name = state.textPrompt,
                    score = state.maxScore
                };

                SimulationManager.instance.totalScore += step.score;
                assessmentSheet.mode = SimulationManager.instance.isAssessmentMode ? "assessment" : "guided";
                Debug.Log("assessment mode " + SimulationManager.instance.isAssessmentMode);
                assessmentSheet.start_ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                assessmentSheet.steps.Add(step);
            }

            var json = JsonUtility.ToJson(assessmentSheet);
            Debug.Log(json);
            if (SimulationManager.instance.Scoretext != null) { SimulationManager.instance.Scoretext.text = "Score: 0/" + SimulationManager.instance.totalScore; } 
            //  var url = "https://hint.8chili.com/api/v1.0/consim/simulations/4/assessment/start";
         APIManager.Instance.StartCoroutine(APIManager.Instance.POSTRequest(json, ApiUrlManager.SimulationStart(APIManager.Instance.simulationID), null));
        }
        public static void GenerateAssessmentStepResult(int stepIndex, float stepScore, AssessmentStatus assessmentStatus, string errorMessage)
        {
            var assessmentResult = new AssessmentResult
            {
                id = stepIndex + 1,
                event_ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                score = stepScore,
                status = assessmentStatus,
                error_msg = errorMessage
            };

            SimulationManager.instance.currentScore += assessmentResult.score;

            var json = JsonUtility.ToJson(assessmentResult);
            if (SimulationManager.instance.Scoretext != null)
            {
                SimulationManager.instance.Scoretext.text = "Score: " + SimulationManager.instance.currentScore + "/" + SimulationManager.instance.totalScore;
            }
            // var url = "https://hint.8chili.com/api/v1.0/consim/simulations/4/assessment/step/submit";
            APIManager.Instance.StartCoroutine(APIManager.Instance.POSTRequest(json, ApiUrlManager.AssessmentStepSubmit(APIManager.Instance.simulationID), null));
        }
        public static void UpdateAssessmentSheet()
        {
            foreach (var state in SimulationManager.instance.simulationStates)
            {

                var step = new AssessmentStep
                {
                    name = state.textPrompt,
                    score = state.maxScore
                };
                SimulationManager.instance.totalScore += step.score;

            }
        }
    }
}








