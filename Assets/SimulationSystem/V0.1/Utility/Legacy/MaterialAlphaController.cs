using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Legacy
{
    public class MaterialAlphaController : MonoBehaviour
    {
        public Renderer mesh;

        private bool _isFirst = true;
        private Color _currentColor;
        private Material _material;

        private void Start()
        {
            //var newMat = new Material(_material);
            //mesh.sharedMaterials[1] = newMat;
            _currentColor = mesh.materials[1].color;
        }

        public void UpdateAlpha(float newAlphaValue)
        {
            _currentColor.a = newAlphaValue;
            mesh.materials[1].color = _currentColor;
        }
    }
}
