using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Animation.Drop.ToBeRefacored
{
    public class DropAnimation : MonoBehaviour
    {
        public GameObject dropModel;
        public Transform endPosition;
        public Vector3 endSize = Vector3.one;
        public float duration = 0.5f;

        public UnityEvent onEndEvent;
        private Sequence _mySequence;

        private void Start()
        {
            StartDropAnimation();
        }
    
        //Dropping Animations
        public void StartDropAnimation()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(dropModel.transform.DOLocalMove(endPosition.localPosition, duration));
            mySequence.Insert(0, dropModel.transform.DOScale(endSize, 0.3f));
            mySequence.OnComplete(delegate { mySequence.Restart(false, -1); });
            _mySequence = mySequence;
        }
    
        private void OnDisable()
        {
            _mySequence.Kill();
        }

        public void StopDropAnimation()
        {
            _mySequence.Kill();
            dropModel.SetActive(false);
        }
    }
}
