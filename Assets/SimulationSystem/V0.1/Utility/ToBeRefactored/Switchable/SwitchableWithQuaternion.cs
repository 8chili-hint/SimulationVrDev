using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored.Switchable
{
    public class SwitchableWithQuaternion : SwitchableAbs
    {
        [SerializeField] private bool canSwitch = false;
    
        [Header("Original Transform")]
        public Vector3 _origPosition;
        public Quaternion _origRotation;
        public Vector3 _origScale;
    
        [Space(10)]
        [Header("Switched Transform")]
        public Vector3 switchedPosition;
        public Quaternion switchedRotation;
        public Vector3 switchedScale;

        protected override IEnumerator SwitchPosition()
        {
            if (canSwitch)
            {
                yield return null;
                transform.DOLocalMove(switchedPosition, 1);
                transform.DOLocalRotateQuaternion(switchedRotation, 1);
                transform.DOScale(switchedScale, 1);
            }
            else
            {
                yield return null;
            }
        
        }
    
        protected override IEnumerator OriginalPosition()
        {
            if (canSwitch)
            {
                yield return null;
                transform.DOLocalMove(_origPosition, 1);
                transform.DOLocalRotateQuaternion(_origRotation, 1);
                transform.DOScale(_origScale, 1);
            }
            else
            {
                yield return null;
            }
        }

        [ContextMenu("Save Original Transform")]
        public void SaveOriginalPosition()
        {
            _origPosition = transform.localPosition;
            _origRotation = transform.localRotation;
            _origScale = transform.localScale;
        }
    
        [ContextMenu("Save Switched Transform")]
        public void SaveSwitchedPosition()
        {
            switchedPosition = transform.localPosition;
            switchedRotation = transform.localRotation;
            switchedScale = transform.localScale;
        }
    
        [ContextMenu("Move to Original Transform")]
        public void MoveToOriginalPosition()
        {
            transform.SetLocalPositionAndRotation(_origPosition, _origRotation);
            transform.localScale = _origScale;
        }
    
        [ContextMenu("Move to Switched Transform")]
        public void MoveToSwitchedPosition()
        {
            transform.SetLocalPositionAndRotation(switchedPosition, switchedRotation);
            transform.localScale = switchedScale;
        }
    
        public void SwitchPositionIndividual()
        {
            transform.DOLocalMove(switchedPosition, 1);
            transform.DOLocalRotateQuaternion(switchedRotation, 1);
        }
        public void OriginalPositionIndividual()
        {
            transform.DOLocalMove(_origPosition, 1);
            transform.DOLocalRotateQuaternion(_origRotation, 1);
        }
    }
}