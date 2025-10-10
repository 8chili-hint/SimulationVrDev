namespace SimulationSystem.V0._1.Assessment.Interface
{
    public interface IAssessment
    {
        public float NegativeMarks { get; set; }
        public AssessmentStatus AssessmentResultStatus { get; set; }
        public string ErrorMessage { get;} 
    }
}