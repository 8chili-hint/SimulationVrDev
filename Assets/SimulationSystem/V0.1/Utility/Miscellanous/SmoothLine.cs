using System.Collections.Generic;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class SmoothLine : MonoBehaviour
    {
        public bool shouldUpdate;

        public void UpdateLineBool(bool setBool)
        {
            shouldUpdate = setBool;
        }
        private void Start()
        {
            SetPoints();
        }

        private void Update()
        {
            if (shouldUpdate)
            {
                SetPoints();
            }
        }

        private void SetPoints()
        {
            List<Vector3> points = new List<Vector3>();

            foreach (Transform child in transform)
            {
                points.Add(child.position);
            }

            var linepoints = LineSmoother.SmoothLine(points.ToArray(), 0.01f);

            LineRenderer renderer = GetComponent<LineRenderer>();
            renderer.positionCount = linepoints.Count;
            for (int i = 0; i < linepoints.Count; i++)
            {
                renderer.SetPosition(i, linepoints[i]);
            }
        }
    }
}