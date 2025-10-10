using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class FadeInOut : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeTime = 1.0f;

        public Coroutine FadeOut()
        {
            return StartCoroutine(Fade(fadeImage, 1.0f, 0.0f));
        }

        public Coroutine FadeIn()
        {
            return StartCoroutine(Fade(fadeImage, 0.0f, 1.0f));
        }

        private IEnumerator Fade(Image image, float startAlpha, float endAlpha)
        {
            float elapsedTime = 0.0f;
            Color startColor = image.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeTime);
                image.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }
        }
    }
}
