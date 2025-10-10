using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class EncapsulatingCollider : MonoBehaviour
{
    [MenuItem("Tools/MakeEncapsulatingMeshCollider")]
    public static void MakeEncapsulatingMeshCollider()
    {
        // Get the currently selected GameObject
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject in the hierarchy.");
            return;
        }

        // Add a MeshCollider component to the selected GameObject
        MeshCollider meshCollider = selectedObject.AddComponent<MeshCollider>();

        // Fit the MeshCollider to the mesh in the child objects
        MeshFilter[] childFilters = selectedObject.GetComponentsInChildren<MeshFilter>();
        Bounds bounds = new Bounds();

        foreach (MeshFilter childFilter in childFilters)
        {
            bounds.Encapsulate(childFilter.sharedMesh.bounds);
        }

        meshCollider.sharedMesh = new Mesh();
        meshCollider.sharedMesh.CombineMeshes(childFilters.Select(childFilter => new CombineInstance
        {
            mesh = childFilter.sharedMesh,
            transform = selectedObject.transform.worldToLocalMatrix * childFilter.transform.localToWorldMatrix
        }).ToArray());
    }
    
    [MenuItem("Tools/AddEncapsulatingBoxCollider")]
    public static void AddBoxColliderToFitMeshInChild()
    {
        // Get the currently selected GameObject
        GameObject selectedObject = Selection.activeGameObject;
        
        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject in the hierarchy.");
            return;
        }
        
        // Ensure the parent has a BoxCollider
        BoxCollider boxCollider = selectedObject.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = selectedObject.AddComponent<BoxCollider>();
        }

        // Initialize an empty bounds object in local space
        Bounds combinedBounds = new Bounds(Vector3.zero, Vector3.zero);

        // Flag to check if bounds have been initialized
        bool hasInitializedBounds = false;

        // Iterate through all renderers in child objects
        foreach (Renderer renderer in selectedObject.GetComponentsInChildren<Renderer>())
        {
            if (!hasInitializedBounds)
            {
                combinedBounds = renderer.bounds; // Initialize bounds
                hasInitializedBounds = true;
            }
            else
            {
                combinedBounds.Encapsulate(renderer.bounds); // Expand bounds
            }
        }

        // Convert world bounds to local bounds and apply to Box Collider
        boxCollider.center = selectedObject.transform.InverseTransformPoint(combinedBounds.center);
        boxCollider.size = combinedBounds.size;

    }
}
#endif
