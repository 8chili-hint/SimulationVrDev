using SimulationSystem.V0._1.Assessment.Interface;
using UnityEngine;
using System.Collections.Generic;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Simulation;
using System.Linq;
using SimulationSystem.V0._1.Utility.Extensions;
using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.Assessment.Assessment_Types
{
    [RequireComponent(typeof(AssessmentController))]
    public class UIAssessment : MonoBehaviour, IAssessment
    {
        [Header(("Override for Correct UI"))] public Button ButtonOverrideForcorrectUI;
        private UIAnimationHandler CorrectUi;
        public List<UIAnimationHandler> WrongUi;
        public bool ShouldRandomizeUi;
        public UnityEvent onWrongUITaken;
        [field: SerializeField] public float NegativeMarks { get; set; }
        [field: SerializeField] public AssessmentStatus AssessmentResultStatus { get; set; }
        public string ErrorMessage { get; set; }

        private List<TransfromwithoutScale> UiTransforms = new List<TransfromwithoutScale>();

        [Space(5f)]
        [Header("Overrides")]
        public string thisAssessmentText;


        public void RandomizeUI()
        {
            if (ShouldRandomizeUi)
            {
                UiTransforms.Shuffle();


                for (int i = 0; i < UiTransforms.Count; i++)
                {
                    if (i == 0)
                    {
                        CorrectUi.transform.position = UiTransforms[i].Postion;
                        CorrectUi.transform.rotation = UiTransforms[i].Rotation;
                        continue;
                    }

                    WrongUi[i - 1].transform.position = UiTransforms[i].Postion;
                    WrongUi[i - 1].transform.rotation = UiTransforms[i].Rotation;
                }
            }

        }
        public void DisplayWrongUI()
        {
            foreach (UIAnimationHandler uah in WrongUi)
            {
                uah.OnDetectOnce();
            }

        }

        public void WrongUISelected()
        {
            foreach (UIAnimationHandler UIanim in WrongUi)
            {
                UIanim.OnUnDetected();
            }
            AssessmentManager.DeductScore(AssessmentType.UI);
            onWrongUITaken.Invoke();
        }

        private void SetupwrongCallbacks()
        {
            foreach (var wrongUI in WrongUi)
            {
                wrongUI.TryGetComponent<Button>(out Button a);
                if (a) { a.onClick.AddListener(() => WrongUISelected()); }

                else
                {
                    wrongUI.GetComponentInChildren<Button>().onClick.AddListener(() => WrongUISelected());

                }
            }
            GetComponent<AssessmentController>().OnHintTaken.AddListener(() =>
            {
                foreach (UIAnimationHandler UIanim in WrongUi)
                {
                    UIanim.OnUnDetected();
                }
            });
        }

        private void Awake()
        {
            if (thisAssessmentText != "")
            {
                ErrorMessage = thisAssessmentText;
            }
            else ErrorMessage = "Wrong UI was selected";

            if (ButtonOverrideForcorrectUI)
            {
                CorrectUi = ButtonOverrideForcorrectUI.GetComponent<UIAnimationHandler>();

                //adding listener
                ButtonOverrideForcorrectUI.onClick.AddListener(() =>
                {
                    if (SimulationStatePromptManager.HasStateAudioEnded && SimulationManager.instance.isAssessmentMode)
                    {

                        SimulationManager.instance.NextState();
                        ButtonOverrideForcorrectUI.GetComponent<Button>().enabled = false;

                    }

                });

            }
            else
            {
                CorrectUi = GetComponent<SimulationState>().uiParentAnimationHandler;
            }
            if (GetComponent<SimulationState>().stateType == SimulationState.StateType.UI)
            {
                TransfromwithoutScale tws = new TransfromwithoutScale();
                tws.Postion = CorrectUi.transform.position;
                tws.Rotation = CorrectUi.transform.rotation;
                UiTransforms.Add(tws);

                foreach (UIAnimationHandler uah in WrongUi)
                {
                    tws.Postion = uah.transform.position;
                    tws.Rotation = uah.transform.rotation;

                    UiTransforms.Add(tws);
                }
            }

            RandomizeUI();
            SetupwrongCallbacks();
        }
    }
}

struct TransfromwithoutScale
{
    public Vector3 Postion;
    public Quaternion Rotation;
}