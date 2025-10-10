using System;
using SimulationSystem.V0._1.Utility.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Syringe
{
    public class SyringeController : MonoBehaviour
    {
        public enum Mode
        {
            In,
            Out,
            None
        }
    
        public float liquidMaxScale, liquidMinScale, liquidDiffScale;
        public float liquidMaxPos, liquidMinPos, liquidDiffPos;
        public Transform liquid;
    
        [Range(0,1)]
        public float fill, maxFill;
        public TextMeshProUGUI text;
        public bool veilTouch = false, injectionPointContact = false;
        public UnityEvent OnFillComplete, OnEmptyComplete;
        public FloatEvent onFilling, onInjecting;
        public Mode mode = Mode.In;

        private Syringe _syringe;
    
        private void Start()
        {
            liquidDiffScale = liquidMaxScale - liquidMinScale;
            liquidDiffPos = liquidMaxPos - liquidMinPos;

            _syringe = GetComponent<Syringe>();
        }

        private float _previousPressedProgress = 1;
        public void SyringeUpdate(float value)
        {
            if (value > _previousPressedProgress && mode == Mode.Out && injectionPointContact)
            {
                UpdateLiquid(value);
                onInjecting?.Invoke(value);
                if (_previousPressedProgress >= 1)
                {
                    OnEmptyComplete?.Invoke();
                    mode = Mode.None;
                }
            }
            else if (value < _previousPressedProgress && mode == Mode.In && veilTouch)
            {
                UpdateLiquid(value);
                onFilling.Invoke(value);
                if (_previousPressedProgress <= 0)
                {
                    OnFillComplete?.Invoke();
                    mode = Mode.None;
                }
            }

            UpdateText();
        }
    
        public void SyringeTriggerUpdate(float value)
        {
            UpdateLiquid(value);
            UpdateText();
        }

        public void ReverseTriggerUpdate(float value)
        {
            float value2 = 1 - value;
            UpdateLiquid(value2);
        }
    
        private void UpdateText()
        {
            if (text)
                text.text = 1 - Math.Round((fill * 1), 2) + " ml/" + maxFill + " ml";
        }

        public void ResetVeilText()
        {
            text.text = "0 ml/"+ maxFill +" ml";
        }

        private void UpdateLiquid(float value)
        {
            _previousPressedProgress = value;
            fill = _previousPressedProgress;
            var transform1 = liquid.transform;
            var localPosition = transform1.localPosition;
            Transform transform2;
            (transform2 = liquid.transform).localPosition = new Vector3(localPosition.x,
                localPosition.y,
                Mathf.Lerp(liquidMinPos, liquidMaxPos, 1 - _previousPressedProgress));
        
            var localScale = transform2.localScale;
            localScale = new Vector3(localScale.x,
                Mathf.Lerp(liquidMinScale, liquidMaxScale, 1 - _previousPressedProgress),
                localScale.z);
        
            liquid.transform.localScale = localScale;

           /* if(_syringe.type == Syringe.SyringeType.FlipFlop)
                _syringe.trigger.localPosition = Vector3.Lerp(new Vector3(0, 0, 0.00511f), new Vector3(0, 0, 0), _previousPressedProgress);*/
        }
    }
}
