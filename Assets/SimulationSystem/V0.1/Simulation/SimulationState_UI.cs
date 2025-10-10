using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine;
using SimulationSystem.V0._1.UI;
namespace SimulationSystem.V0._1.Simulation
{
    public partial class SimulationState : MonoBehaviour
    {

        public UIAnimationHandler Hintbutton;
        public UIAnimationHandler Prompthlder;
        private void SetupStateUI()
        {
            onStateStart.AddListener(() =>
            {
                uiParentAnimationHandler.OnDetectOnce();

                UIButtonComponent.onClick.AddListener(() =>
                {
                    UIStateExit();
                });
            });
        }
        public void UIStateExit()
        {

            if (SimulationManager.instance.isAssessmentMode)
            {
                if (SimulationStatePromptManager.HasStateAudioEnded)
                {
                    SimulationManager.instance.NextState();
                    uiParentAnimationHandler.OnUnDetected();
                   // buttonPokeInteractable.enabled = false;

                }
            }
            else
            {
                if (SimulationManager.instance.stateChangeOnlyAfterPromptIsOver)
                {
                    if (SimulationStatePromptManager.HasStateAudioEnded)
                    {
                        SimulationManager.instance.NextState();
                        uiParentAnimationHandler.OnUnDetected();
                      //  buttonPokeInteractable.enabled = false;

                    }
                }
                else
                {
                    SimulationManager.instance.NextState();
                    uiParentAnimationHandler.OnUnDetected();
                  //  buttonPokeInteractable.enabled = false;

                }
            }

        }
        public void AssessmentStatepromptEnable()
        {

            if (SimulationManager.instance.isAssessmentMode && stateType == StateType.UI && transform.GetComponent<Assessment.AssessmentController>() == null)
            {
                SimulationManager.instance.promptHolder.TryGetComponent<UIAnimationHandler>(out Prompthlder);
                SimulationManager.instance.hintButton.TryGetComponent<UIAnimationHandler>(out Hintbutton);
                if (this.stateType == StateType.UI)
                {
                    Prompthlder.ScaleDown();
                    Hintbutton.ScaleDown();
                }
            }
            else
            {
                if (transform.GetComponent<Assessment.AssessmentController>() != null)
                {
                    SimulationManager.instance.promptHolder.TryGetComponent<UIAnimationHandler>(out Prompthlder);
                    SimulationManager.instance.hintButton.TryGetComponent<UIAnimationHandler>(out Hintbutton);
                    if (!Prompthlder)
                    {
                        Prompthlder = SimulationManager.instance.promptHolder.GetComponentInChildren<UIAnimationHandler>();
                    }
                    if (!Hintbutton)
                    {
                        Hintbutton = SimulationManager.instance.hintButton.GetComponentInChildren<UIAnimationHandler>();
                    }
                    if (SimulationManager.instance.isAssessmentMode && playStateInAssessmentMode)
                    {
                        Prompthlder.ScaleDown();
                        Hintbutton.ScaleUp();

                    }


                }
                else if (SimulationManager.instance.isAssessmentMode && playStateInAssessmentMode)
                {
                    SimulationManager.instance.promptHolder.TryGetComponent<UIAnimationHandler>(out Prompthlder);
                    SimulationManager.instance.hintButton.TryGetComponent<UIAnimationHandler>(out Hintbutton);
                    Prompthlder.ScaleUp();
                    Hintbutton.ScaleDown();

                    if (this.stateType == StateType.UI)
                    {
                        Prompthlder.ScaleDown();
                        Hintbutton.ScaleDown();
                    }

                }

                if (!SimulationManager.instance.isAssessmentMode)
                {
                    if ((this.stateType == StateType.UI))
                    {
                        SimulationManager.instance.promptHolder.TryGetComponent<UIAnimationHandler>(out Prompthlder);
                        SimulationManager.instance.hintButton.TryGetComponent<UIAnimationHandler>(out Hintbutton);
                        Prompthlder.ScaleDown();
                        Hintbutton.ScaleDown();
                    }
                    else
                    {
                        SimulationManager.instance.promptHolder.TryGetComponent<UIAnimationHandler>(out Prompthlder);
                        SimulationManager.instance.hintButton.TryGetComponent<UIAnimationHandler>(out Hintbutton);
                        Prompthlder.ScaleUp();
                        Hintbutton.ScaleDown();
                    }
                }
            }
        }
        public void AssessmentStatePromptDisabe()
        {
            if (this.stateType == StateType.UI)
            {
                Prompthlder.ScaleDown();
                Hintbutton.ScaleDown();
            }
            else
            {
                Prompthlder.ScaleUp();
                Hintbutton.ScaleDown();
            }
        }
    }
}









