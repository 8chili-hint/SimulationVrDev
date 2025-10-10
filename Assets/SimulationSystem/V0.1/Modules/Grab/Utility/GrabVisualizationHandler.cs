using System;
using Plugins.QuickOutline.Scripts;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Utility.Event;
using UnityEngine;


namespace SimulationSystem.V0._1.Modules.Grab.Utility
{
    public class GrabVisualizationHandler : MonoBehaviour
    {
        
        #region Variable Declaration

        private struct HandInteractionStatus
        {
            public bool canGrab;
            public bool isHovered;
        }
        
        [NonSerialized] public bool isGrabbable = true;

        private readonly Color _onHoverColor = Color.yellow;
        private readonly Color _canGrabColor = Color.blue;
        
        private bool _isGrabbed;
        private GameObject _leftHandHoverCollider;
        private GameObject _rightHandHoverCollider;
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabbable;
        /*private HandGrabInteractable[] _handGrabInteractables;
        private HandGrabInteractor _leftHandGrabInteractor;
        private HandGrabInteractor _rightHandGrabInteractor;
       */
        private HandInteractionStatus _leftHandInteractionStatus;
        private HandInteractionStatus _rightHandInteractionStatus;
        private Outline[] _outlines;

        #endregion


        #region Monobehaviour

        private void Start()
        {
            _grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            _leftHandHoverCollider = GameManager.Instance.PlayerManager.leftHandHoverCollider;
            _rightHandHoverCollider = GameManager.Instance.PlayerManager.rightHandHoverCollider;/*
            _leftHandGrabInteractor = GameManager.Instance.PlayerManager.LeftGrabInteractor;
            _rightHandGrabInteractor = GameManager.Instance.PlayerManager.RightGrabInteractor;*//*
            _handGrabInteractables = _grabbable.GetComponentsInChildren<HandGrabInteractable>();*/

            AddPointableUnityEventListeners();
            AddTriggerUnityEventListeners();
        }

        private void OnDisable()
        {
            ToggleVisualization(false);
        }

        #endregion


        #region Add Unity Event listeners

        /// <summary>
        /// Add listeners to Trigger related Unity events in the TriggerUnityEventWrapper class on this game object
        /// </summary>
        /// 
        // *rework this
        //  * 
        private void AddTriggerUnityEventListeners()
        {
            var triggerUnityEventWrapper = GetComponentInChildren<TriggerUnityEventWrapper>();
            if (triggerUnityEventWrapper == null) return;
            triggerUnityEventWrapper.onTriggerEnterEvent.AddListener(EnableOnHoverVisualisation);
            triggerUnityEventWrapper.onTriggerExitEvent.AddListener(DisableOnHoverVisualisation);
        }

        /// <summary>
        /// Add listeners to Grabbable related Unity events in the PointableUnityEventWrapper class on this game object
        /// </summary>
        private void AddPointableUnityEventListeners()
        {
/*
            var pointableUnityEventWrapper = GetComponent<PointableUnityEventWrapper>();*/
            // Can be grabbed event
            _grabbable.hoverEntered.AddListener(arg0 =>
            {
                SetCanGrabHandInteractionStatus();
                if ((_leftHandInteractionStatus.canGrab && _rightHandInteractionStatus.canGrab) || _isGrabbed) return;
                SetVisualizationColor(_canGrabColor);
                ToggleVisualization(true);

            });

            // Cannot be grabbed anymore event

            _grabbable.hoverExited.AddListener(arg0 =>
            {
                SetCanGrabHandInteractionStatus();
                if (_leftHandInteractionStatus.canGrab || _rightHandInteractionStatus.canGrab || _isGrabbed) return;
                SetVisualizationColor(_onHoverColor);
                ToggleVisualization(false);

            });

            // On grab event
            _grabbable.selectEntered.AddListener(arg0 =>
            {
                ToggleVisualization(false);
                _isGrabbed = true;
            });

            // On un grab event
            _grabbable.selectExited.AddListener(arg0 =>
            {
                ToggleVisualization(true);
                _isGrabbed = false;
            });
        }

        #endregion

        #region On hover methods

        /// <summary>
        /// Enable OnHover visualisation (Toggle visualisations on and set color to _OnHoverColor)
        /// if collisionObject is a HandHoverCollider, the object is grabbable and is not already hovered.
        /// </summary>
        /// <param name="collisionObject">Collider passed by Trigger Unity event</param>
        private void EnableOnHoverVisualisation(Collider collisionObject)
        {
            if (!collisionObject.CompareTag("GrabHoverCollider") || !isGrabbable) return;

            if (_leftHandInteractionStatus.isHovered || _rightHandInteractionStatus.isHovered)
            {
                SetIsHoveredInteractionStatus(collisionObject.gameObject, true);
                return;
            }

            SetIsHoveredInteractionStatus(collisionObject.gameObject, true);
            ToggleVisualization(true);
            SetVisualizationColor(_onHoverColor);
        }

        /// <summary>
        /// Disable OnHover visualisation (Toggle visualisations off)
        /// if collisionObject is a HandHoverCollider and object is not already hovered.
        /// </summary>
        /// <param name="collisionObject">Collider passed by Trigger Unity event</param>
        private void DisableOnHoverVisualisation(Collider collisionObject)
        {
            if (!collisionObject.CompareTag("GrabHoverCollider")) return;

            SetIsHoveredInteractionStatus(collisionObject.gameObject, false);

            if (_leftHandInteractionStatus.isHovered || _rightHandInteractionStatus.isHovered) return;

            ToggleVisualization(false);
        }

        #endregion

        #region Visualization Methods

        /// <summary>
        /// Adds Outline components to child GameObjects containing MeshRenderers,
        /// configures the outline's properties and store them in the _outlines array.
        /// </summary>
        public void AddOutlineComponents()
        {
            var meshRenderers = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>(true);
            var meshRendererCount = meshRenderers.Length;

            _outlines = new Outline[meshRendererCount];

            for (var i = 0; i < meshRendererCount; i++)
            {
                var outlineElement = meshRenderers[i].gameObject.AddComponent(typeof(Outline)) as Outline;

                if (outlineElement != null)
                {
                    outlineElement.OutlineMode = Outline.Mode.OutlineAll;
                    outlineElement.OutlineWidth = 4f;
                    outlineElement.enabled = false;
                }

                _outlines[i] = outlineElement;
            }
        }

        private void SetVisualizationColor(Color color)
        {
            if (_outlines[0] == null && !_outlines[0].enabled) ToggleVisualization(true);

            foreach (var outlineElement in _outlines)
            {
                outlineElement.OutlineColor = color;
            }
        }

        public void ToggleVisualization(bool enable)
        {
            if (_outlines == null) return;
            foreach (var outlineElement in _outlines)
            {
                if (outlineElement != null) outlineElement.enabled = enable;
            }
        }

        #endregion

        #region Hand Interaction Status methods

        /// <summary>
        /// Sets the isHovered property of either _rightHandInteractionStatus or _leftHandInteractionStatus
        /// based on the collisionObject.
        /// </summary>
        private void SetIsHoveredInteractionStatus(GameObject collisionObject, bool setBool)
        {
            if (collisionObject.gameObject == _rightHandHoverCollider) _rightHandInteractionStatus.isHovered = setBool;
            else if (collisionObject.gameObject == _leftHandHoverCollider) _leftHandInteractionStatus.isHovered = setBool;
        }

        /// <summary>
        /// Checks if the Grabbable object can be grabbed by either hands,
        /// by checking if any HandGrabInteractables of the Grabbable objects are present in any HangGrabInteractors as interactable,
        /// and sets the canGrab boolean of the HandInteractionStatus struct accordingly.
        /// </summary>
        private void SetCanGrabHandInteractionStatus()
        {
            _rightHandInteractionStatus.canGrab = false;
            _leftHandInteractionStatus.canGrab = false;

/*            foreach (var handGrabInteractable in _handGrabInteractables)
            {
                if (_rightHandGrabInteractor.Interactable == handGrabInteractable) _rightHandInteractionStatus.canGrab = true;
                if (_leftHandGrabInteractor.Interactable == handGrabInteractable) _leftHandInteractionStatus.canGrab = true;
            }*/
        }

        #endregion

    }
}