using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class ResettableItem : MonoBehaviour
    {
       /* [Header("Delay to begin Position Reset (if any)")]
        [Tooltip("in Seconds")]
        [SerializeField] private int _resetDelay = 1;

        [Header("Targets to reset to")]
        [Space(5)]
        [Tooltip("Can be added as Children to GameObject, Transforms Only.")]
        [SerializeField] private List<Vector3> _destinations = new();
        [SerializeField] private List<Transform> _targets = new();
        private Vector3 currentvelocity;

        [Header("Unity Event fired after completion")]
        [Space(5)]
        [SerializeField] private UnityEvent onDestinationReached;

        private int index = 0;
        private PointableUnityEventWrapper _event;
        private float speed = 0.3f;
        private Grabbable _parent;
        private bool _isGrabbed;
        private Quaternion _rotation;
        private float lerpAmount;
        private CancellationTokenSource _cancellation;
        private bool _setupDone;
    
        #region References cached on Awake and Setting up Events
    
        private void OnEnable()
        {
            SetupObject();
        }
    
        private void OnDisable()
        {
            _cancellation?.Cancel();
            DeRegisterObject();
        }

        public void DeRegisterObject()
        {
            _event.WhenUnselect.RemoveListener(delegate 
            {
                BeginReset();
            });
            _event.WhenSelect.RemoveListener(delegate
            {
                Grabbed();
            });
            _event.WhenUnselect.RemoveListener(delegate
            {
                Released();
            });
        }
    
        private void SetupObject()
        {
            if (_setupDone) return;
        
            _parent = GetComponent<Grabbable>();
            _cancellation = new CancellationTokenSource();
            if (_parent != null)
            {
                _event = _parent.gameObject.GetComponent<PointableUnityEventWrapper>();
                _destinations.Add(transform.position);
                _rotation = _parent.transform.rotation;

                if (_event != null)
                {
                    _event.WhenUnselect.AddListener(delegate 
                    {
                        BeginReset();
                    });
                    _event.WhenUnselect.AddListener(delegate
                    {
                        Released();
                    });
                    _event.WhenSelect.AddListener(delegate 
                    {
                        Grabbed();
                    });
                }
            }

            if (_targets.Count > 0)
            {
                foreach (Transform child in _targets)
                {
                    _destinations.Add(child.position);
                }
            }

            _setupDone = true;
        }

        void BeginReset()
        {
            ResetPositionAsync(index);
        }
    
        #endregion

        #region Async Lerp Code for position reset
        /// <summary>
        /// Lerps Object to Destination position asynchronously
        /// </summary>
        /// <param name="destination">Target to Lerp Towards</param>
        /// <returns>No return value</returns>
        async Task ResetToDestination(Vector3 destination, CancellationToken token)
        {
            await Task.Delay(_resetDelay * 1000);

            while (transform.position != destination)
            {
                if (!_isGrabbed)
                {
                    lerpAmount = Mathf.Clamp01(lerpAmount += Time.deltaTime);
                    _parent.gameObject.transform.position = Vector3.SmoothDamp(_parent.gameObject.transform.position, destination, ref currentvelocity, speed);
                    _parent.transform.rotation = Quaternion.Lerp(_parent.transform.rotation, _rotation, lerpAmount * 0.1f);

                    await Task.Yield();

                    if(_cancellation.IsCancellationRequested)
                    {
                        
                    }
                }

                else
                {
                    break;
                }
            }

            if (_cancellation.IsCancellationRequested)
            {
              
            }
        }

        /// <summary>
        /// Iterates through list of positions awaiting for each
        /// </summary>
        /// <param name="positionIndex"></param>
        public async void ResetPositionAsync(int positionIndex)
        {
            for (int i = 0; i < 1; i++)
            {
                //Awaits Reset of position for the Item
                await ResetToDestination(_destinations[positionIndex], _cancellation.Token);

                lerpAmount = 0;
                onDestinationReached?.Invoke();
            }
        }

        public void Grabbed()
        {
            _isGrabbed = true;
        }

        public void Released()
        {
            _isGrabbed = false;
        }

        #endregion*/
    
    }
}