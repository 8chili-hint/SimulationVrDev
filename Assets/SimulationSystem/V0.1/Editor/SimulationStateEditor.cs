using System;
using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Assessment.Interface;
using SimulationSystem.V0._1.Simulation;
using UnityEditor;
using UnityEngine.Video;
using UnityEngine;
using SimulationSystem.V0._1.Manager;

namespace SimulationSystem.V0._1.Editor
{
    [CustomEditor(typeof(SimulationState))]
    public class SimulationStateEditor : UnityEditor.Editor
    {
        private SimulationState _simulationState;
        private AssessmentController _assessmentController;
        private List<ObjectToDetectPerState> currentStateDetects = new List<ObjectToDetectPerState>();
        private bool left = false;
        private bool right = false;

        private void OnEnable()
        {
            _simulationState = (SimulationState)target;
        }

        public override void OnInspectorGUI()
        {
            #region EnumInspector
            var EnumProperty = serializedObject.FindProperty("stateType");
            EditorGUILayout.PropertyField(EnumProperty, true);
            serializedObject.ApplyModifiedProperties();
            #endregion

            #region PromptInspector
            GUILayout.Label("[Prompt]");
            GUILayout.Space(10);
            var AudioPromptProperty = serializedObject.FindProperty("audioPrompt");
            var stringPromptProperty = serializedObject.FindProperty("textPrompt");
            var VideoClipPromptProperty = serializedObject.FindProperty("videoPrompt");

            EditorGUILayout.PropertyField(AudioPromptProperty, true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.PropertyField(stringPromptProperty, true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.PropertyField(VideoClipPromptProperty, true);
            serializedObject.ApplyModifiedProperties();

            #endregion

            #region EventInspector
            GUILayout.Label("[Events]");
            GUILayout.Space(10);

            var onStateStartEventProperty = serializedObject.FindProperty("onStateStart");
            var onStateCompleteEventProperty = serializedObject.FindProperty("onStateComplete");

            EditorGUILayout.PropertyField(onStateStartEventProperty, true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.PropertyField(onStateCompleteEventProperty, true);
            serializedObject.ApplyModifiedProperties();

            #endregion

            #region DelayedEventsInspector
            var OnStateDelayedProperty = serializedObject.FindProperty("onStateStartDelayedEvents");

            EditorGUILayout.PropertyField(OnStateDelayedProperty, true);
            serializedObject.ApplyModifiedProperties();


            GUILayout.Space(5);
            #endregion

            #region ChangebaleState And InstructorUpdater Inspector
            GUILayout.Label("[Changeable States]");
          
   
            var playStateInAssessmentModeProperty = serializedObject.FindProperty("playStateInAssessmentMode");
            var instructorUpdaterProperty = serializedObject.FindProperty("instructorUpdater");

            EditorGUILayout.PropertyField(playStateInAssessmentModeProperty, true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.PropertyField(instructorUpdaterProperty, true);
            serializedObject.ApplyModifiedProperties();


            #endregion




            if (_assessmentController != null)
            {
                if (_assessmentController.useGrabAssessment)
                {
                    var serializedProperty = serializedObject.FindProperty("allowedStateGrabbables");
                    EditorGUILayout.PropertyField(serializedProperty,true);
                    
                    serializedObject.ApplyModifiedProperties();

                    var StateserializedProperty = serializedObject.FindProperty("stateGrabbables");
                    EditorGUILayout.PropertyField(StateserializedProperty, true);

                    serializedObject.ApplyModifiedProperties();
                }
            }
            

            switch (_simulationState.stateType)
            {
                case (SimulationState.StateType.DetectWithGrab):
                    EditorGUILayout.Space(5);
                    serializedObject.Update();
                    var objectToDetectListSerializedProperty = serializedObject.FindProperty("objectToDetectList");
                    EditorGUILayout.PropertyField(objectToDetectListSerializedProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    var serializeddProperty = serializedObject.FindProperty("stateGrabbables");
                    EditorGUILayout.PropertyField(serializeddProperty, true);

                    serializedObject.ApplyModifiedProperties();
                    var GrabbableserializedProperty = serializedObject.FindProperty("allowedStateGrabbables");
                    EditorGUILayout.PropertyField(GrabbableserializedProperty, true);

                    serializedObject.ApplyModifiedProperties();



                    _simulationState.nextStateOnObjectGrab = false;
                    _simulationState.nextStateOnPromptEnd = false;
                    break;

                case (SimulationState.StateType.DetectWithHand):
                    EditorGUILayout.Space(5);
                    serializedObject.Update();

                    GUILayout.Label("[Detect Hand]");
                    var leftHandBool = serializedObject.FindProperty("LeftHand");
                    EditorGUILayout.PropertyField(leftHandBool, true);
                    serializedObject.ApplyModifiedProperties();


                    var rightHandBool = serializedObject.FindProperty("RightHand");
                    EditorGUILayout.PropertyField(rightHandBool, true);
                    serializedObject.ApplyModifiedProperties();


                    var objectToDetectWithHandListSerializedProperty = serializedObject.FindProperty("objectToDetectList");
                    EditorGUILayout.PropertyField(objectToDetectWithHandListSerializedProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    if (left != leftHandBool.boolValue)
                    {
                        left = leftHandBool.boolValue;
                        CheckLeftHand(leftHandBool.boolValue, objectToDetectWithHandListSerializedProperty);

                    }
                    if (right != rightHandBool.boolValue)
                    {
                        right = rightHandBool.boolValue;
                        CheckRightHand(rightHandBool.boolValue, objectToDetectWithHandListSerializedProperty);

                    }


                    _simulationState.nextStateOnObjectGrab = false;
                    _simulationState.nextStateOnPromptEnd = false;
                    break;

                case SimulationState.StateType.Prompt:
                    _simulationState.nextStateOnObjectGrab = false;
                    _simulationState.nextStateOnPromptEnd = true;
                    break;

                case SimulationState.StateType.Grab:
                    var serializedProperty = serializedObject.FindProperty("stateGrabbables");
                    EditorGUILayout.PropertyField(serializedProperty, true);

                    serializedObject.ApplyModifiedProperties();
                    var ThisGrabbableserializedProperty = serializedObject.FindProperty("allowedStateGrabbables");
                    EditorGUILayout.PropertyField(ThisGrabbableserializedProperty, true);

                    serializedObject.ApplyModifiedProperties();
                    _simulationState.nextStateOnObjectGrab = true;
                    var abcd = serializedObject.FindProperty("nextStateOnObjectGrab");
                    EditorGUILayout.PropertyField(abcd, true);


                    _simulationState.CanGoToNextStateOnGrab = true;
                    var abcd2 = serializedObject.FindProperty("CanGoToNextStateOnGrab");
                    EditorGUILayout.PropertyField(abcd2, true);
                    

                    _simulationState.nextStateOnPromptEnd = false;
                    break;

                case SimulationState.StateType.UI:
                    EditorGUILayout.Space(5);
                    var uiParentAnimationHandlerSerializedProperty = serializedObject.FindProperty("uiParentAnimationHandler");
                    EditorGUILayout.PropertyField(uiParentAnimationHandlerSerializedProperty,true);
   /*                 var buttonPointableUnityEventWrapperSerializedProperty = serializedObject.FindProperty("buttonPokeInteractable");
                    EditorGUILayout.PropertyField(buttonPointableUnityEventWrapperSerializedProperty, true);*/
                    serializedObject.ApplyModifiedProperties();
                    
                    _simulationState.nextStateOnObjectGrab = false;
                    _simulationState.nextStateOnPromptEnd = false;
                    break;

                case (SimulationState.StateType.Gaze):
                    EditorGUILayout.Space(5);
                    serializedObject.Update();
                    var objectToDetectSerializedProperty = serializedObject.FindProperty("objectToDetectList");
                    EditorGUILayout.PropertyField(objectToDetectSerializedProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    _simulationState.nextStateOnObjectGrab = false;
                    _simulationState.nextStateOnPromptEnd = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_assessmentController == null)
            {
                _simulationState.maxScore = 0;
                _simulationState.shouldShowPrompt = true;
                
                if (_simulationState.TryGetComponent<AssessmentController>(out AssessmentController assessmentController))
                {
                    _assessmentController = assessmentController;
                }
            }
            else
            {    
                    _simulationState.shouldShowPrompt = false;              
            }



            var GrabbableHelper = serializedObject.FindProperty("GrabbableHelper");

            EditorGUILayout.PropertyField(GrabbableHelper, true);
            serializedObject.ApplyModifiedProperties();
        }
        private void CheckLeftHand(bool left, SerializedProperty stateDetects)
        {
            if (!left)
            {
                Debug.Log("Left Hand Clicked");
                var index = GetLeftIndex(_simulationState.objectToDetectList);

                if (index >= 0)
                {
                    stateDetects.DeleteArrayElementAtIndex(index);
                }
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var index = GetLeftIndex(_simulationState.objectToDetectList);

                if (index == -1)
                {
                    ObjectToDetectPerState obj = new ObjectToDetectPerState();

                    obj.gameObjectsToDetect = new List<Collider>();

                    obj.detectObject = _simulationState.cachedObjectToDetectList[1].detectObject;
                    obj.gameObjectsToDetect = _simulationState.cachedObjectToDetectList[1].gameObjectsToDetect;
                    obj.shouldMoveToNextState = _simulationState.cachedObjectToDetectList[1].shouldMoveToNextState;
                    _simulationState.objectToDetectList.Add(obj);


                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void CheckRightHand(bool right, SerializedProperty stateDetects)
        {


            if (!right)
            {

                Debug.Log("Right Hand Clicked");
                var index = GetRightIndex(_simulationState.objectToDetectList);
                if (index >= 0)
                {
                    stateDetects.DeleteArrayElementAtIndex(index);
                }
                serializedObject.ApplyModifiedProperties();
            }

            else
            {

                var index = GetRightIndex(_simulationState.objectToDetectList);

                if (index == -1)
                {
                    ObjectToDetectPerState obj = new ObjectToDetectPerState();

                    obj.gameObjectsToDetect = new List<Collider>();

                    obj.detectObject = _simulationState.cachedObjectToDetectList[0].detectObject;
                    obj.gameObjectsToDetect = _simulationState.cachedObjectToDetectList[0].gameObjectsToDetect;
                    obj.shouldMoveToNextState = _simulationState.cachedObjectToDetectList[0].shouldMoveToNextState;
                    _simulationState.objectToDetectList.Add(obj);

                    serializedObject.ApplyModifiedProperties();

                }

            }
        }

        private int GetLeftIndex(List<ObjectToDetectPerState> objList)
        {
            for (int i = 0; i <= objList.Count - 1; i++)
            {
                if (objList[i].gameObjectsToDetect[0].name == PlayerManager.Instance.leftHand.gameObject.name)
                    return i;
            }
            return -1;
        }
        private int GetRightIndex(List<ObjectToDetectPerState> objList)
        {
            for (int i = 0; i <= objList.Count - 1; i++)
            {
                if (objList[i].gameObjectsToDetect[0].name == PlayerManager.Instance.rightHand.gameObject.name)
                    return i;
            }
            return -1;
        }
    }
}