using System;
using System.Collections.Generic;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Utility.Event;
using SimulationSystem.V0._1.Utility.Miscellanous;
using UnityEngine;
using UnityEngine.Events;
/*
using Timer = SimulationSystem.V0._1.Utility.Miscellanous.Timer;
using WrongTimer = SimulationSystem.V0._1.Utility.Miscellanous.WrongTimer;*/
namespace SimulationSystem.V0._1.Modules.Detect
{
    public abstract class DetectAbstract : MonoBehaviour
    {

        #region Variable declaration
        [Header("Position Overrides")]
       [Space(3f)] public bool DontDoAnything;
        public ObjectBehaviourOnDetect ObjectBehaviourOnDetect;
        public Transform DestinationTransform;

        [Space(5f)]
        [Header("Grabbable And Reset Overrides")]
        [Space(3f)]

        public bool deRegisterThisObjectOnDetect;
        public bool ForceUngrabThisObject;

        private List<ObjectMovementHelper> _objectMovementHelper = new List<ObjectMovementHelper>();

        [HideInInspector]public bool hintIsTaken;
        private float _timers;
        private Timer _timer;
        private WrongTimer _Wrongtimer;
        private bool _canDetect;

        [Space(5f)]
        [Header("Detect Timer Control")]
        [Space(3f)]
        [SerializeField] public bool resetTimerOnDetectionStop;
        public float detectTimer;
        
        [SerializeField] private UIImageHandler progressFillImage;
        private UIAnimationHandler _progressFillImageAnimationHandler;
        
        public UnityEvent onDetectReset;
        public UnityEvent onDetected;
        public UnityEvent<float> onDetectingEvent;
        public UnityEvent onUnDetect;
        public UnityEvent onDetectionComplete;
    
        public List<DelayedEvent> delayedEventsOnDetectionComplete;
        
        [NonSerialized] public DetectStates state = DetectStates.Normal;

       
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            SetupTimer();
            SetupWrongTimer();
            if(progressFillImage != null) _progressFillImageAnimationHandler = progressFillImage.GetComponent<UIAnimationHandler>();
            onDetectionComplete.AddListener(CheckThisObjectResetBehaviour);
        }

        #endregion

        #region Toggle detection

        public virtual void EnableDetection()
        {
            _canDetect = true;
            foreach (var VARIABLE in SimulationManager.instance.currentState.stateGrabbables)
            {
                _objectMovementHelper.Add((VARIABLE.transform.GetComponent<ObjectMovementHelper>()));
            }

        }
        public void DisableDetection()
        {
            _canDetect = false;
            state = DetectStates.Normal;
            _timer.StopTimer(true);
            _Wrongtimer.StopTimer(true);
        }

        #endregion

        #region Detect Methods

        protected virtual void ResetDetect()
        {
            DisableDetection();
            onDetectReset.Invoke();
        }
        
        protected virtual void ResetWrongDetect()
        {
            DisableDetection();
            onDetectReset.Invoke();
        }
        protected virtual void Initialize()
        {
        }

        protected void OnDetectionStart()
        {
            if (!_canDetect) return;
            
            state = DetectStates.Detect;
            onDetected.Invoke();
            _timer.StartTimer();


            if (progressFillImage == null) return;
            
            progressFillImage.OnDetectOnce();
            _progressFillImageAnimationHandler.OnDetectOnce();
        }

        protected void OnWrongDetectionStart()
        {
            state = DetectStates.Detect;
            onDetected.Invoke();
            _Wrongtimer.StartTimer();

            if (progressFillImage == null) return;

            progressFillImage.OnDetectOnce();
            _progressFillImageAnimationHandler.OnDetectOnce();
        }

        protected void OnDetectionStop()
        {
            if (!_canDetect) return;
         
            state = DetectStates.UnDetect;
            if (resetTimerOnDetectionStop) ResetDetect();
            _timer.StopTimer(resetTimerOnDetectionStop);
            onUnDetect.Invoke();
            
          //  if (progressFillImage != null) _progressFillImageAnimationHandler.OnUnDetected();
        }

        protected void OnWrongDetectionStop()
        {
            state = DetectStates.UnDetect;
            /*if (resetTimerOnDetectionStop) ResetDetect();*/
            _Wrongtimer.StopTimer(resetTimerOnDetectionStop);
            onUnDetect.Invoke();

           // if (progressFillImage != null) _progressFillImageAnimationHandler.OnUnDetected();
        }
        
        private void SetupWrongTimer()
        {
            _Wrongtimer = new WrongTimer(detectTimer);

            _Wrongtimer.OnTimerRunning += timerProgress =>
            {
                onDetectingEvent.Invoke(timerProgress);
                if (progressFillImage != null) { 
                    progressFillImage.OnDetecting(timerProgress);
                    progressFillImage.GetComponent<UITextHandler>().OnDetecting(((1 - timerProgress) * detectTimer).ToString("0"));
                }

            };

            _Wrongtimer.OnTimerEnd += () =>
            {
                if(GameManager.Instance.AudioManager != null) GameManager.Instance.AudioManager.PlayDetectEffect();

                onDetectionComplete.Invoke();
                if (progressFillImage)
                {
                    progressFillImage.OnDetecting(1);
                }
                foreach (var unityEvent in delayedEventsOnDetectionComplete)
                {
                    unityEvent.InvokeDelayedEvent();
                }
                
                ResetDetect();
            };
        }
        private void SetupTimer()
        {
            _timer = new Timer(detectTimer);

            _timer.OnTimerRunning += timerProgress =>
            {
                onDetectingEvent.Invoke(timerProgress);
                if (progressFillImage != null)
                {
                    progressFillImage.OnDetecting(timerProgress);
                    progressFillImage.GetComponent<UITextHandler>().OnDetecting(((1 - timerProgress) * detectTimer).ToString("0"));
                }

            };

            _timer.OnTimerEnd += () =>
            {
                if (GameManager.Instance.AudioManager != null) GameManager.Instance.AudioManager.PlayDetectEffect();

                onDetectionComplete.Invoke();
                if (progressFillImage)
                {
                    progressFillImage.OnDetecting(1);
                }
                foreach (var unityEvent in delayedEventsOnDetectionComplete)
                {
                    unityEvent.InvokeDelayedEvent();
                }

                ResetDetect();
            };
        }
        private void CheckThisObjectResetBehaviour()
        {
               
            if (ForceUngrabThisObject)
            {
                    foreach (var tempGrabbable in SimulationManager.instance.currentState.stateGrabbables)
                    {
                        tempGrabbable.ForceUnGrab();            
                    }
            }
            
            if (ObjectBehaviourOnDetect == ObjectBehaviourOnDetect._MoveObjectToDetectPos)
                {
                   
                    foreach (var tempObjMovHelper in _objectMovementHelper)
                    {
                        if (deRegisterThisObjectOnDetect)
                        {
                            tempObjMovHelper.DeregisterResettable();
                        }

                    if (!DontDoAnything)
                    {
                        tempObjMovHelper.ForceUpdateResettablePos(DestinationTransform);
                        tempObjMovHelper.ForceResetNow();
                    }

                        if (SimulationManager.instance.GrabbableShouldNotResetInItsRespectiveStep)
                        {
                            if (SimulationManager.instance.isAssessmentMode)
                            {
                                tempObjMovHelper.ResetThisObjectOnRelease = true;
                                tempObjMovHelper.ForceUpdateResettablePos(DestinationTransform);

                                SimulationManager.instance.currentState.onStateComplete.AddListener(() => tempObjMovHelper.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true);
                            }
                        }
                        

                    }
                }
            
            else
            {
                    
                foreach (var VARIABLE in _objectMovementHelper)
                {
                    if (!DontDoAnything)
                    {
                        VARIABLE.ForceResetNow();
                    }
                        
                    if (deRegisterThisObjectOnDetect)
                    {
                        VARIABLE.DeregisterResettable();
                    }

                    if (SimulationManager.instance.isAssessmentMode)
                    {
                        foreach (var tempGrabbable in SimulationManager.instance.currentState.stateGrabbables)
                        {

                            if (!SimulationManager.instance.simulationStates[SimulationManager.instance.currentStateIndex + 1].IsGrabbableUsedInNextState(tempGrabbable))
                            {
                                VARIABLE.ResetThisObjectOnRelease = true;
                            }

                        }

                        if (!DontDoAnything)
                            VARIABLE.ForceResetNow();
                        {
                            SimulationManager.instance.currentState.onStateComplete.AddListener(() => VARIABLE.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true);
                        }
                    }
                }
                
            }

            
        }
        


        #endregion



    }

}
public enum ObjectBehaviourOnDetect
{
    _MoveObjectBackToOriginalPos, _MoveObjectToDetectPos
}