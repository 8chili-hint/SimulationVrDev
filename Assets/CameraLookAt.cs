using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform LookAt;

    private void Update()
    {
     transform.LookAt(transform);
    }
}
