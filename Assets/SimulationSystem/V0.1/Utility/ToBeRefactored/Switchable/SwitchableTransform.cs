using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored.Switchable
{
    public class SwitchableTransform : SwitchableAbs
    {
        public Transform origTransform;
        public Transform switchedTransform;
    
        protected override IEnumerator SwitchPosition()
        {
            yield return null;
            transform.DOLocalMove(switchedTransform.localPosition, 1);
            transform.DOLocalRotateQuaternion(switchedTransform.localRotation, 1);
            transform.DOScale(switchedTransform.localScale, 1);
        }
    
        protected override IEnumerator OriginalPosition()
        {
            yield return null;
            transform.DOLocalMove(origTransform.localPosition, 1);
            transform.DOLocalRotateQuaternion(origTransform.localRotation, 1);
            transform.DOScale(origTransform.localScale, 1);
        }
    
        public void SwitchPositionIndividual()
        {
            transform.DOLocalMove(switchedTransform.localPosition, 1);
            transform.DOLocalRotateQuaternion(switchedTransform.localRotation, 1);
            transform.DOScale(switchedTransform.localScale, 1);
        }
        public void OriginalPositionIndividual()
        {
            transform.DOLocalMove(origTransform.localPosition, 1);
            transform.DOLocalRotateQuaternion(origTransform.localRotation, 1);
            transform.DOScale(origTransform.localScale, 1);
        }
    
    
    }
}