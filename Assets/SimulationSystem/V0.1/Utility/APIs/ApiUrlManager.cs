namespace SimulationSystem.V0._1.Utility.APIs
{
    public class ApiUrlManager
    {
        public static string baseURL;
        public static string LoginUrl(string pin)
        {
            return baseURL + "/ca/vr_login?pin=" + pin;
        }
        public static string SimulationStart(string simulationID)
        {
            return baseURL + "/consim/simulations/" + simulationID + "/start";
        }
        public static string AssessmentStepSubmit(string simulationID)
        {
            return baseURL + "/consim/simulations/" + simulationID + "/assessment/step/submit";
        }
        public static string SimulationEnd(string simulationID)
        {
            return baseURL + "/consim/simulations/" + simulationID + "/end";
        }
    }
}