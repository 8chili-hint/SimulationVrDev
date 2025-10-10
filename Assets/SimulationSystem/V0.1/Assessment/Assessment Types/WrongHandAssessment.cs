using SimulationSystem.V0._1.Assessment.Interface;
using UnityEngine;

namespace SimulationSystem.V0._1.Assessment.Assessment_Types
{
    [RequireComponent(typeof(AssessmentController))]
    public class WrongHandAssessment : MonoBehaviour, IAssessment
    {
        [field: SerializeField] public float NegativeMarks {  get; set; }
        [field: SerializeField] public AssessmentStatus AssessmentResultStatus {  get; set; }
        public string ErrorMessage { get; set; }
        
        private void Awake()
        {
            ErrorMessage = "Wrong hand was used";
        }
    }
}