
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.UI;
using TMPro;
using UnityEditor;
using System.Text;
using System.IO;
using SimulationSystem.V0._1.VR_Player;
using UnityEngine.UI;

public class SimulationStepWizard : EditorWindow
{
    [SerializeField] private TextAsset stepNamesFile;
    [SerializeField] private Transform simulationParent;
    [SerializeField] private Transform leftHandCollider;
    [SerializeField] private Transform rightHandCollider;
    [SerializeField] private Transform centerEyeAnchor;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private GameObject detectPrefab;
    [SerializeField] private Transform detectParent;
    [SerializeField] private GameObject grabbablePrefab;
    [SerializeField] private Transform grabbableParent;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private Transform uiParent;
    [SerializeField] private GameObject gazePrefab;
    [SerializeField] private Transform gazeParent;

    public List<SimulationStepEntity> stepList;

    private List<string> prompts = new List<string>();

    private Vector2 scrollPosition = Vector2.zero;


    #region GenerateStepVariables
    private bool firstHalf = true;
    private string stepIndex = null;
    private string instructionText = null;
    private string stepType;
    private string objectName;
    private bool isAssessed;
    #endregion 

    [MenuItem("SimulationSystem/SimulationStepWizard")]
    private static void ShowWindow()
    {
        var window = GetWindow<SimulationStepWizard>();
        window.titleContent = new GUIContent("SimulationStepWizard");
        window.Show();
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);


        GUILayout.Label("Generate steps and corresponding detects, grabbables and UI using simulation specific CSV. All connections are automatically maintained.\nThen add audio prompts to steps.\n\n<b>Please be careful while clearing all, as it removes all steps, detects, grabbables and UI elements.</b>");
        GUILayout.Space(20);
        GUILayout.Label("CSV File with steps");
        stepNamesFile = EditorGUILayout.ObjectField(stepNamesFile, typeof(TextAsset), true) as TextAsset;

        GUILayout.Space(5);
        GUILayout.Label("Simulation Manager Transform");
        simulationParent = EditorGUILayout.ObjectField(simulationParent, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("Left Hand Collider");
        leftHandCollider = EditorGUILayout.ObjectField(leftHandCollider, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("Right Hand Collider");
        rightHandCollider = EditorGUILayout.ObjectField(rightHandCollider, typeof(Transform), true) as Transform;
        
        GUILayout.Space(5);
        GUILayout.Label("Center Eye");
        centerEyeAnchor = EditorGUILayout.ObjectField(centerEyeAnchor, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("Detect Prefab");
        detectPrefab = EditorGUILayout.ObjectField(detectPrefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(5);
        GUILayout.Label("Detect Parent");
        detectParent = EditorGUILayout.ObjectField(detectParent, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("Grabbable Prefab");
        grabbablePrefab = EditorGUILayout.ObjectField(grabbablePrefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(5);
        GUILayout.Label("Grabbable Parent");
        grabbableParent = EditorGUILayout.ObjectField(grabbableParent, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("UI Prefab");
        uiPrefab = EditorGUILayout.ObjectField(uiPrefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(5);
        GUILayout.Label("UI Parent");
        uiParent = EditorGUILayout.ObjectField(uiParent, typeof(Transform), true) as Transform;

        GUILayout.Space(5);
        GUILayout.Label("Gaze Prefab");
        gazePrefab = EditorGUILayout.ObjectField(gazePrefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(5);
        GUILayout.Label("Gaze Parent");
        gazeParent = EditorGUILayout.ObjectField(gazeParent, typeof(Transform), true) as Transform;

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty audioClipProperty = so.FindProperty("audioClips");
        GUILayout.Space(5);
        GUILayout.Label("Visual child of grab/detect");
        EditorGUILayout.PropertyField(audioClipProperty, true);
        so.ApplyModifiedProperties();

        GUILayout.Space(30);
        if (GUILayout.Button("Generate Steps (one-time)"))
        {
            GenerateSteps();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Clear All Steps"))
        {
            ClearAll();
           
           var objectToDetectPerStates = FindObjectsOfType<SimulationState>();

            foreach(SimulationState a in objectToDetectPerStates)
            {
                a.cachedObjectToDetectList.Clear();
            }

            var abcd = FindObjectsOfType<SimulationState>();

            foreach (SimulationState a in abcd)
            {
                a.objectToDetectList.Clear();
            }


        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add audio to steps"))
        {
            AddAudioToSteps();
        }
        GUILayout.Space(30);
        GUILayout.Label("Prompts");

        GUILayout.Space(10);
        if (GUILayout.Button("Get Prompt Texts"))
        {
            GetText();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Update Prompt Texts"))
        {
            UpdateText();
        }

        GUILayout.EndScrollView();
    }
    
    public void GenerateSteps()
    {
        // if (stepNamesFile != null)
        {
            Debug.Log("generate Steps");
            string filePath = Path.Combine(Application.dataPath, "stepSheet.tsv");
            // string fileContent = stepNamesFile.text;
            // string[] steps = fileContent.Split('\n');
            string[] steps = File.ReadAllLines(filePath);
            Debug.Log(steps.Length);
            int totalSteps = steps.Length - 1;
            stepList = new List<SimulationStepEntity>();
            
            for (int i = 0; i < totalSteps; i++)
            {
                int index = i + 1;

                string[] stepFields = steps[index].Split("\t");
                Debug.Log(stepFields.Length);
                if (stepFields[0] == "")
                {
                    continue;
                }
                if (stepFields.Length == 4)
                {
                    Debug.Log("No New Line");
                    stepIndex = stepFields[0];
                    instructionText = stepFields[1];
                    stepType = stepFields[2];
                    bool.TryParse(stepFields[3], out isAssessed);

                    CheckState();
                }
                else
                {
                    Debug.Log("New Line Detected");
                    if (firstHalf)
                    {
                        stepIndex = stepFields[0];
                        instructionText = stepFields[1];
                        firstHalf = !firstHalf;
                    }
                    else
                    {
                        string restOfText = stepFields[0];
                        instructionText += restOfText;
                        stepType = stepFields[1];
                        bool.TryParse(stepFields[2], out isAssessed);
                        firstHalf = !firstHalf;
                        CheckState();
                    }
                }
            }
        }
    }

    private void CheckState()
    {
        objectName = $"{stepIndex}";
        objectName += $" - {instructionText}";

        var stepEntity = new SimulationStepEntity();
        stepEntity.stepInstruction = instructionText;
        stepEntity.stepType = GetStateType(stepType);

        var stepObject = new GameObject(objectName);
        stepObject.AddComponent<SimulationState>();
        stepObject.transform.SetParent(simulationParent);
        SimulationState state = stepObject.GetComponent<SimulationState>();
        if (state != null)
        {
            state.textPrompt = instructionText;
            if (isAssessed)
            {
                state.gameObject.AddComponent<AssessmentController>();
            }

            state.stateType = GetStateType(stepType);
        }

        stepEntity.simulationState = state;

        Debug.Log(stepType);

        if (stepType == "Detect With Grab")
        {
            string detectName = $"{stepIndex}_Detect";
            var childDetect = (GameObject)PrefabUtility.InstantiatePrefab(detectPrefab, detectParent);
            state.DetectObject = childDetect.GetComponent<DetectObject>();
            childDetect.name = detectName;

            state.objectToDetectList = new List<ObjectToDetectPerState>();
            state.cachedObjectToDetectList = new List<ObjectToDetectPerState>();


            string grabbableName = $"{stepIndex}_Grab";
            var childGrab = (GameObject)PrefabUtility.InstantiatePrefab(grabbablePrefab, grabbableParent);
            childGrab.name = grabbableName;
            var grabObj = childGrab.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            state.stateGrabbables = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            state.stateGrabbables.Add(grabObj);

            ObjectToDetectPerState obj = new ObjectToDetectPerState();
            obj.detectObject = state.DetectObject;
            obj.shouldMoveToNextState = true;
            obj.gameObjectsToDetect = new List<Collider>();
            var colliders = childGrab.GetComponentsInChildren<BoxCollider>();
            colliders[0].name = $"{stepIndex}_Grab_Collider";
            foreach (var collider in colliders)
            {
                obj.gameObjectsToDetect.Add(collider);
            }

            state.objectToDetectList.Add(obj);
            state.cachedObjectToDetectList.Add(obj);

            stepEntity.detectList.Add(state.DetectObject);
            stepEntity.grabbableList.Add(grabObj);
        }
        else if (stepType == "Detect With Hand")
        {
            state.objectToDetectList = new List<ObjectToDetectPerState>();
            state.cachedObjectToDetectList = new List<ObjectToDetectPerState>();            


            Debug.Log("Detect with Hand");
            string detectName = $"{stepIndex}_Detect_Right";
            var childDetect = (GameObject)PrefabUtility.InstantiatePrefab(detectPrefab, detectParent);
            var detectObject = childDetect.GetComponent<DetectObject>();
            childDetect.name = detectName;


            
            ObjectToDetectPerState obj = new ObjectToDetectPerState();
            obj.detectObject = detectObject;
            obj.shouldMoveToNextState = true;


            obj.gameObjectsToDetect = new List<Collider>();
            var collider = rightHandCollider.GetComponent<SphereCollider>();
            obj.gameObjectsToDetect.Add(collider);


            state.cachedObjectToDetectList.Add(obj);
            stepEntity.detectList.Add(detectObject);
            state.RightHand = true;


            //LeftHand

            string detectNameL = $"{stepIndex}_Detect_Left";
            var childDetectL =(GameObject) PrefabUtility.InstantiatePrefab(detectPrefab, detectParent);
            var detectObjectL = childDetectL.GetComponent<DetectObject>();
            childDetectL.name = detectNameL;

            ObjectToDetectPerState objL = new ObjectToDetectPerState();
            objL.detectObject = detectObjectL;
            objL.shouldMoveToNextState = true;


            objL.gameObjectsToDetect = new List<Collider>();
            var colliderL = leftHandCollider.GetComponent<SphereCollider>();
            objL.gameObjectsToDetect.Add(colliderL);


            state.cachedObjectToDetectList.Add(objL);
            stepEntity.detectList.Add(detectObjectL);
            state.LeftHand = true;
        }
        else if (stepType == "Grab")
        {
            string grabbableName = $"{stepIndex}_Grab";
            var childGrab = (GameObject)PrefabUtility.InstantiatePrefab(grabbablePrefab, grabbableParent);
            childGrab.name = grabbableName;
            var colliders = childGrab.GetComponentsInChildren<BoxCollider>();
            colliders[0].name = $"{stepIndex}_Grab_Collider";
            var grabObj = childGrab.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            state.stateGrabbables = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            state.stateGrabbables.Add(grabObj);
            stepEntity.grabbableList.Add(grabObj);
        }
        else if (stepType == "UI")
        {
            string uiName = $"{stepIndex}_UIWithButton";
            var childUI = (GameObject)PrefabUtility.InstantiatePrefab(uiPrefab, uiParent);
            childUI.name = uiName;
            stepEntity.uiObject = (GameObject)childUI;
            var textObjs = childUI.GetComponentsInChildren<TextMeshProUGUI>();
            if (textObjs.Length > 0)
            {
                textObjs[0].text = stepEntity.stepInstruction;
            }
            state.uiParentAnimationHandler = childUI.GetComponent<UIAnimationHandler>();
            var pointableInteractor = childUI.GetComponentInChildren<Button>();
            if (pointableInteractor)
            {
                state.UIButtonComponent = pointableInteractor;
            }
        }
        else if (stepType == "Gaze")
        {
            string detectName = $"{stepIndex}_Gaze";
            var childDetect = (GameObject)PrefabUtility.InstantiatePrefab(gazePrefab, gazeParent);
            state.DetectObject = childDetect.GetComponent<DetectObject>();
            childDetect.name = detectName;

            state.objectToDetectList = new List<ObjectToDetectPerState>();
            state.cachedObjectToDetectList = new List<ObjectToDetectPerState>();


            ObjectToDetectPerState obj = new ObjectToDetectPerState();
            obj.detectObject = state.DetectObject;
            obj.shouldMoveToNextState = true;
            obj.gameObjectsToDetect = new List<Collider>();
            
            obj.gameObjectsToDetect.Add(centerEyeAnchor.gameObject.GetComponent<Collider>());
            

            state.objectToDetectList.Add(obj);
            state.cachedObjectToDetectList.Add(obj);

            stepEntity.detectList.Add(state.DetectObject);
        }


        stepList.Add(stepEntity);
    }


    public void AddStepEntityExperimental(int stepIndex, string instructionText, string stepType, bool isAssessed)
    {
        // Sync();
        //
        // var afterIndexStepList = stepList.GetRange(stepIndex - 1, stepList.Count - stepIndex + 1);
        // stepList.RemoveRange(stepIndex - 1, stepList.Count - stepIndex + 1);
        //
        // var stepEntity = new SimulationStepEntity();
        // stepEntity.stepInstruction = instructionText;
        // stepEntity.stepType = GetStateType(stepType);
        //
        // var objectName = $"{stepIndex}";
        // objectName += $" - {instructionText}";
        //
        // var stepObject = new GameObject(objectName);
        // stepObject.AddComponent<SimulationState>();
        // stepObject.transform.SetParent(simulationParent);
        // SimulationState state = stepObject.GetComponent<SimulationState>();
        // if (state != null)
        // {
        //     state.textPrompt = instructionText;
        //     if (isAssessed)
        //     {
        //         state.gameObject.AddComponent<AssessmentController>();
        //     }
        //
        //     state.stateType = GetStateType(stepType);
        // }
        //
        // stepEntity.simulationState = state;
        //
        // if (stepType == "Detect")
        // {
        //     string detectName = $"{stepIndex}_Detect";
        //     var childDetect = PrefabUtility.InstantiatePrefab(detectPrefab, detectParent);
        //     var detectObject = childDetect.GetComponent<DetectObject>();
        //     childDetect.name = detectName;
        //     state.objectToDetectList = new List<ObjectToDetectPerState>();
        //     string grabbableName = $"{stepIndex}_Grab";
        //     var childGrab = PrefabUtility.InstantiatePrefab(grabbablePrefab, grabbableParent);
        //     childGrab.name = grabbableName;
        //     var grabObj = childGrab.GetComponent<Grabbable>();
        //     state.stateGrabbables = new List<Grabbable>();
        //     state.stateGrabbables.Add(grabObj);
        //     ObjectToDetectPerState obj = new ObjectToDetectPerState();
        //     obj.detectObject = detectObject;
        //     obj.shouldMoveToNextState = true;
        //     obj.gameObjectsToDetect = new List<GameObject>();
        //     var colliders = childGrab.GetComponentsInChildren<BoxCollider>();
        //     foreach (var collider in colliders)
        //     {
        //         obj.gameObjectsToDetect.Add(collider.gameObject);
        //     }
        //     state.objectToDetectList.Add(obj);
        //     
        //     stepEntity.detectList.Add(detectObject);
        //     stepEntity.grabbableList.Add(grabObj);
        // }
        // else if (stepType == "Grab")
        // {
        //     string grabbableName = $"{stepIndex}_Grab";
        //     var childGrab = PrefabUtility.InstantiatePrefab(grabbablePrefab, grabbableParent);
        //     childGrab.name = grabbableName;
        //     var grabObj = childGrab.GetComponent<Grabbable>();
        //     state.stateGrabbables = new List<Grabbable>();
        //     state.stateGrabbables.Add(grabObj);
        //     stepEntity.grabbableList.Add(grabObj);
        // }
        // else if (stepType == "UI")
        // {
        //     string uiName = $"{stepIndex}_UI";
        //     var childUI = PrefabUtility.InstantiatePrefab(uiPrefab, uiParent);
        //     childUI.name = uiName;
        //     stepEntity.uiObject = childUI as GameObject;
        //     var textObjs = childUI.GetComponentsInChildren<TextMeshProUGUI>();
        //     if (textObjs.Length > 0)
        //     {
        //         textObjs[0].text = instructionText;
        //     }
        // }
        // stepList.Add(stepEntity);
        //
        // foreach (var sEnt in afterIndexStepList)
        // {
        //     stepList.Add(sEnt);
        // }
        //
        // for (int i = 0; i < stepList.Count; i++)
        // {
        //     var sEnt = stepList[i];
        //     int index = i + 1;
        //     var objName = $"{index}";
        //     objName += $" - {sEnt.stepInstruction}";
        //
        //     sEnt.simulationState.gameObject.name = objName;
        //     foreach(var d in sEnt.detectList)
        //     {
        //         d.gameObject.name = $"{index}_Detect";
        //     }
        //     foreach(var g in sEnt.grabbableList)
        //     {
        //         g.gameObject.name = $"{index}_Grab";
        //     }
        //     if(sEnt.uiObject)
        //         sEnt.uiObject.name = $"{index}_UI";
        // }
        // ReorderSimulationItems();

    }

    private void ReorderSimulationItems()
    {
        for (int i = 0; i < stepList.Count; i++)
        {
            stepList[i].simulationState.transform.SetSiblingIndex(i);
        }
        // ReorderChildren(detectParent);
        // ReorderChildren(grabbableParent);
        // ReorderChildren(uiParent);
    }
    private void ReorderChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
            children[i] = parent.GetChild(i);

        var sortedChildren = children.OrderBy(child => child.name).ToArray();

        for (int i = 0; i < sortedChildren.Length; i++)
            sortedChildren[i].SetSiblingIndex(i);
    }

    private void Sync()
    {
        stepList = new List<SimulationStepEntity>();
        foreach (Transform stepObj in simulationParent)
        {
            var stepEntity = new SimulationStepEntity();
            SimulationState step;
            if (!stepObj.TryGetComponent<SimulationState>(out step))
            {
                Debug.LogError($"Step _{stepObj.name}_ doesn't contain SimulationState");
                return;
            }

            stepEntity.simulationState = step;
            stepEntity.stepInstruction = step.textPrompt;
            stepEntity.stepType = step.stateType;
            if (step.stateType == SimulationState.StateType.DetectWithGrab)
            {
                stepEntity.detectList.Add(step.objectToDetectList[0].detectObject);
                stepEntity.grabbableList.Add(step.stateGrabbables[0]);
            }
            else if (step.stateType == SimulationState.StateType.DetectWithHand)
            {
                stepEntity.detectList.Add(step.objectToDetectList[0].detectObject);
            }
            else if (step.stateType == SimulationState.StateType.Grab)
                stepEntity.grabbableList.Add(step.stateGrabbables[0]);
            else if (step.stateType == SimulationState.StateType.UI)
                stepEntity.uiObject = step.uiParentAnimationHandler.gameObject;
            else if (step.stateType == SimulationState.StateType.Gaze)
            {
                stepEntity.detectList.Add(step.objectToDetectList[0].detectObject);
            }
            stepList.Add(stepEntity);
        }
    }


    public void ClearAll()
    {
        stepList.Clear();
        for (int i = simulationParent.childCount - 1; i >= 0; i--)
        {
            Transform child = simulationParent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        for (int i = detectParent.childCount - 1; i >= 0; i--)
        {
            Transform child = detectParent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        for (int i = grabbableParent.childCount - 1; i >= 0; i--)
        {
            Transform child = grabbableParent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        for (int i = uiParent.childCount - 1; i >= 0; i--)
        {
            Transform child = uiParent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        for (int i = gazeParent.childCount - 1; i >= 0; i--)
        {
            Transform child = gazeParent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    public void UpdateStepText()
    {
        // stepNames = new List<string>();
        //
        // if (stepNamesFile != null)
        // {
        //     #region Get all step info
        //
        //     string fileContent = stepNamesFile.text;
        //     string[] steps = fileContent.Split('\n');
        //     int totalSteps = steps.Length - 2;
        //     Debug.LogError(totalSteps);
        //     Debug.LogError(simulationTransform.childCount);
        //     
        //     string[] objectNames = new string[totalSteps];
        //     string[] stepInstructions = new string[totalSteps];
        //     string[] stepTypes = new string[totalSteps];
        //     bool[] toBeAssessed = new bool[totalSteps];
        //     for (int i = 0; i < totalSteps; i++)
        //     {
        //         
        //         int index = i + 1;
        //         string tmp;
        //
        //         string[] stepFields = steps[index].Split(',');
        //
        //         string stepIndex = stepFields[0];
        //         string instructionText = stepFields[1];
        //         string stepType = stepFields[2];
        //         bool.TryParse(stepFields[3], out bool isAssessed);
        //         
        //         tmp = $"{stepIndex}";
        //         tmp += $" - {instructionText}";
        //         stepInstructions[i] = instructionText;
        //         objectNames[i] = tmp;
        //         stepNames.Add(instructionText);
        //         stepTypes[i] = stepType;
        //         toBeAssessed[i] = isAssessed;
        //     }
        //
        //     #endregion
        //
        //     #region Set GameObject, step prompt and step type
        //
        //     for (int i = 0; i < objectNames.Length; i++)
        //     {
        //         if (objectNames.Length <= i)
        //         {
        //             break;
        //         }
        //
        //         var child = simulationTransform.GetChild(i);
        //         if (child)
        //         {
        //             var tmp = $"{i}";
        //             tmp += $" - {stepNames[i]}";
        //             child.name = tmp;   
        //             SimulationState state = child.GetComponent<SimulationState>();
        //             if (state != null)
        //             {
        //                 state.textPrompt = stepNames[i];
        //             }
        //         }
        //     }
        //
        //     #endregion
        // }
    }
    public void AddAudioToSteps()
    {
        int j = 0;
        for (int i = 0; i < simulationParent.childCount; i++)
        {
            if (simulationParent.GetChild(i).gameObject.activeInHierarchy)
            {
                if (simulationParent.GetChild(i) && simulationParent.GetChild(i).GetComponent<SimulationState>())
                {
                    simulationParent.GetChild(i).GetComponent<SimulationState>().audioPrompt = audioClips[j];
                    j++;
                }
            }
        }
    }
    public void UpdateText()
    {
        string directory = Environment.CurrentDirectory + "/PromptsFile";
        if (!Directory.Exists(directory))
        {
            Debug.Log("No Directory Found");
            return;
        }
        string path = directory + "/Prompts.txt";
        var prompts = File.ReadAllLines(path, Encoding.UTF8);
        var steps = simulationParent.GetComponentsInChildren<SimulationState>(false);
        for (int i = 0; i <= steps.Length - 1; i++)
        {
            steps[i].textPrompt = prompts[i];
            if (steps[i].stateType == SimulationState.StateType.UI)
            {
                CheckIfChecklist(steps[i], prompts[i]);
            }
            steps[i].name = UpdateStepName(steps[i].name, prompts[i]);
        }
    }
    private string UpdateStepName(string previousName, string updatedName)
    {
        string[] splitName = previousName.Split("-");
        string newStepName = splitName[0] + "- " + updatedName;
        return newStepName;
    }
    private void CheckIfChecklist(SimulationState step, string prompt)
    {
        if (step.uiParentAnimationHandler.gameObject.name.Length >= 10)
        {
            if (step.uiParentAnimationHandler.gameObject.name.Substring(0, 9) == "Checklist")
            {
                return;
            }
            else
            {
                step.uiParentAnimationHandler.GetComponentInChildren<TMP_Text>().text = prompt;
            }
        }
        else
            return;
    }
    public void GetText()
    {
        var states = simulationParent.GetComponentsInChildren<SimulationState>(false);
        prompts = new List<string>();
        foreach (var state in states)
        {
            prompts.Add(state.textPrompt);
        }
        string directory = Environment.CurrentDirectory + "/PromptsFile";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string path = directory + "/Prompts.txt";

        File.WriteAllLines(path, prompts, Encoding.UTF8);
        Debug.Log("PromptsWritten");
    }


    public SimulationState.StateType GetStateType(string type)
    {
        if (type == "Grab")
            return SimulationState.StateType.Grab;
        else if (type == "Detect With Grab")
            return SimulationState.StateType.DetectWithGrab;
        else if (type == "Detect With Hand")
            return SimulationState.StateType.DetectWithHand;
        else if (type == "UI")
            return SimulationState.StateType.UI;
        else if (type == "Gaze")
            return SimulationState.StateType.Gaze;
        else
            return SimulationState.StateType.Prompt;
    }

    [Serializable]
    public class SimulationStepEntity
    {
        public string stepInstruction;
        public SimulationState.StateType stepType;
        public SimulationState simulationState;
        public List<DetectObject> detectList;
        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> grabbableList;
        public GameObject uiObject;

        public SimulationStepEntity()
        {
            detectList = new List<DetectObject>();
            grabbableList = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();                
        }
    }



    
}
#endif