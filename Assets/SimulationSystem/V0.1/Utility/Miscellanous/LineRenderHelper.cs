using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    [ExecuteInEditMode]
    public class LineRenderHelper : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lineRenderer.SetPosition(0, startPoint.position);
            _lineRenderer.SetPosition(1, endPoint.position);
        }
    }
}
