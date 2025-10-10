using DG.Tweening;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;

namespace SimulationSystem.V0._1.UI
{
    public class UIAnimationHandler : MonoBehaviour, IUIDetect
    {
        public bool IsForPrompts;
        public bool IsActive { get; private set; } = false;

        [SerializeField] private float animationTime = 3;
        [SerializeField] private Vector3 hoverScale = Vector3.one;
        [SerializeField] private Vector3 unHoverScale = Vector3.zero;
    
  
        [SerializeField] private Transform uiTransform;

        private DetectStates _states = DetectStates.Normal;
        private LabelStatus _status = LabelStatus.None;

        public void OnDetectOnce() => OnDetect();
        public void OnDetecting(){}
        public void OnDetecting(float value) {}
        public void OnDetectingFinished() {}
        public void OnUnDetected() => OnRemove();
    
        private void OnDetect()
        {
            _states = DetectStates.Detect;
        }
        private void OnRemove()
        {
            _states = DetectStates.UnDetect;
        }
        private void Update()
        {
            
                if (_states == DetectStates.Detect && _status == LabelStatus.None)
                {
                    if (!uiTransform)
                    {
                        this.transform.AnimatePop(hoverScale, animationTime).OnComplete(delegate
                        {
                            //IsActive = true;
                        });
                    }
                    else
                    {
                        uiTransform.transform.AnimatePop(hoverScale, animationTime).OnComplete(delegate
                        {
                            //IsActive = true;
                        });
                    }

                    _status = LabelStatus.Show;
                }
                else if (_states == DetectStates.UnDetect && _status == LabelStatus.Show)
                {
                    if (!uiTransform)
                    {
                        this.transform.AnimatePop(unHoverScale, animationTime).OnComplete(delegate
                        {
                            _states = DetectStates.Normal;
                            _status = LabelStatus.None;
                        });
                    }
                    else
                    {
                        uiTransform.transform.AnimatePop(unHoverScale, animationTime).OnComplete(delegate
                        {
                            _states = DetectStates.Normal;
                            _status = LabelStatus.None;
                        });
                    }

                    _status = LabelStatus.Hide;
                }
                else if (_states == DetectStates.Normal && _status == LabelStatus.Hide)
                {
                    _status = LabelStatus.None;
                }
            
           
        }

        public void ForceRemove()
        {
            if (!uiTransform)
                this.transform.AnimatePop(unHoverScale, animationTime).OnComplete(delegate
                {
                    Invoke(nameof(DisableDetection), 0.3f);
                });
            else
                uiTransform.transform.AnimatePop(unHoverScale, animationTime).OnComplete(delegate
                {
                    Invoke(nameof(DisableDetection), 0.3f);
                });
        }
        public void DisableDetection()
        {
            //isFirstTrigger = false;
            IsActive = true;
        }

        #region PromptScreenFunctions
        public void ScaleUp()
        {                      
            this.transform.AnimatePop(hoverScale, animationTime);            
        }
        public void ScaleDown()
        { 
           this.transform.AnimatePop(unHoverScale, animationTime);
        }

        #endregion

    }
}
