#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Modules.Detect;
using UnityEditor;
using UnityEngine;


public class MakeVisual : EditorWindow
{
    public Transform objectVisual;
    private Transform visualChild;
    private Transform targetObject;

    [MenuItem("SimulationSystem/MakeVisual")]
    private static void ShowWindow()
    {
        var window = GetWindow<MakeVisual>();
        window.titleContent = new GUIContent("MakeVisual");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.Label("Actual Visual of the object");
        objectVisual = EditorGUILayout.ObjectField(objectVisual, typeof(Transform), true) as Transform;

        GUILayout.Space(20);
        GUILayout.Label("Detect/Grab object");
        targetObject = EditorGUILayout.ObjectField(targetObject, typeof(Transform), true) as Transform;

        GUILayout.Space(20);
        GUILayout.Label("Visual child of grab/detect");
        visualChild = EditorGUILayout.ObjectField(visualChild, typeof(Transform), true) as Transform;

        if (GUILayout.Button("PrepareGrabObject"))
        {
            PrepareGrabObject();
        }

        if (GUILayout.Button("PrepareDetectObject"))
        {
            PrepareDetectObject();
        }
    }


    public void PrepareDetectObject()
    {
        targetObject.position = objectVisual.position;
    }

    public void PrepareGrabObject()
    {
        targetObject.position = objectVisual.position;
        var visualInGrab = targetObject.GetChild(0);
        objectVisual.SetParent(visualInGrab);
        var dummyVisual = visualInGrab.GetComponentInChildren<MeshRenderer>().transform;
        Destroy(dummyVisual);
    }
}

#endif
