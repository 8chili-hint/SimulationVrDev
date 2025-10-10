using SimulationSystem.V0._1.Manager;
using UnityEngine;

namespace SimulationSystem.V0._1.Warnings
{
    public abstract class DetectWarning : MonoBehaviour
    {
        protected bool hasDetected;
        
        [SerializeField] 
        protected string warningText = string.Empty;
        
        public string WarningText
        {
            get => warningText;
            set => warningText = value;
        }
        
        public bool CanCheck { get; set; } = true;

        public DetectStates State { get; protected set; }
        
        protected virtual void UpdateState()
        {
            switch (hasDetected)
            {
                case true when State == DetectStates.Normal:
                    State = DetectStates.Detect;
                    break;
                case false when State == DetectStates.Detect:
                    State = DetectStates.UnDetect;
                    break;
                case true when State == DetectStates.UnDetect:
                case false when State == DetectStates.UnDetect:
                    State = DetectStates.Normal;
                    break;
            }
        }
        
    }
}