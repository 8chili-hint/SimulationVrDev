using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Modules.Detect.Legacy;

namespace SimulationSystem.V0._1.Simulation.Manager
{
    public static class SimulationStateDetectManager
    {
        public static void EnableDetectAbstract(DetectAbstract detect, bool shouldEnable)
        {
            var detectType = detect.GetType();

            ToggleDetectGameObject(detect, shouldEnable);
        
            if (detectType != typeof(SimultaneousMultipleDetectObjects)) return;
        
            var simultaneousMultipleDetectObjectsComponent = detect.GetComponent<SimultaneousMultipleDetectObjects>();

            foreach (var detects in simultaneousMultipleDetectObjectsComponent.detectObjectPerState[SimulationManager.instance.currentStateIndex])
            {
                var detectAbstract = detects.Key.GetComponent<DetectAbstract>();
                ToggleDetectGameObject(detectAbstract, shouldEnable);
            }
        }

        private static void ToggleDetectGameObject(DetectAbstract detect, bool toggleOn)
        {
            if(!toggleOn) detect.DisableDetection();
            else detect.EnableDetection();
        
            detect.gameObject.SetActive(toggleOn);
        }
    }
}