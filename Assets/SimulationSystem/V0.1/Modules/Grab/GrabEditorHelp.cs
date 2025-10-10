#if (UNITY_EDITOR)
using SimulationSystem.V0._1.Modules.Detect;
using UnityEditor;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Grab
{
    public class GrabbableEditorHelper : MonoBehaviour
    {

        public void MakeColliderTrigger()
        {
            Collider[] _objectTransform = Resources.FindObjectsOfTypeAll<Collider>(); 
        
            foreach (var child in _objectTransform)
            {
                if (child.GetType() == typeof(CharacterController))
                {
                    continue;
                }
                else if (child.GetType() == typeof(MeshCollider))
                {
                    child.GetComponent<MeshCollider>().convex = true;
                    child.GetComponent<MeshCollider>().isTrigger = true;
                }
                else
                {
                    child.isTrigger = true;
                }
            }
        }

        public void Help()
        {

            var _objectTransform = GetComponent<DetectAssetHolder>()._objectTransform;
            var green = GetComponent<DetectAssetHolder>().green;
        
            if(_objectTransform.childCount > 1) DestroyImmediate(_objectTransform.GetChild(1).gameObject);
            if(_objectTransform.childCount > 1) DestroyImmediate(_objectTransform.GetChild(1).gameObject);
            if(_objectTransform.childCount > 1) DestroyImmediate(_objectTransform.GetChild(1).gameObject);
            if(_objectTransform.GetChild(0).childCount > 0) DestroyImmediate(_objectTransform.GetChild(0).GetChild(0).gameObject);

            Component[] components = _objectTransform.GetComponents(typeof(Component));
        
            foreach(Component component in components)
            {
                if (component.GetType() != typeof(Transform))
                {
                    DestroyImmediate(component);
                }
            }

            var list = _objectTransform.GetComponentsInChildren<Renderer>();
            foreach (var var in list)
            {
                var.material = green;
            }
        }

        public void FindCollider()
        {
            var holder = GetComponent<DetectAssetHolder>();

            Collider[] components = Resources.FindObjectsOfTypeAll<Collider>();

            foreach (var colliderX in components)
            {
                if(!colliderX.isTrigger) holder.colliders.Add(colliderX.gameObject);
            }
        }
    }

    [CustomEditor(typeof(GrabbableEditorHelper))]
    class GrabEditorHelpEditor : Editor 
    {
        GrabbableEditorHelper _target;

        void OnEnable()
        {
            _target = (GrabbableEditorHelper)target; 
        }
    
        public override void OnInspectorGUI() {
            if(GUILayout.Button("Green Material"))
                _target.Help();
        
            if(GUILayout.Button("Collider"))
                _target.MakeColliderTrigger();
        
            if(GUILayout.Button("Find Non Trigger"))
                _target.FindCollider();
        }  
    }
}
#endif