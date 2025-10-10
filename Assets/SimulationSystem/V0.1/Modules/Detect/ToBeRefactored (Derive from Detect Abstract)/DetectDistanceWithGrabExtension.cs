using System.Collections;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_
{
    /*[System.Obsolete ("This class has been refactored with DetectDistanceExtension")]
    [RequireComponent(typeof(DetectDistance), typeof(PointableUnityEventWrapper), typeof(DetectDistanceExtension))]
    public class DetectDistanceWithGrabExtension : MonoBehaviour
    {
        private DetectDistance _detectDistance;
        private PointableUnityEventWrapper _eventWrapper;
        private DetectDistanceExtension _extensionEventWrapper;

        private bool _isGrabbed = false;
        private bool _isInRange = false;

        [SerializeField] private UnityEvent onDetectOnceWithGrab;
        [SerializeField] private UnityEvent<float> onDetectingWithGrab;
        [SerializeField] private UnityEvent onDetectCompleteAndRelease;
        [SerializeField] private UnityEvent onDetectCompleteWithInRangeAndRelease;


        private Coroutine _coroutine;

        private void Start()
        {
            _detectDistance = GetComponent<DetectDistance>();
            _eventWrapper = GetComponent<PointableUnityEventWrapper>();
            _extensionEventWrapper = GetComponent<DetectDistanceExtension>();

        
            _extensionEventWrapper.onIdleDetect.AddListener(() => _isInRange = true);
            _extensionEventWrapper.onLessThenIdleDetect.AddListener(() => _isInRange = false);
        
            _eventWrapper.WhenSelect.AddListener((arg0 => _isGrabbed = true));
            _eventWrapper.WhenRelease.AddListener((arg0 =>
            {
                _isGrabbed = false;
                if (_isInRange)
                    StartCoroutine(OnDetectCompleteWithInDetectionEnumerator());
            }));
        
            _detectDistance.OnDetect.AddListener(OnDetectOnce);
            _detectDistance.OnDetectingEvent.AddListener(OnDetecting);
            _detectDistance.OnDetectComplete.AddListener(OnDetectComplete);
        }

        private void OnDetectOnce()
        {
            StartCoroutine(OnDetectOnceEnumerator());
        }

        private void OnDetecting(float value)
        {
            if (_isGrabbed)
            {
                onDetectingWithGrab?.Invoke(value);
            }
        }

        private void OnDetectComplete()
        {
            StartCoroutine(OnDetectCompleteAndReleaseEnumerator());
        }
    
        private void OnDetectCompleteWithInDetection()
        {
            _coroutine = StartCoroutine(OnDetectCompleteWithInDetectionEnumerator());
        }
    

        private IEnumerator OnDetectOnceEnumerator()
        {
            yield return new WaitUntil(() => _isGrabbed);
            onDetectOnceWithGrab?.Invoke();
        }
    
        private IEnumerator OnDetectCompleteAndReleaseEnumerator()
        {
            yield return new WaitUntil(() => _isGrabbed);
            yield return new WaitUntil(() => !_isGrabbed);
            onDetectCompleteAndRelease?.Invoke();
        }
    
        private IEnumerator OnDetectCompleteWithInDetectionEnumerator()
        {
            yield return new WaitUntil(() => !_isGrabbed);
            yield return new WaitUntil(() => _isInRange);
            onDetectCompleteWithInRangeAndRelease?.Invoke();
        }
    }*/
}
