using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect.Legacy
{
    [RequireComponent(typeof(Detectable))]
    public class DetectObjectDynamic : DetectObject
    {
        public enum DetectionType
        {
            HandCollider,
            name,
            none
        }
        
        [Header("DynamicColliders")][Space(10)]
        [SerializeField] private bool canRemoveDynamicObject = false;
        public DetectionType type = DetectionType.HandCollider;
        [SerializeField] private List<string> dynamicObjectName = new List<string>();
        [SerializeField] private GameObject dynamicObjectParents;
        
        [Space(10)][Header("Grab Detection")]
        [SerializeField] private float grabTimer;
        [SerializeField] private float grabDetectTimer;
        [SerializeField] private DetectHand detectHand;
        
        public UnityEvent OnDetectWithGrabComplete;
        public UnityEvent<float> OnDetectingWithGrab;
        public UnityEvent<bool> OnDetectWithoutGrab;

        public GameObject wrongHand;
        private string wrongHandName;

        private void Awake()
        {
            if(wrongHand != null) wrongHandName = wrongHand.name + "_CapsuleCollider";
        }

        public void OnObjectEnter(Collider other)
        {
            if (wrongHand != null && other.gameObject.name == wrongHandName)
            {
                AssessmentManager.DeductScore(AssessmentType.WrongHand);
                return;
            }
            
            // base.OnObjectEnter(other);
            // if (type == DetectionType.name)
            // {
            //     if (dynamicObjectName.Any(dynamicObject =>
            //             objectsToDetect.Count < 1 && string.Compare(dynamicObject, other.name) == 0))
            //     {
            //         //! was throwing error
            //         //objectToDetect.Add(other.gameObject); 
            //     }
            // }
        }

        public void OnObjectExit(Collider other)
        {
            // base.OnObjectExit(other);

            // if (canRemoveDynamicObject)
            // {
            //     if (objectsToDetect.Count > 0)
            //     {
            //         if (dynamicObjectName.Any(dynamicObject =>
            //                 objectsToDetect.Count > 0 && string.Compare(dynamicObject, other.name) == 0))
            //         {
            //             //! was throwing error
            //             // objectToDetect.Remove(other.gameObject);
            //         }
            //     }
            // }
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            if (type == DetectionType.HandCollider)
            {
                //Hard Coded
                Transform parent = dynamicObjectParents.transform.GetChild(1);
                foreach (Transform componentsInChild in parent)
                {
                    //! was throwing error
                    // objectToDetect.Add(componentsInChild.gameObject.transform.GetChild(0).gameObject);
                }
            }
        }

        private float previousGrabTimer = 0;
        public void UpdateLoop()
        {
            // base.UpdateLoop();
            //
            // if (detectHand)
            // {
            //     if (canDetect && isDetected && detectHand.IsGrabbedWithAssignedHand)
            //     {
            //         grabTimer += Time.deltaTime;
            //         if (grabTimer >= grabDetectTimer && objectsToDetect.Count > 0)
            //         {
            //             OnDetectWithGrabComplete?.Invoke();
            //             previousGrabTimer = grabTimer;
            //         }
            //         else if (grabTimer < grabDetectTimer)
            //         {
            //             if (Mathf.InverseLerp(0, grabDetectTimer, grabTimer) < 1)
            //                 OnDetectingWithGrab?.Invoke(Mathf.InverseLerp(0, grabDetectTimer, grabTimer));
            //         }
            //
            //         OnDetectWithoutGrab?.Invoke(false);
            //     }
            //
            //     if (canDetect && isDetected && detectHand.IsGrabbedWithAssignedHand)
            //     {
            //         OnDetectWithoutGrab?.Invoke(true);
            //     }
            // }
        }
    }
    
    //Editor script
    #if UNITY_EDITOR
    [CustomEditor((typeof(DetectObjectDynamic)),true)]
    public class DetectDynamicObjectsEditor : Editor
    {
        
        SerializedProperty Type;
        SerializedProperty DynamicObjectParents;
    
        void OnEnable()
        {
            Type = serializedObject.FindProperty("type");
            DynamicObjectParents = serializedObject.FindProperty("dynamicObjectParents");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            DetectObjectDynamic objectDynamic = (DetectObjectDynamic)target;
            
            objectDynamic.type = (DetectObjectDynamic.DetectionType)EditorGUILayout.EnumPopup("Type", objectDynamic.type);
            
            // Serialize the field only if the enum value matches a specific value
            if (objectDynamic.type == DetectObjectDynamic.DetectionType.HandCollider)
            {
                EditorGUILayout.PropertyField(DynamicObjectParents);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    #endif
}