
using SimulationSystem.V0._1.Assessment.Interface;
using SimulationSystem.V0._1.Modules.Detect;
using UnityEngine;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;

namespace SimulationSystem.V0._1.Assessment.Assessment_Types
{
    [RequireComponent(typeof(AssessmentController))]
    public class DetectAssessment : MonoBehaviour, IAssessment
    {
        [field: SerializeField] public float NegativeMarks { get; set; }
        [field: SerializeField] public AssessmentStatus AssessmentResultStatus { get; set; }

        [Space(5f)]
        [Header("Overrides")]
        public string thisAssessmentText;
        public string ErrorMessage { get; set; }

        [Header("Wrong detects.")]
        [Tooltip("//The object detected will be the same as the step's state grabable//")]
        public List<DetectObject> WrongDetects = new List<DetectObject>();
        private void Awake()
        {
            if (thisAssessmentText != "")
            {
                ErrorMessage = thisAssessmentText;
            }
            else ErrorMessage = "Wrong object was detected";

            foreach(DetectObject DO in WrongDetects)
            {
                DO.ThisIsWrongDetect = true;
            }
          
        }

        public void AddListeners()
        {
            GetComponent<SimulationState>().onStateStart.AddListener(() =>
            {
                if (SimulationManager.instance.isAssessmentMode) ToggleWrongdetects(true);
            });

            GetComponent<SimulationState>().onStateComplete.AddListener(() =>
            {
                if (SimulationManager.instance.isAssessmentMode) ToggleWrongdetects(false);
            });
        }
        private void OnDestroy()
        {
            removeListeners();
        }
        public void removeListeners()
        {
            GetComponent<SimulationState>().onStateStart.RemoveListener(() =>
            {
                if (SimulationManager.instance.isAssessmentMode) ToggleWrongdetects(true);
            });

            GetComponent<SimulationState>().onStateComplete.RemoveListener(() =>
            {
                if (SimulationManager.instance.isAssessmentMode) ToggleWrongdetects(false);
            });
        }

        private void ToggleWrongdetects(bool ShouldDetect)
        {
            if(!ShouldDetect)
            {
                foreach (var GO in WrongDetects)
                {
                    GO.gameObject.SetActive(false);
                }
            }
            else if (ShouldDetect)
            {
                foreach (var GO in WrongDetects)
                {
                    GO.gameObject.SetActive(true);
                }
            }

        }
    }
}
