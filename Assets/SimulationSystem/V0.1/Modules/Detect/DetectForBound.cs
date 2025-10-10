using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Utility.BoundDetect;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect
{
    public class DetectForBound : MonoBehaviour
    {
        private enum Bounds
        {
            None,
            In,
            Out,
        }

        public List<BoundsBoxCast> BoundsBoxCasts = new List<BoundsBoxCast>();

        [Space(10)]
        [SerializeField] private UnityEvent onInnerDetect;
        [SerializeField] private UnityEvent onOuterDetect;
        [SerializeField] private UnityEvent onUnDetect;

        private Bounds _bounds = Bounds.None;
        private DetectStates _states = DetectStates.Normal;


        private void Start()
        {
            BoundsBoxCasts = GetComponentsInChildren<BoundsBoxCast>().ToList();
        }

        private void FixedUpdate()
        {
            IsInRange();
        
            if (_bounds == Bounds.In && _states == DetectStates.Normal || _bounds == Bounds.In && _states == DetectStates.UnDetect)
            {
                onInnerDetect?.Invoke();
                _states = DetectStates.Detect;
            }
            else if (_bounds == Bounds.Out && _states == DetectStates.Detect)
            {
                onOuterDetect?.Invoke();
                _states = DetectStates.UnDetect;
            }
            else if(_bounds == Bounds.None && _states == DetectStates.UnDetect)
            {
                onUnDetect?.Invoke();
                _states = DetectStates.Normal;
            }
        }
    
        private void IsInRange()
        {
            if (BoundsBoxCasts.All(g => g.isDetect))
            {
                _bounds = Bounds.In;
            }
            else if(BoundsBoxCasts.Any(g => g.isDetect))
            {
                _bounds = Bounds.Out;
            }
            else if (BoundsBoxCasts.All(g => !g.isDetect))
            {
                _bounds = Bounds.None;
            }
        
        }
    }
}