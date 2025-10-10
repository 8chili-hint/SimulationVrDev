using System.Collections.Generic;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect
{
    public class DetectAssetHolder : MonoBehaviour
    {
        public Transform _objectTransform;
        public Material green;
        public List<GameObject> colliders = new List<GameObject>();
    }
}