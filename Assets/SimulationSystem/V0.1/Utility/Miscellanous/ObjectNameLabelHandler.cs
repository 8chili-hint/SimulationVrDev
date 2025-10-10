
using SimulationSystem.V0._1.UI;
using SimulationSystem.V0._1.Utility.Event;
using TMPro;
using UnityEngine;


namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class ObjectNameLabelHandler : MonoBehaviour
    {

        #region Variable declaration

        [SerializeField] private string objectDisplayName;
        public UIAnimationHandler grabbableObjectLabel;

        private bool _isGrabbed;
        private bool _isActivated;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            AddTriggerUnityEventListeners();

            var textMeshProUGUI = grabbableObjectLabel.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshProUGUI != null) textMeshProUGUI.text = objectDisplayName;

            if (GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() == null) return;

            var xrGrabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            xrGrabInteractable.hoverEntered.AddListener(arg0 => EnableObjectNameLabel());
            xrGrabInteractable.hoverExited.AddListener(arg0 => DisableObjectNameLabel());

            // On grab event
            xrGrabInteractable.selectEntered.AddListener(arg0 =>
            {
                _isGrabbed = true;
                DisableObjectNameLabel();
            });

            // On un grab event
            xrGrabInteractable.selectExited.AddListener(arg0 =>
            {
                _isGrabbed = false;
                EnableObjectNameLabel();
            });
        }

        #endregion

        #region Add Unity Event listeners

        private void AddTriggerUnityEventListeners()
        {
            var triggerUnityEventWrapper = GetComponentInChildren<TriggerUnityEventWrapper>();

            if (triggerUnityEventWrapper == null) return;

            triggerUnityEventWrapper.onTriggerEnterEvent.AddListener(collisionObject =>
            {
                if (!collisionObject.CompareTag("GrabHoverCollider") || _isActivated || !_isGrabbed) return;
                EnableObjectNameLabel();
            });

            triggerUnityEventWrapper.onTriggerExitEvent.AddListener(collisionObject =>
            {
                if (!collisionObject.CompareTag("GrabHoverCollider") || !_isActivated) return;
                DisableObjectNameLabel();
            });
        }

        #endregion

        #region Toggle label functions

        private void EnableObjectNameLabel()
        {
            grabbableObjectLabel.OnDetectOnce();
            _isActivated = true;
        }

        private void DisableObjectNameLabel()
        {
            grabbableObjectLabel.OnUnDetected();
            _isActivated = false;
        }

        #endregion

    }
}