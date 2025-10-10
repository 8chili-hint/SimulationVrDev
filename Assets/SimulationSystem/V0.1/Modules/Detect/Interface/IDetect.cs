using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect.Interface
{
    public interface IDetect
    {
        public void Initialize();
        public void UpdateLoop();

        public void OnDetectionStart(Collider other);
        public void OnDetecting(Collider other);
        public void OnDetectionEnd(Collider other);
        
    }
}
