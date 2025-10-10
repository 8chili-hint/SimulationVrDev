using UnityEngine;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.Utility.Legacy
{
    public class ImageValueRemapper : MonoBehaviour
    {
        public Image image;
        public Remapper remapper;
        public float AlphaValue { get; set; }
        private Color newColor;
        private void Start()
        {
            remapper.onRemap.AddListener((g)=> AlphaValue = g);
        
            newColor = image.color;
            newColor.a = 0.5f;
        }

        public void UpdateValue(float value)
        {
            remapper.UpdateValue(value);
        
            image.color = Color.Lerp(image.color, newColor, AlphaValue);
   
        }
    }
}
