using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Legacy
{
    public class MaterialColorLerper : MonoBehaviour
    {
        public Material material;
        private Color startColor;
        private Color endColor;

        void Start()
        {
            // Store the start time and color values.
            startColor = material.color;
            endColor = new Color(startColor.r, startColor.g, startColor.b, 255.0f);
        }

        void LerpAlpha(float value)
        {
            float t = value;

            // Clamp the value to ensure it doesn't exceed 1.
            t = Mathf.Clamp01(t);

            // Lerp the alpha value of the color.
            Color lerpedColor = Color.Lerp(startColor, endColor, t);

            // Update the material's color property.
            material.color = lerpedColor;
        }
    }
}
