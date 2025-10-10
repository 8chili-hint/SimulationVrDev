using System;
using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Warnings;
using UnityEngine;

namespace SimulationSystem.V0._1.Manager
{
    public class UIManager : MonoBehaviour
    {
        [HideInInspector] public bool canLabel = true;
    
        public GameObject RightHandNamePanel;
        public GameObject LeftHandNamePanel;
        public GameObject WarningUI;
        public GameObject WarningUI_Grab;
        public GameObject WarningUI_Object;
        public GameObject WarningUI_Red;

        [Header("Restart")] 
        [SerializeField] private GameObject RestartUI;
    
        private LabelSystem _labelSystem = new LabelSystem();
        private DetectWarningSystem _warningTrigger = new DetectWarningSystem();
        private DetectWarningSystem _warningTriggerPlusGrab = new DetectWarningSystem();
        private DetectWarningSystem _warningGrab = new DetectWarningSystem();
        private DetectWarningSystem _warningBasic = new DetectWarningSystem();

        private void Start()
        {
            _labelSystem.Initialize(RightHandNamePanel, LeftHandNamePanel);
        
            if (WarningUI.TryGetComponent(out UIAnimationHandler anim))
            {
                anim.OnUnDetected();
            }
            if (WarningUI_Grab.TryGetComponent(out UIAnimationHandler anim2))
            {
                anim2.OnUnDetected();
            }
            if (WarningUI_Object.TryGetComponent(out UIAnimationHandler anim3))
            {
                anim3.OnUnDetected();
            }

            _warningBasic.Initialize(WarningUI_Red, GameManager.Instance.warningDetectsBasic.ToList<DetectWarning>());
            _warningGrab.Initialize(WarningUI_Grab, GameManager.Instance.warningDetectsGrab.ToList<DetectWarning>());
            _warningTrigger.Initialize(WarningUI, GameManager.Instance.warningDetectsTriggers.ToList<DetectWarning>());
            _warningTriggerPlusGrab.Initialize(WarningUI_Object, GameManager.Instance.warningDetectsTriggerPlusGrab.ToList<DetectWarning>());
        }

        private void Update()
        {
            _labelSystem.SetLabel(canLabel);
        
            _warningGrab.CheckWarning();
            _warningTrigger.CheckWarning();
            _warningTriggerPlusGrab.CheckWarning();
            _warningBasic.CheckWarning();
        }

        public void PopDetectWarning(DetectWarning warning)
        {
            _warningBasic.PopDetect(warning);
        }
    
        public void AddDetectWarning(DetectWarning warning)
        {
            _warningBasic.AddDetect(warning);
        }

        #region Labels
        public void HideAllLabels()
        {
            _labelSystem.HideAllLabels();
        }
    
        public void ForceHideAllLabels()
        {
            _labelSystem.ForceHideAllLabels();
        }

        public void ShowRightLabel(string objectName)
        {
            _labelSystem.ShowRightLabel(objectName);
        }
    
        public void ShowLeftLabel(string objectName)
        {
            _labelSystem.ShowLeftLabel(objectName);
        }

        public void HideRightLabel()
        {
            _labelSystem.HideRightLabel();
        }
    
        public void HideLeftLabel()
        {
            _labelSystem.HideLeftLabel();
        }
        #endregion

        #region RestartUI

        public void InvokeRestartUI()
        {
            if (RestartUI.TryGetComponent(out UIAnimationHandler anim))
            {
                anim.OnDetectOnce();
            }
        }

        #endregion

    }

//Make Struct for Warning
    [Serializable]
    public struct DetectWarningSystem
    {
        private bool _hasWarningDetect;
        private GameObject _warningLabel;
        private List<DetectWarning> _detectWarnings;

        public DetectWarningSystem(GameObject warningLabel, List<DetectWarning> detectWarnings)
        {
            _warningLabel = warningLabel;
            _detectWarnings = detectWarnings;
            _hasWarningDetect = false;
        }

        public void Initialize(GameObject warningLabel, List<DetectWarning> detectWarnings)
        {
            _warningLabel = warningLabel;
            _detectWarnings = detectWarnings;
            _hasWarningDetect = false;
        }

        public void PopDetect(DetectWarning warning)
        {
            if(_detectWarnings.Contains(warning))
                _detectWarnings.Remove(warning);
        }
    
        public void AddDetect(DetectWarning warning)
        {
            if(!_detectWarnings.Contains(warning))
                _detectWarnings.Add(warning);
        }
    
        //Refactor this Function
        //It basically does is takes all the warning in the scene and whichever state is detected it show their respective UIPrefab
        public void CheckWarning()
        {
            string firstDetectWarningText = null;
            bool hasDetectWarning = false;

            foreach (var warning in _detectWarnings)
            {
                if (warning.State == DetectStates.Detect)
                {
                    firstDetectWarningText = warning.WarningText;
                    hasDetectWarning = true;
                    break;
                }
            }

            if (hasDetectWarning && !_hasWarningDetect)
            {
                ShowWarning(_warningLabel, firstDetectWarningText);
                _hasWarningDetect = true;
            }
            else if (_detectWarnings.Any(g => g.State == DetectStates.UnDetect) && _hasWarningDetect)
            {
                HideWarning(_warningLabel);
                _hasWarningDetect = false;
            }
        
            /*var text = from detect in _detectWarnings
            where detect.State == DetectStates.Detect
            select detect.WarningText;
        
        if (_detectWarnings.Any(g => g.State == DetectStates.Detect) && !_hasWarningDetect)
        {
            ShowWarning(_warningLabel, text.ToList()[0]);
            _hasWarningDetect = true;
        }
        else if (_detectWarnings.Any(g => g.State == DetectStates.UnDetect) && _hasWarningDetect)
        {
            HideWarning(_warningLabel);
            _hasWarningDetect = false;
        }*/
        }
    
        private void ShowWarning(GameObject warningUI, string text)
        {
            if (warningUI.TryGetComponent(out UITextHandler uiText))
            {
                uiText.text.text = text;
            }

            if (warningUI.TryGetComponent(out UIAnimationHandler anim))
            {
                anim.OnDetectOnce();
            }
        }

        private void HideWarning(GameObject warningUI)
        {
            if (warningUI.TryGetComponent(out UITextHandler uiText))
            {
                uiText.text.text = String.Empty;
            }

            if (warningUI.TryGetComponent(out UIAnimationHandler anim))
            {
                anim.OnUnDetected();
            }
        }
    }

// Change this with Legacy One
    [Serializable]
    public struct LabelSystem
    {
        private bool _canLabel;
        private GameObject _rightHandNamePanel;
        private GameObject _leftHandNamePanel;

        public LabelSystem(GameObject rightHandNamePanel, GameObject leftHandNamePanel)
        {
            _canLabel = true;
            _rightHandNamePanel = rightHandNamePanel;
            _leftHandNamePanel = leftHandNamePanel;
        }

        public void Initialize(GameObject rightHandNamePanel, GameObject leftHandNamePanel)
        {
            _rightHandNamePanel = rightHandNamePanel;
            _leftHandNamePanel = leftHandNamePanel;
        }
    
        public void SetLabel(bool canLabel)
        {
            _canLabel = canLabel;
        }
    
        public void HideAllLabels()
        {
            if(_rightHandNamePanel.TryGetComponent<UIAnimationHandler>(out var uir))
            {
                uir.OnUnDetected();
            }

            if (_leftHandNamePanel.TryGetComponent<UIAnimationHandler>(out var uil))
            {
                uil.OnUnDetected();
            }
        }
    
        public void ForceHideAllLabels()
        {
            if(_rightHandNamePanel.TryGetComponent<UIAnimationHandler>(out var uir))
            {
                uir.ForceRemove();
            }

            if (_leftHandNamePanel.TryGetComponent<UIAnimationHandler>(out var uil))
            {
                uil.ForceRemove();
            }
        }
    
        public void ShowRightLabel(string objectName)
        {
            var obj = _rightHandNamePanel;
            ShowLabel(obj, objectName);
        }
    
        public void ShowLeftLabel(string objectName)
        {
            var obj = _leftHandNamePanel;
            ShowLabel(obj, objectName);
        }
    
        public void HideRightLabel()
        {
            var obj = _rightHandNamePanel;
            HideLabel(obj);
        }
    
        public void HideLeftLabel()
        {
            var obj = _leftHandNamePanel;
            HideLabel(obj);
        }
    
        private void ShowLabel(GameObject label, string objectName)
        {
            if(!_canLabel)
                return;
        
            if (label.TryGetComponent<UIAnimationHandler>(out var anim))
            {
                anim.OnDetectOnce();
            }

            if (label.TryGetComponent<UITextHandler>(out var uiText))
            {
                uiText.text.text = objectName;
            }
        }
        private void HideLabel(GameObject label)
        {
            if (label.TryGetComponent<UIAnimationHandler>(out var anim))
            {
                anim.OnUnDetected();
            }

            if (label.TryGetComponent<UITextHandler>(out var uiText))
            {
                uiText.text.text = "";
            }
        }
    }
}