using TMPro;
using UnityEngine;

namespace SimulationSystem.V0._1.UI
{
    public class UITextHandler : MonoBehaviour ,IUIDetect
    {
        public TMP_Text text;

        public void UpdateText(string text)
        {
            this.text.text = text;
        }
    
        private void UpdateDetectText(float percentage)
        {
            text.text = $"Total Detect {Mathf.Floor(Mathf.Lerp(0, 100, percentage))} %.";
        }
    
        public void UpdatePercentage(float percentage)
        {
            text.text = $"{Mathf.Floor(Mathf.Lerp(0, 100, 1 - percentage))} %";
        }
    
        public void UpdateInvertedPercentage(float percentage)
        {
            text.text = $"{Mathf.Floor(Mathf.Lerp(0, 100, percentage))} %";
        }
    
        public void UpdateAngle(double angle)
        {
            text.text = $"{angle}°";
        }

        public void OnDetectOnce() => UpdateText("");

        public void OnDetecting() { }

        public void OnDetecting(float value) => UpdateDetectText(value);

        public void OnDetecting(string text) => UpdateText(text);
        public void OnDetectingFinished() { }

        public void OnUnDetected()
        {
            text.text = "";
        }
    }
}
