using SimulationSystem.V0._1.Assessment.Interface;
using UnityEngine;

namespace SimulationSystem.V0._1.Assessment.Assessment_Types
{
    [RequireComponent(typeof(AssessmentController))]
    public class GrabAssessment : MonoBehaviour, IAssessment
    {
        [field: SerializeField] public float NegativeMarks { get; set; }
        [field: SerializeField] public AssessmentStatus AssessmentResultStatus { get; set; }
        public string ErrorMessage { get; private set; }
        [Space(5f)]
        [Header("Overrides")]
        public string thisAssessmentText;

        private void Awake()
        {
          
            if (thisAssessmentText != "")
            {
                ErrorMessage = thisAssessmentText;
            }
            else ErrorMessage = "Wrong object was grabbed";
        }
    }
}