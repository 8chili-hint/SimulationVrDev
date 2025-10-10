using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored.Switchable
{
    public abstract class SwitchableAbs : MonoBehaviour
    {
        public virtual IEnumerator GetSwitchIEnumerator()
        {
            return SwitchPosition();
        }

        public virtual IEnumerator GetOriginalIEnumerator()
        {
            return OriginalPosition();
        }

        protected virtual IEnumerator SwitchPosition()
        {
            yield return null;
        }
    
        protected virtual IEnumerator OriginalPosition()
        {
            yield return null;
        }
    }

    public class Switchable : SwitchableAbs
    {
        public Vector3 _origPosition;
        public Vector3 _origRotation;
    
        public Vector3 switchedPosition;
        public Vector3 switchedRotation;

        protected override IEnumerator SwitchPosition()
        {
            yield return null;
            transform.DOLocalMove(switchedPosition, 1);
            transform.DOLocalRotate(switchedRotation, 1);
        }
    
        protected override IEnumerator OriginalPosition()
        {
            yield return null;
            transform.DOLocalMove(_origPosition, 1);
            transform.DOLocalRotate(_origRotation, 1);
        }

        [ContextMenu("Save Original Transform")]
        public void SaveOriginalPosition()
        {
            _origPosition = transform.localPosition;
            _origRotation = transform.localRotation.eulerAngles;
        }
    
        [ContextMenu("Save Switched Transform")]
        public void SaveSwitchedPosition()
        {
            switchedPosition = transform.localPosition;
            switchedRotation = transform.localRotation.eulerAngles;
        }
    
        /*[ContextMenu("Move to Original Transform")]
    public void SaveOriginalPosition()
    {
        _origPosition = transform.localPosition;
        _origRotation = transform.localRotation.eulerAngles;
    }
    
    [ContextMenu("Move to Switched Transform")]
    public void SaveSwitchedPosition()
    {
        switchedPosition = transform.localPosition;
        switchedRotation = transform.localRotation.eulerAngles;
    }*/

        public void SwitchPositionIndividual()
        {
            transform.DOLocalMove(switchedPosition, 1);
            transform.DOLocalRotate(switchedRotation, 1);
        }
        public void OriginalPositionIndividual()
        {
            transform.DOLocalMove(_origPosition, 1);
            transform.DOLocalRotate(_origRotation, 1);
        }
    }
}