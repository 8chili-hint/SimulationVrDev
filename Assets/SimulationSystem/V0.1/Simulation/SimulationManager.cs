using System;
using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Legacy.Refactoring;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Simulation.Manager;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Utility.APIs;
using SimulationSystem.V0._1.Utility.Miscellanous;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Events;
namespace SimulationSystem.V0._1.Simulation
{
    public class SimulationManager : MonoBehaviour
    {
        

        #region Variable declaration

        public static SimulationManager instance;

        [Header("Prompt and Hint Fields")]
        [SerializeField] private UIAnimationHandler setSimulationModeUI;
        [SerializeField] public UIAnimationHandler hintButton;
        [SerializeField] public Transform promptHolder;

        [Header("State Change UI fields")]
        [SerializeField] private float stateChangeCountdownTime;
        [SerializeField] private UIAnimationHandler stateChangeCountdownUI;

        [Header("Prompts")] 
        [HideInInspector] [SerializeField] public bool stateChangeOnlyAfterPromptIsOver;
        private UITextHandler _stateChangeCountdownUITextHandler;
        private UIImageHandler _stateChangeCountdownUIImageHandler;

        private Timer _stateChangeCountdownTimer;
        


        //use in future no need in strides modules
        [HideInInspector] public StateManagerStateOverride stateManagerStateOverride;
        
        [HideInInspector] public bool isAssessmentMode;
        [HideInInspector] public SimulationState currentState;
        [HideInInspector] public int currentStateIndex;
        [HideInInspector] public List<SimulationState> simulationStates;
        [HideInInspector] public List<SimulationState> BufferedStates;

        [HideInInspector] public int simulationId = 0;

        [HideInInspector] public Button _hintButtonComponent;

        [Header("Assessment")]
        [HideInInspector] public float totalScore;
        [HideInInspector] public float currentScore;

        //Expose in future if need score.
        [HideInInspector] public TMP_Text Scoretext;
        
        [Header("Assessment Prompts")]
        public AudioClip hintAssessmentPrompt;
        public AudioClip grabsAssessmentPrompt;
        public AudioClip wrongHandAssessmentPrompt;
        public AudioClip UIAssessmentPrompt;
        public AudioClip detectAssessmentPrompt;
        public AudioClip failureAssessmentPrompt;

        [Header("Automation")] 
        public bool shouldAutomate;
        public int automateTillStateIndex;



       

        [Header("Scenario States")]
        //Expose it in future for enabling scenarios
       [HideInInspector] public List<Scenarios> AddScenarios;

        [Header("General Component Overrides")]
        public DetectVizmode TurnOffDetectVizUsing = DetectVizmode.FirstChild;
        public bool GrabbableShouldNotResetInItsRespectiveStep;


        [Header("This is called on Awake//On Start Of the Scene")]
        public UnityEvent OnSimulationSceneBegin;
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            #region Set Scenario InActive Here
            foreach (var scenario in AddScenarios)
            {
                foreach (var State in scenario.DefaultScenarios)
                {
                    State.gameObject.SetActive(true);
                }
                foreach(var State in scenario.AlternateScenarioStates)
                {
                    State.gameObject.SetActive(true);
                }
            }
          
            #endregion
            SimulationStatePromptManager.isFailure = false;

            instance = this;

            SimulationStateGrabbableManager.simulationManager = this;
            foreach (Transform simulationTransform in transform)
            {
                if (simulationTransform.gameObject.activeSelf)
                    simulationStates.Add(simulationTransform.GetComponent<SimulationState>());
            }
            SimulationStatePromptManager.SetupPrompt(promptHolder);

            if (hintButton != null)
            {
                _hintButtonComponent = hintButton.GetComponentInChildren<Button>();
                _hintButtonComponent.onClick.AddListener(CheckHintAssessment);
            }
            
            _stateChangeCountdownUITextHandler = stateChangeCountdownUI.GetComponent<UITextHandler>();
            _stateChangeCountdownUIImageHandler = stateChangeCountdownUI.GetComponent<UIImageHandler>();
            
            _stateChangeCountdownTimer = new Timer(stateChangeCountdownTime);
            _stateChangeCountdownTimer.OnTimerStart += () =>
            {
                stateChangeCountdownUI.OnDetectOnce();
                _stateChangeCountdownUITextHandler.UpdateText("0");
                _stateChangeCountdownUIImageHandler.OnDetecting(0);
            };
            _stateChangeCountdownTimer.OnTimerRunning += timerProgress =>
            {
                var timeLeft = (1 - timerProgress) * stateChangeCountdownTime;
                _stateChangeCountdownUITextHandler.UpdateText((timeLeft).ToString("0"));
                _stateChangeCountdownUIImageHandler.OnDetecting(Mathf.InverseLerp(stateChangeCountdownTime, 0, timeLeft));
            };
            _stateChangeCountdownTimer.OnTimerEnd += () =>
            {
                // _stateChangeCountdownUITextHandler.UpdateText("Next Step");
                _stateChangeCountdownUIImageHandler.OnDetecting(0);
                stateChangeCountdownUI.OnUnDetected();
                ChangeState(currentStateIndex + 1);
            };
        }
        void CheckHintAssessment()
{
        if (isAssessmentMode)
        {
            AssessmentManager.DeductScore(AssessmentType.Hint);
        }
}

        private void Start()
        {

            if(setSimulationModeUI != null) setSimulationModeUI.OnDetectOnce();
            else SetMode(false);

            
        }

        #endregion

        #region State Functions

        public void NextState()
        {
            if (currentStateIndex < simulationStates.Count)
            {
                if (stateChangeOnlyAfterPromptIsOver)
                {
                    StartCoroutine(StateChangeOnlyAfterPromptIsOverCoroutine());
                }
                else
                {
                    _stateChangeCountdownTimer.StartTimer();
                }
            }
        }

        private IEnumerator StateChangeOnlyAfterPromptIsOverCoroutine()
        {

            if (SimulationStatePromptManager.promptAudioSource != null) yield return new WaitUntil(() => !SimulationStatePromptManager.promptAudioSource.isPlaying);
            _stateChangeCountdownTimer.StartTimer();
        }
        
        public void PreviousState()
        {
            if (currentStateIndex > 0)
            {
                ChangeState(currentStateIndex-1);
            }
        }
        
        private void ChangeState(int index)
        {
            EndState();
            currentStateIndex = index;
            
            if (currentStateIndex == simulationStates.Count)
            {
                SimulationEnd();
                return;
            }
            
            currentState = simulationStates[currentStateIndex];
            
            switch (isAssessmentMode)
            {
                case true when !currentState.isAssessed:
                    SimulationStateGrabbableManager.DisableAllUnAssessedGrabbables();
                    break;
                case true when currentState.isAssessed:
                    SimulationStateGrabbableManager.EnableAllUnAssessedGrabbables();
                    break;
            }
            PlayState();
        }
                
        private void PlayState()
        {

            currentState.onStateStart.Invoke();

            if (shouldAutomate && currentStateIndex <= automateTillStateIndex)
            {
               // SimulationStateAutomation.AutomateState(currentState);
            }
            
            if (isAssessmentMode)
            {
                if (currentState.shouldShowPrompt)
                {
                    hintButton.OnUnDetected();
                    _hintButtonComponent.interactable = false;
                    _hintButtonComponent.enabled = false;

                    SimulationStatePromptManager.DisplayPrompts();
                }
                else
                {
                    hintButton.OnDetectOnce();
                    _hintButtonComponent.interactable = true;
                    _hintButtonComponent.enabled = true;

                }
            }
            else
            {
                SimulationStatePromptManager.DisplayPrompts();
            }

            foreach (var delayedEvent in currentState.onStateStartDelayedEvents)
            {
                delayedEvent.InvokeDelayedEvent();
            }

            if (GrabbableShouldNotResetInItsRespectiveStep)
            {
                foreach (var Grabbable in currentState.stateGrabbables)
                {
                    Grabbable.GetComponent<ObjectMovementHelper>().ResetThisObjectOnRelease = false;
                }
            }


            if (currentStateIndex != 0)
            {
               /* GameManager.Instance.PlayerManager.RightGrabInteractor.Enable();
                GameManager.Instance.PlayerManager.LeftGrabInteractor.Enable();
*/
            }
        }

        public void RestartScene()
        {
            EndState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SimulationEnd()
        {
            long epochTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            SimpleJSON.JSONObject  obj = new JSONObject();
            obj.Add("end_ts", epochTime.ToString());
            string json = obj.ToString();
            APIManager.Instance.StartCoroutine(APIManager.Instance.POSTRequest(json, ApiUrlManager.SimulationEnd(APIManager.Instance.simulationID), null));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            { 
                //NextState();
            }
        }

        private void EndState()
        {
            SimulationStatePromptManager.CLearPrompts();

            currentState.onStateComplete.Invoke();
            StopAllCoroutines();
          
            int _tempIndex = SimulationManager.instance.BufferedStates.IndexOf(SimulationManager.instance.currentState);
            AssessmentMessageGenerator.GenerateAssessmentStepResult(_tempIndex, currentState.currentScore, currentState.assessmentStatus, currentState.errorMessage);
        }

        #endregion

        #region Set Simulation Mode

        public void SetMode(bool setAssessmentMode)
        {   simulationStates.Clear();
            BufferedStates = new List<SimulationState>();
            foreach (Transform simulationTransform in transform)
            {
                if (simulationTransform.gameObject.activeSelf)
                {
                    Debug.Log(simulationTransform.gameObject.name);
                    var state = simulationTransform.GetComponent<SimulationState>();
                    state.setGrabbableHelperEvents();
                    if ((setAssessmentMode && state.playStateInAssessmentMode) || !setAssessmentMode) 
                    {   simulationStates.Add(state);
                        BufferedStates.Add(state);
                    }
                }
            }

            isAssessmentMode = setAssessmentMode;
            Debug.Log("assessment mode " + isAssessmentMode);
            AssessmentMessageGenerator.GenerateAssessmentSheet(simulationId);
              
            var index = 0;
            

            ConfigureScenarioSteps();
            foreach (var state in simulationStates)
            {
                state.ConfigureGrabbable();
                state.ConfigureDetects(index);
               
                foreach(var detect in state.objectToDetectList)
                {
                    detect.detectObject.SetupWrongDetects(state.transform);
                }
                index++;
            }
            StartSimulation();
        }

        private void StartSimulation()
        {
            currentState = simulationStates[currentStateIndex];
            PlayState();
        }

        #endregion


        #region // ScenarioSteps //
      
        public void SelectDefaultScenario(int ScenarioNumber)
        {
            if (AddScenarios.Count >= ScenarioNumber)
            {
              foreach (var scenario in AddScenarios[ScenarioNumber].AlternateScenarioStates)
              {
                /*if (simulationStates.Contains(scenario)|| (isAssessmentMode && !scenario.playStateInAssessmentMode))
                 {
                   continue;
                 }*/
                    simulationStates.Remove(scenario);
                    scenario.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("The 'scenariocount' Int exceeds the total available scenarios. Please check if the list is populated and int passed is correct");
            }
            
        }
       
        public void SelectAlternateScenario(int ScenarioNumber)
        {//Int Scenario number is the exact number/Index in List i.e If scenario is 1st in list its index is 0 so here the ScenarioNumber Passed will Also be 0

            if (AddScenarios.Count >= ScenarioNumber)
            {    //If the Default Bool Is false We will Add the Alternate Steps in the State List
                    foreach (var scenario in AddScenarios[ScenarioNumber].DefaultScenarios)
                    {
                      /*  if (simulationStates.Contains(scenario) || (isAssessmentMode && !scenario.playStateInAssessmentMode))
                        {
                            continue;
                        }*/
                        simulationStates.Remove(scenario);
                        scenario.gameObject.SetActive(false);

                    }         
            }
            else
            {
                Debug.LogError("The 'scenariocount' Int exceeds the total available scenarios. Please check if the list is populated and int passed is correct");
            }

        }


        void ConfigureScenarioSteps()
        {
            foreach(var scenarios in AddScenarios)
            {
                var index = 0;
                foreach (var DefaultSteps in scenarios.DefaultScenarios)
                {
                    DefaultSteps.AddListners();
                    DefaultSteps.ConfigureGrabbable();
                    DefaultSteps.ConfigureDetects(index);
                    index++;
                }
                var index2 = 0;
                foreach (var AlternateSteps in scenarios.AlternateScenarioStates)
                {
                    AlternateSteps.AddListners();
                    AlternateSteps.ConfigureGrabbable();
                    AlternateSteps.ConfigureDetects(index2);

                    index2++;
                }
            }

        }
        #endregion
    }
   

    [Serializable]
    public class Scenarios
    {
        public List<SimulationState> DefaultScenarios;
        public List<SimulationState> AlternateScenarioStates;
    }

    public enum DetectVizmode
    {
        Renderer, FirstChild
    }

}