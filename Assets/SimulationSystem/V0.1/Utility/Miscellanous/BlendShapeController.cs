using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class BlendShapeController : MonoBehaviour
    {
        int blendShapeCount;
        SkinnedMeshRenderer skinnedMeshRenderer;
        Mesh skinnedMesh;

        void Awake ()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
            skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        void Start ()
        {
            blendShapeCount = skinnedMesh.blendShapeCount; 
        }

        public void UpdateBlendWeightOnDetect(float value)
        {
            if (blendShapeCount > 0)
                skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, value));
        }
    }
}
