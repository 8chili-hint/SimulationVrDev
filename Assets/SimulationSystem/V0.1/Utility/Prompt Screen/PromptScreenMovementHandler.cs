using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Prompt_Screen
{
    public class PromptScreenMovementHandler : MonoBehaviour
    {
        [SerializeField] private Transform _transformHolder;
        [SerializeField] private List<Transform> _transformList = new List<Transform>();
        [SerializeField] private float _resetDelay = 1.0f;
        
        private Vector3 currentvelocity;
        private float speed = 0.3f;
        private IEnumerator  _lerp;
        [SerializeField] private int _moveToIndex;
        private Vector3 originalPos;

        [Space] [Header("Follow Behavior")] 
        [SerializeField] private bool willFollowAnchor;
        [SerializeField] private bool flip180;
        [SerializeField] private Transform anchorTransform;
        [SerializeField] private Transform camTransform;

        [Space(10f)]
        [Header("Follow Behaviour Overrides")]
        //Added
        [SerializeField] private bool UseThreshold;
        [SerializeField] private bool continousLookAtCam;

        //Vec 3 can be used later instead of float for a better control
        [SerializeField] private float ThresholdValue;
        private void Awake()
        {
            _resetDelay = 1.0f;

            foreach (Transform child in _transformHolder)
            {
                if(child.gameObject.activeSelf)
                {
                    _transformList.Add(child);
                }
            }
        }
        
        private void LateUpdate()
        {
            if (willFollowAnchor)
            {
                //added
                if (!UseThreshold)
                {
                    transform.DOMove(anchorTransform.position, 0.1f);
                    LookAt();
                }
                //added
                else
                {
                    if (continousLookAtCam)
                    {
                        if (Vector3.Distance(anchorTransform.position, transform.position) > ThresholdValue)
                            transform.DOMove(anchorTransform.position, 0.1f);
                        
                        LookAt();
                    } 
                    else
                    {
                        if (Vector3.Distance(anchorTransform.position, transform.position) > ThresholdValue)
                        {
                            transform.DOMove(anchorTransform.position, 0.1f);
                            LookAt();
                        }
                    }
                }
            }
        }

        public void LookAt()
        {
            if(flip180)
            {
                transform.LookAt(camTransform.position);
                transform.Rotate(0, 180, 0);
            }
            else
                transform.LookAt(camTransform);
        }

        [ContextMenu("Move to Position")]
        public void MoveTo()
        {
            originalPos = transform.position;
            MoveToPosition(_moveToIndex);
        }

        [ContextMenu("Reset Position")]
        public void ResetPositon()
        {
            if(originalPos != null)
            {
                transform.position = originalPos;
            }
        }
    
        public void MoveToPosition(int index)
        {
            if(_lerp != null) StopCoroutine(_lerp);
            _moveToIndex = index;

            if(_moveToIndex <= _transformList.Count)
            {
                _lerp = MoveToPositionAsync();
                StartCoroutine(_lerp);
            }
        
            else
            {
                Debug.Log("wrong index for Prompt position List", this);
                return;
            }
        }
    
        private IEnumerator MoveToPositionAsync()
        {
            var destination = _transformList[_moveToIndex];
            var lerpAmount = 0f;
            transform.DOScale(destination.localScale, _resetDelay);
        
            while (transform.position != destination.position)
            {
                lerpAmount = Mathf.Clamp01(lerpAmount += Time.deltaTime);
                transform.position = Vector3.SmoothDamp(transform.position, destination.position, ref currentvelocity, speed);
                transform.rotation = Quaternion.Lerp(transform.rotation, destination.rotation, lerpAmount * 0.1f);
            
                yield return null;
            }
        }


        //Added 
        public void InjectThresholdControl(bool UseThreshold)
        {
             this.UseThreshold = UseThreshold;
        }
        public void InjectContinousLookAtCam(bool continousLookAtCam)
        {
            this.continousLookAtCam = continousLookAtCam;
        }

        public void InjectThresholdControl(bool UseThreshold, bool continousLookAtCam)
        {
            this.UseThreshold = UseThreshold;
            this.continousLookAtCam = continousLookAtCam;
        }

        public void InjectThresholdControl(bool continousLookAtCam, float ThresholdValue )
        {
            this.continousLookAtCam = continousLookAtCam;
            this.ThresholdValue = ThresholdValue;
        }
    }
}