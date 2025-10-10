using UnityEngine;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.UI
{
    public class UIImageHandler : MonoBehaviour , IUIDetect
    {
        public bool isInverted = false;
        public Image loaderImage;
        public Image stepCompleteImage;

        public void OnDetectOnce()
        {
            loaderImage.fillAmount = 0;
        }

        public void OnDetecting()
        {
        
        }
    
        public void OnDetecting(float value)
        {
            if (!isInverted)
                loaderImage.fillAmount = value;
            else
                loaderImage.fillAmount = 1 - value;
        }

        public void OnDetectingFinished()
        {
        
        }

        public void OnUnDetected()
        {
        
        }

        public void ResetDetect()
        {
            loaderImage.fillAmount = 0;
        }
    }
}
