using System.Collections;
using DG.Tweening;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.Modules.Grab.Utility
{
    public class FloatToDestination : MonoBehaviour
    {
        private bool _canTrigger = true;
        public bool _isGrabbed;
        private float _distanceBtwVectors = 0f;
        private Vector3 _initialPos = Vector3.zero;
        private Quaternion _initialRot = Quaternion.identity;
        private IEnumerator _coroutine;
        public Vector3 InitialPos => _initialPos; //Properties
        public Quaternion InitialRot => _initialRot; //Properties

        [SerializeField] private Transform destination;
        [SerializeField] private Transform moveTransform;
        [SerializeField] private float animationSpeedModifier = 2f;

        [Header("Parent Position")]
        [SerializeField] private Transform parentTransform;

        private GameObject _newParent;

        [Header("Unity Event")] [Space(5)]
        public UnityEvent onDestinationReached;

        private void Awake()
        {
            SetupPU();
        }

        public void SetupPU()
        {
            /*_pointableUnityEventWrapper = GetComponent<PointableUnityEventWrapper>();

            if (_pointableUnityEventWrapper == null) return;
            
            _pointableUnityEventWrapper.WhenSelect.AddListener(arg0 =>
            {
                AnimateStop(arg0);  
                _isGrabbed = true;
            });
        
            _pointableUnityEventWrapper.WhenUnselect.AddListener(arg0 =>
            {
                _isGrabbed = false;
            });*/
        }

        public void Start()
        {
            if (!moveTransform)
                moveTransform = this.transform;
    
            if (destination == null)
            {
                _initialPos = moveTransform.localPosition;
                _initialRot = moveTransform.localRotation;
            }
            else
            {
                _initialPos = destination.localPosition;
                _initialRot = destination.localRotation;
            }

            if (parentTransform)
            {
                if (parentTransform.TryGetComponent(out FloatToDestination value))
                {
                    _initialPos = value.InitialPos;
                    _initialRot = value.InitialRot;
                }
            }

            GameObject newParentTransform = new GameObject();
            _newParent = newParentTransform;
            _newParent.transform.SetParent(this.transform.parent);
            _newParent.name = this.gameObject.name + " Destination Parent";
            _newParent.transform.localPosition = _initialPos;
            _newParent.transform.localRotation = _initialRot;
    
            _coroutine = AnimateAfterDelay();
            SubscribeToEvent();
        }

        public void AnimateStop( )
        {
            DOTween.Pause(moveTransform);
            StopCoroutine(_coroutine);
        }
        
        public void AnimateMove( )
        {
            _coroutine = AnimateAfterDelay();
            StartCoroutine(_coroutine);
        }

        private IEnumerator AnimateAfterDelay()
        {
            yield return new WaitForSeconds(5);
            yield return new WaitUntil((() => !_isGrabbed));
            DOTween.Play(moveTransform);
            yield return new WaitUntil((() => _canTrigger));
    
            _canTrigger = false;                                                                                                        
    
            moveTransform.DistanceFrom(_newParent.transform.localPosition, 0.4f, out _distanceBtwVectors);
    
            var animationTime = (_distanceBtwVectors / animationSpeedModifier);
            moveTransform.DOLocalMove(_newParent.transform.localPosition, animationTime).OnComplete(delegate
            {
                _canTrigger = true;
                onDestinationReached?.Invoke();
            });
            moveTransform.DOLocalRotateQuaternion(_newParent.transform.localRotation, animationTime);
        }

        public void SetIsGrabbedBool(bool isGrabbed)
        {
            _isGrabbed = isGrabbed;
        }

        public void SubscribeToEvent()
        {
            GetComponent<Button>().onClick.AddListener(AnimateMove);
            GetComponent<Button>().onClick.AddListener(AnimateStop);
        }
        public void UnsubscribeToEvent()
        {
            GetComponent<Button>().onClick.RemoveListener(AnimateMove);
            GetComponent<Button>().onClick.RemoveListener(AnimateStop);
        }
    }
}
