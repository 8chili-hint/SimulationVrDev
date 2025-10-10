using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class GetSiblingMaterial : MonoBehaviour
    {
        [SerializeField] private MeshRenderer thisMesh;
        [SerializeField] private MeshRenderer siblingMesh;
        [SerializeField] private Color oColor;

        private bool _isFirstDetect = false;

        public void OnDetecting()
        {
            if (!_isFirstDetect)
            {
                var newMat = new Material(thisMesh.sharedMaterial);
                thisMesh.sharedMaterial = newMat;
                _isFirstDetect = true;
            }

            thisMesh.sharedMaterial.color = siblingMesh.sharedMaterial.color;
        }

        public void ResetDetect()
        {
            thisMesh.sharedMaterial.color = oColor;
        }
    }
}
