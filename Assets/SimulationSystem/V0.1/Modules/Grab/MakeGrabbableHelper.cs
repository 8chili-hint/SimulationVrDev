#if UNITY_EDITOR
using System.Linq;
using SimulationSystem.V0._1.Modules.Grab.Utility;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Utility.Event;
using SimulationSystem.V0._1.Utility.Miscellanous;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Grab
{
    public class MakeGrabbableHelper : MonoBehaviour
    {
        /* public string NameOfGrabbableObject;

         [Header("Use Occulus Default Ghost Prefabs")]
         public GameObject AnimatedGhostHandPrefab;

         [Header("Use World Space Canvas prefab")]
         public GameObject CanvasPrefab;

         [Header("Make sure this Mesh has HandInteractables in Children")]
         public MeshRenderer GrabbableMesh;



         private MakeGrabbableHelper grababbleHelper;

         HandGrabInteractable[] interactables;

         public void MakeThisGrabbable()
         {
             grababbleHelper = GetComponent<MakeGrabbableHelper>();
             GameObject rootParent = new GameObject(grababbleHelper.NameOfGrabbableObject);

             var Rb = rootParent.AddComponent<Rigidbody>();
             Rb.isKinematic = true;
             Rb.useGravity = false;

             //Adding Grabbable, Pointable unity event wrapper, Ressetable item, Grab Viz Handler, Object name Label handler, Move To helper, OneGrabTransformTranslate
             //Cached for later use(will get disposed after each iteration)
             var GrabbableCachedObj = rootParent.AddComponent<Grabbable>();
             var PointableCachedObj = rootParent.AddComponent<PointableUnityEventWrapper>();
             var objectnameHandler = rootParent.AddComponent<ObjectNameLabelHandler>();


             PointableCachedObj.InjectPointable(GrabbableCachedObj);
             rootParent.AddComponent<GrabVisualizationHandler>();
             rootParent.AddComponent<ObjectMovementHelper>();



             GameObject visuals_obj = new GameObject("Visuals");
             GameObject grabvis_obj = new GameObject("Grab visualisation");
             GameObject grabInt_obj = new GameObject("Grab Interactables");
             GameObject Ui_obj = new GameObject("UI Label");

             //making all the gameObjects a child of root object
             visuals_obj.transform.SetParent(rootParent.transform);
             grabvis_obj.transform.SetParent(rootParent.transform);
             grabInt_obj.transform.SetParent(rootParent.transform);
             Ui_obj.transform.SetParent(rootParent.transform);

             //making sure it is in correct order
             visuals_obj.transform.SetSiblingIndex(0);
             grabvis_obj.transform.SetSiblingIndex(1);
             grabInt_obj.transform.SetSiblingIndex(2);
             Ui_obj.transform.SetSiblingIndex(3);

             //For Setting up components in Visuals Object
             GameObject colliderObj = new GameObject("Collider");
             colliderObj.AddComponent<TriggerUnityEventWrapper>();
             BoxCollider meshColl = colliderObj.AddComponent<BoxCollider>();
             meshColl.isTrigger = true;
             GameObject mainMesh = grababbleHelper.GrabbableMesh.GameObject();
             meshColl.size = mainMesh.transform.localScale;

             //transferring position from Mesh Obj to Root Parent
             rootParent.transform.SetPositionAndRotation(grababbleHelper.GrabbableMesh.GameObject().transform.position, Quaternion.identity);
             rootParent.transform.rotation = grababbleHelper.GrabbableMesh.GameObject().transform.rotation;


             mainMesh.TryGetComponent<Rigidbody>(out var rbMain);
             {
                 if (rbMain)
                 {
                     DestroyImmediate(rbMain);
                 }
             }
             mainMesh.TryGetComponent<Collider>(out var Coll);
             {
                 *//* var bounds = Coll.bounds;*//*
                 // meshColl.contactOffset = bounds.extents.x * 2;
                 if (Coll)
                 {
                     DestroyImmediate(Coll);
                 }
             }

             //make both Mesh and collider a child of visuals obj
             colliderObj.transform.SetParent(visuals_obj.transform);
             *//*the line below is needed otherwise, it will take some amount of offset*//*
             colliderObj.transform.localPosition = Vector3.zero;

             mainMesh.transform.SetParent(visuals_obj.transform);

             mainMesh.transform.rotation = Quaternion.identity;
             mainMesh.transform.localRotation = Quaternion.identity;

             mainMesh.transform.position = Vector3.zero;
             mainMesh.transform.localPosition = Vector3.zero;
             //making sure the index is correct
             colliderObj.transform.SetAsFirstSibling();
             mainMesh.transform.SetAsLastSibling();

             //Setting up GrabVisualization
             GameObject ghostHandObj = new GameObject("Ghost Hand");
             GameObject animatedghostHandObj = Instantiate(grababbleHelper.AnimatedGhostHandPrefab);
             animatedghostHandObj.transform.SetParent(ghostHandObj.transform);
             ghostHandObj.transform.SetParent(grabvis_obj.transform);
             ghostHandObj.SetActive(false);

             //setting up GrabInteractables
             interactables = mainMesh.GetComponentsInChildren<HandGrabInteractable>();
             int tempCount = interactables.Count();
             GameObject[] tempGoArray = new GameObject[tempCount];

             for (int i = 0; i < tempCount; i++)
             {
                 tempGoArray[i] = interactables[i].GameObject();
             }

             foreach (var interactable in tempGoArray)
             {
                 // var Pos = interactable.transform.localPosition;
                 GameObject handGrabInteractable = interactable.GameObject();
                 *//*handGrabInteractable.name = "HandGrabInteractable";*//*

                 handGrabInteractable.transform.SetParent(grabInt_obj.transform);
                 handGrabInteractable.transform.localPosition = Vector3.zero;
                 handGrabInteractable.transform.localScale = Vector3.one;
                 // handGrabInteractable.transform.GetChild(0).transform.localPosition = (Vector3.zero);
             }

             //setting up UIlabel
             objectnameHandler.grabbableObjectLabel = Ui_obj.AddComponent<UIAnimationHandler>();

             GameObject ParentCanvas = (GameObject)PrefabUtility.InstantiatePrefab(CanvasPrefab);
             ParentCanvas.transform.SetParent(Ui_obj.transform);
             ParentCanvas.transform.localPosition = Vector3.zero;

             //replace later
             ParentCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = grababbleHelper.NameOfGrabbableObject;


         }

     }



     [CustomEditor(typeof(MakeGrabbableHelper))]
     class MakeGrabbableEditor : Editor
     {
         MakeGrabbableHelper _target;

         void OnEnable()
         {
             _target = (MakeGrabbableHelper)target;
         }

         public override void OnInspectorGUI()
         {
             if (GUILayout.Button("Make Grabbable"))
                 _target.MakeThisGrabbable();

             base.OnInspectorGUI();
         }
     }*/
    }
}
#endif
