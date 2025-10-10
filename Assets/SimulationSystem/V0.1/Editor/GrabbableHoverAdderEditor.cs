using SimulationSystem.V0._1.Modules.Grab.Utility;
using UnityEditor;
using UnityEngine;

namespace SimulationSystem.V0._1.Editor
{
    [CustomEditor(typeof(GrabbableHoverAdder))]
    public class GrabbableHoverAdderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GrabbableHoverAdder myScript = (GrabbableHoverAdder)target;
        
            if(GUILayout.Button("Build Object"))
            {
                myScript.BuildObject();
            }
        }
    }
}