using SimulationSystem.V0._1.Manager;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect.Utility
{
    public class DetectAudioHandler : MonoBehaviour
    {
        public void PlayDetectEffect()
        {
            GameManager.Instance.AudioManager.PlayDetectEffect();
        }
    }
}
