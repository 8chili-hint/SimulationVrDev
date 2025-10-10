using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.Utility.Event;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Assessment.Utility
{
    public class AssessmentStateEvents : MonoBehaviour
    {
        [Header("[Events]")] 
        [Space]
        
        public UnityEvent onStateStartAssessment = new UnityEvent();
        public UnityEvent onStateCompleteAssessment = new UnityEvent();
        public List<DelayedEvent> onStateStartDelayedEventsAssessment;

        #region Monobehaviour

        private void Start() 
        {
            if (TryGetComponent<SimulationState>(out SimulationState state))
            {
                state.onStateStart.AddListener(()=>{
                    if(SimulationManager.instance.isAssessmentMode) onStateStartAssessment.Invoke();
                });

                state.onStateComplete.AddListener(()=>{
                    if(SimulationManager.instance.isAssessmentMode)onStateCompleteAssessment.Invoke();
                });
                
                onStateStartAssessment.AddListener(() =>
                {
                    if(SimulationManager.instance.isAssessmentMode)
                    {
                        foreach (var delayedEvent in onStateStartDelayedEventsAssessment)
                        {
                            delayedEvent.InvokeDelayedEvent();
                        }
                    }
                }); 
            }
        }

        #endregion
    }
}
