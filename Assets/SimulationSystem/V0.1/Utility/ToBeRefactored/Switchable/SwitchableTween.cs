using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored.Switchable
{
    public class SwitchableTween : SwitchableAbs
    {
        public Vector3[] pathLeft;
        public Vector3[] pathRight;
    
        public Vector3 originalScale;
        public Vector3 switchedScale;
    
        protected override IEnumerator SwitchPosition()
        {
            yield return null;
            //transform.DOPath(pathLeft, 1);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOLocalPath(pathLeft, 1));
            mySequence.Insert(0, transform.DOScale(switchedScale, 0.1f));
            //mySequence.OnComplete(delegate { mySequence.Restart(false, -1); });
        }
    
        protected override IEnumerator OriginalPosition()
        {
            yield return null;
            //transform.DOPath(pathRight, 1);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOLocalPath(pathRight, 1));
            mySequence.Insert(0, transform.DOScale(originalScale, 0.1f));
        }
    }
}