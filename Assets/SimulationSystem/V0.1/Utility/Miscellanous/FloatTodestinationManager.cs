using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Modules.Grab.Utility;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class FloatTodestinationManager : MonoBehaviour
    {
        private List<FloatToDestination> _floatToDestinations = new List<FloatToDestination>();

        private void Start()
        {
            _floatToDestinations = Resources.FindObjectsOfTypeAll<FloatToDestination>().ToList();
        }

        public void EnableAllFloat()
        {
            _floatToDestinations.ForEach(g => g.enabled = true);
        }
    
        public void DisableAllFloat()
        {
            _floatToDestinations.ForEach(g => g.enabled = false);
        }
    }
}
