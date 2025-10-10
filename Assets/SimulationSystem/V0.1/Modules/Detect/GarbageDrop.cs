using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect
{
    public class GarbageDrop : MonoBehaviour
    {
        [SerializeField] private bool startOnEnable;
        [SerializeField] private float distance = 0.7f;
        public Transform garbage;
        public UnityEvent onGarbageDropStart;
        public UnityEvent onGarbageDropEnd;
    
        public void GarbageDropAnimation(Transform _garbage)
        {
            _garbage.parent = null;
            AnimateGarbage(_garbage);
        }

        public void OnEnable()
        {
            if(!startOnEnable)
                return;
        
            AnimateGarbage(garbage);
        }

        private void AnimateGarbage(Transform garbage)
        {
            onGarbageDropStart?.Invoke();
            Vector3 finalPos = garbage.localPosition + (Vector3.down) * distance;
            garbage.DOLocalMove(finalPos, 1.5f).OnComplete(delegate { onGarbageDropEnd?.Invoke(); }).SetEase(Ease.Linear);
        }
    }
}
