using SimulationSystem.V0._1.Assessment.Assessment_Types;
using SimulationSystem.V0._1.Assessment.Interface;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using SimulationSystem.V0._1.Simulation;

namespace SimulationSystem.V0._1.Assessment
{
    public class AssessmentController: MonoBehaviour, IAssessment
    {
        [Header("Assessment Controller")]
        public float MaxScore;
        [Space(10f)]
        [Header("Select Type of Assessment")]
        
        public bool useGrabAssessment;
        public bool useDetectAssessment;
        public bool useWrongHandAssessment; 
        public bool useUIAssessment;
        public bool turnOffDetectVizThisStep = true;
        
        [Space(20f)]
        [Header("Default/Hint Assessment")]
      

        public string assessmentText;
        [field: SerializeField] public float NegativeMarks {  get; set; }
        [field: SerializeField] public AssessmentStatus AssessmentResultStatus {  get; set;  }
        public string ErrorMessage { get; set; }
        [Space(2f)]
        public UnityEvent OnHintTaken;

        private void Awake()
        {
            GetComponent<SimulationState>().maxScore = MaxScore;
            if (assessmentText != "")
            {
                ErrorMessage = assessmentText;
            }
            else ErrorMessage = "Hint was taken";
        }
    
    }
}