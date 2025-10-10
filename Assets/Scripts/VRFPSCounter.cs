using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VRFPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Assign a Text UI element in the Inspector

    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;
        float msec = deltaTime * 1000.0f;

        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }
}
