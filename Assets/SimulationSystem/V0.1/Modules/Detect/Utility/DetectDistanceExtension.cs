
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using SimulationSystem.V0._1.UI;
using UnityEngine;
using UnityEngine.Events;

//Total Refactor to detectdistance
namespace SimulationSystem.V0._1.Modules.Detect.Utility
{
 /*   [RequireComponent(typeof(DetectDistance),(typeof(PointableUnityEventWrapper)))]
    public class DetectDistanceExtension : MonoBehaviour
    {
        private enum IdleState
        {
            None,
            OnIdle,
            LessIdle,
            MoreIdle,
        }
    
        [SerializeField] private UITextHandler textHandler;
        [SerializeField] private float minDis;
        [SerializeField] private float minIdleDis;
        [SerializeField] private float idleDis;
        [SerializeField] private float maxDis;

        [Header("[DetectIdle]")]
        [Space(10)]
        public UnityEvent onIdleDetect;
        public UnityEvent onLessThenIdleDetect;
        public UnityEvent onMoreThenIdleDetect;

        [Header("[DetectWithGrab]")] 
        [Space(10)]
        public UnityEvent onDetectWithGrab;
        public UnityEvent onDetectAfterRelease;
        public UnityEvent onDetectIdleAfterRelease;

        private float _currentDistance;
        private DetectStates _states;
        private PointableUnityEventWrapper _eventWrapper;

        private void Start()
        {
            if (TryGetComponent<DetectDistance>(out var detectDistance))
            {
                detectDistance.OnDetectingFullTravel.AddListener(UpdateDistanceText);
            }

            _eventWrapper = GetComponent<PointableUnityEventWrapper>();
        
            _eventWrapper.WhenMove.AddListener(OnMoveDetect);
            _eventWrapper.WhenRelease.AddListener(OnReleaseDetect);
        }

        private IdleState CheckIdleState()
        {
            if (_currentDistance > minIdleDis && _currentDistance <= idleDis)
            {
                return IdleState.OnIdle;
            }
        
            if (_currentDistance <= minIdleDis)
            {
                return IdleState.LessIdle;
            }
        
            if (_currentDistance >= maxDis)
            {
                return IdleState.MoreIdle;
            }

            return IdleState.None;
        }

        public void OnMoveDetect(PointerEvent arg0)
        {
            if (CheckIdleState() == IdleState.MoreIdle && _states == DetectStates.Normal)
            {
                onMoreThenIdleDetect?.Invoke();
                _states = DetectStates.Detect;
            }
            else if(CheckIdleState() == IdleState.OnIdle && _states == DetectStates.Detect)
            {
                onIdleDetect?.Invoke();
                _states = DetectStates.UnDetect;
            }
            else if(_states == DetectStates.UnDetect)
            {
                _states = DetectStates.Normal;
            }
        }

        public void OnReleaseDetect(PointerEvent arg0)
        {
            if (CheckIdleState() == IdleState.LessIdle)
            {
                onLessThenIdleDetect?.Invoke();
            }

            if (CheckIdleState() == IdleState.OnIdle || CheckIdleState() == IdleState.MoreIdle)
            {
                onDetectIdleAfterRelease?.Invoke();
            }
        }

        private void UpdateDistanceText(float value)
        {
            _currentDistance =  Mathf.Lerp(minDis, maxDis, value);
            var text = _currentDistance.ToString("0.0") + "cm";
            textHandler.UpdateText(text);
        }
    }*/
}
