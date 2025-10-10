using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class MeshExpender : MonoBehaviour
    {
        public Transform checkTransform;
        public float checkTransformMinHeight = -0.3f;
        public float checkTransformMaxHeight = 0.3f;
    
        public float minHeight;
        public float maxHeight;
        public float minScale;
        public float maxScale;

        private void Update()
        {
            var localPosition = transform.localPosition;
            var position = checkTransform.localPosition;
            localPosition = new Vector3(localPosition.x,
                Mathf.Lerp(minHeight, maxHeight, Mathf.InverseLerp(checkTransformMinHeight, checkTransformMaxHeight, position.y)), localPosition.z);
            transform.localPosition = localPosition;

            var localScale = transform.localScale;
            localScale = new Vector3(localScale.x,
                Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(checkTransformMinHeight, checkTransformMaxHeight, position.y)),
                localScale.z);
            transform.localScale = localScale;
        }
    }
}
