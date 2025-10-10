using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Assessment.Assessment_Types;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
    namespace SimulationSystem.V0._1.Editor
    {
        [CustomEditor(typeof(AssessmentController))]
        [CanEditMultipleObjects]
        public class AssessmentControllerEditor : UnityEditor.Editor
        {
            private SerializedProperty _useGrabAssessment,_useDetectAssessment, _useWrongHandAssessment, _useUIAssessment;
            private AssessmentController _target;

            private void OnEnable()
            {
                _useGrabAssessment = serializedObject.FindProperty("useGrabAssessment");
                _useDetectAssessment = serializedObject.FindProperty("useDetectAssessment");
                _useWrongHandAssessment = serializedObject.FindProperty("useWrongHandAssessment");
                _useUIAssessment = serializedObject.FindProperty("useUIAssessment");
            
                _target = (AssessmentController)target;
        
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                DrawDefaultInspector();
                serializedObject.ApplyModifiedProperties();

                if (GUI.changed )
                {
                    if (_useGrabAssessment.boolValue)
                    {
                
                        if (!_target.GetComponent<GrabAssessment>())
                        {
                            _target.gameObject.AddComponent<GrabAssessment>();
                        }
                    }
                    else
                    {
        
                        if (_target.GetComponent<GrabAssessment>())
                        {
                            DestroyImmediate(_target.GetComponent<GrabAssessment>());
                        }
                    }

                    if (_useDetectAssessment.boolValue)
                    {
    
                        if (!_target.GetComponent<DetectAssessment>())
                        {
                            _target.gameObject.AddComponent<DetectAssessment>();
                        }
                    }
                    else
                    {
             
                        if (_target.GetComponent<DetectAssessment>())
                        {
                            DestroyImmediate(_target.GetComponent<DetectAssessment>());
                        }
                    }


                    if (_useWrongHandAssessment.boolValue)
                    {
                
                        if (!_target.GetComponent<WrongHandAssessment>())
                        {
                            _target.gameObject.AddComponent<WrongHandAssessment>();
                        }
                    }
                    else
                    {
              
                        if (_target.GetComponent<WrongHandAssessment>())
                        {
                            DestroyImmediate(_target.GetComponent<WrongHandAssessment>());
                        }
                    }


                    if (_useUIAssessment.boolValue)
                    {
            
                        if (!_target.GetComponent<UIAssessment>())
                        {
                            _target.gameObject.AddComponent<UIAssessment>();
                        }
                    }
                    else
                    {
            
                        if (_target.GetComponent<UIAssessment>())
                        {
                            DestroyImmediate(_target.GetComponent<UIAssessment>());
                        }
                    }
                }
            }
        }
    }
#endif
