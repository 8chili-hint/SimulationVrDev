using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
public class MixedRealityPassThroughcontroller : MonoBehaviour
{

    [SerializeField, Tooltip("AR Plane Manager that is in charge of passthrough.")]
    ARCameraManager m_ARCameraManager;
   public UnityEvent<bool> m_OnARPassthroughFeatureChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        TogglePassthrough(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePassthrough(bool enabled)
    {
        if (m_ARCameraManager == null)
            return;

        m_ARCameraManager.enabled = enabled;
        m_OnARPassthroughFeatureChanged?.Invoke(enabled);
    }
}
