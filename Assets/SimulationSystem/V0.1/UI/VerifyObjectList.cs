using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Utility.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.UI
{
    public class VerifyObjectList : MonoBehaviour
    {
        [System.Serializable]
        public class CheckObject
        {
            [FormerlySerializedAs("detect")] public DetectObject detectObject;
            public GameObject hint;
        }
    
        [System.Serializable]
        public class ToggleHint
        {
            private Toggle _checkToggle;
            private GameObject _hint;

            public Toggle CheckToggle => _checkToggle;

            public ToggleHint(Toggle checkToggle, GameObject hint)
            {
                _checkToggle = checkToggle;
                _hint = hint;
            }

            public void ConfirmCheck()
            {
                _checkToggle.isOn = true;
                _hint.DeactivateThisObject();
            }

            public void DisableCheck()
            {
                _checkToggle.isOn = false;
                _hint.ActivateThisObject();
            }
        }
    
        [SerializeField] private GameObject toggleCheckPrefab;
        [SerializeField] private Transform checkListUIParent;
        [SerializeField] private List<CheckObject> checkObjects = new List<CheckObject>();
        [SerializeField] private UnityEvent onVerifyComplete;
    
        private bool _isFirstCallDone = false;
        private HandStatus _rightHandStatus;
        private HandStatus _leftHandStatus;
        private readonly Dictionary<string, ToggleHint> _detectObjectNames = new Dictionary<string, ToggleHint>();
    
        private void Start()
        {
            checkObjects.ForEach(g =>
            {
                var instantiate = Instantiate(toggleCheckPrefab, checkListUIParent);
                instantiate.name = "g.detectObject.ObjectName";
                
                string nonCamelCaseString = "g.detectObject.ObjectName";
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string camelCaseString = textInfo.ToTitleCase(nonCamelCaseString);
                
                instantiate.transform.GetChild(0).GetComponent<TMP_Text>().text = camelCaseString;
                
                _detectObjectNames.Add("g.detectObject.ObjectName",
                    new ToggleHint(instantiate.GetComponent<Toggle>(), g.hint));
            });

            foreach (var detectObjectName in _detectObjectNames)
            {
                detectObjectName.Value.DisableCheck();
            }
        }

        private void OnEnable()
        {
            _rightHandStatus = GameManager.Instance.PlayerManager.RightHandStatus;
            _leftHandStatus = GameManager.Instance.PlayerManager.LeftHandStatus;
        
            _rightHandStatus.onDetectUpdate.AddListener(VerifyTheObject);
            _leftHandStatus.onDetectUpdate.AddListener(VerifyTheObject);
        }

        private void VerifyTheObject(string objectName)
        {
            if (_detectObjectNames.TryGetValue(objectName, out var detectObjectName))
            {
                detectObjectName.ConfirmCheck();
            }
        
            CheckAllObject();
        }

        //! Can be implemented in a better way
        private void CheckAllObject()
        {
            if (_detectObjectNames.All(g => g.Value.CheckToggle.isOn) && !_isFirstCallDone)
            {
                onVerifyComplete?.Invoke();
                _isFirstCallDone = true;
            }
        }
    }
}
