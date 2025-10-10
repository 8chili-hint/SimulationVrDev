using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Legacy.Refactoring
{
    public class StateManagerStateOverride : MonoBehaviour
    {
        public List<UnityEvent> onStateOverrideToPreviousList = new List<UnityEvent>();

        public void InvokeOverrideToPrevious()
        {
            OverrideToPreviousState();
        }

        public void OverrideToPreviousState()
        {
            //onStateOverrideToPreviousList.ForEach(g => g?.Invoke());
            SimulationManager.instance.PreviousState();
        }

    }
}
