using UnityEngine;

namespace SimulationSystem.V0._1.UI
{
    public class UIReseter : MonoBehaviour
    {
        [SerializeField] private UIAnimationHandler animHandler;
        //[SerializeField] private UIImageHandler imageHandler;
        //[SerializeField] private UIHintHandler hintHandler;
        [SerializeField] private UITextHandler textHandler;

        public void OnUIReset()
        {
            animHandler.OnUnDetected();
            textHandler.OnUnDetected();
        }
    }
}
