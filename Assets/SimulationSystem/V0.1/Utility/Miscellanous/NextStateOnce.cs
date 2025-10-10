using SimulationSystem.V0._1.Simulation;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class NextStateOnce : MonoBehaviour
    {
        private int count = 1;

        public void CheckNextState()
        {
            if (count == 1)
            {
                SimulationManager.instance.NextState();
                count--;
            }
        }
    }
}