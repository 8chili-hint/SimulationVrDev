using System;
using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect.Interface;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions;
namespace SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_
{
    public partial class DetectHand : MonoBehaviour/*, IDetect*/
    {
        /*  [SerializeField] private string objectName = String.Empty;

          //Make a button based Event Sys.

          [SerializeField] private DirectInteractor rightGrabInteractor;
          [SerializeField] private DirectInteractor leftGrabInteractor;

          [SerializeField] private float timer;
          [SerializeField] private float detectTimer;

          [Space(10)]
          public UnityEvent OnDetect;
          public UnityEvent OnDetectCorrectHand;
          public UnityEvent OnDetectInCorrectHand;
          public UnityEvent OnDetectDominantHand;
          public UnityEvent OnDetectionComplete;
          public UnityEvent OnUnDetectHand;
          public UnityEvent<bool> OnHoverDetect;
          public UnityEvent OnUnHoverDetect;

          private bool canDetect = true;
          private bool isFirstTrigger = true;
          private DetectAudioHandler detectAudioHandler;
          private DetectAssignedHand detectAssignedHand;
          private DominantHand _currentDominantHand = DominantHand.RightHand;
          private HandType _currentHandType = HandType.DominantHand;
          private List<HandGrabInteractable> interactables = new List<HandGrabInteractable>();

          public void Initialize()
          {
              timer = 0f;

              #region SetInteractor
              rightGrabInteractor = GameManager.Instance.PlayerManager.RightGrabInteractor;
              leftGrabInteractor = GameManager.Instance.PlayerManager.LeftGrabInteractor;
              #endregion

              if (TryGetComponent<PointableUnityEventWrapper>(out var wrapper))
              {
                  wrapper.WhenSelect.AddListener(OnGrab);
                  wrapper.WhenUnselect.AddListener(OnRelease);
                  wrapper.WhenHover.AddListener(OnHover);
                  wrapper.WhenUnhover.AddListener(OnUnHover);
              }
              else
              {
                  var wrapperAdd = transform.gameObject.AddComponent<PointableUnityEventWrapper>();
                  wrapperAdd.InjectPointable(GetComponent<Grabbable>());

                  wrapperAdd.WhenSelect.AddListener(OnGrab);
                  wrapperAdd.WhenUnselect.AddListener(OnRelease);
                  wrapperAdd.WhenHover.AddListener(OnHover);
                  wrapperAdd.WhenUnhover.AddListener(OnUnHover);
              }

              if (TryGetComponent<DetectAudioHandler>(out var audioHandler))
              {
                  detectAudioHandler = audioHandler;
              }

              if (TryGetComponent<DetectAssignedHand>(out var assignedHand))
              {
                  detectAssignedHand = assignedHand;
              }
          }

          public void UpdateLoop()
          {
              if (canDetect)
              {
                  if (CheckForAssignedHand())
                      DetectionCompleteCheck();
              }
          }

          public void OnDetectionStart(Collider other)
          {
              throw new NotImplementedException();
          }

          public void OnDetecting(Collider other)
          {
              throw new NotImplementedException();
          }

          public void OnDetectionEnd(Collider other)
          {
              throw new NotImplementedException();
          }

          public void OnObjectEnter(Collider other)
          {
              //throw new System.NotImplementedException();
          }

          public void OnObjectStay(Collider other)
          {
              //throw new System.NotImplementedException();
          }

          public void OnObjectExit(Collider other)
          {
              //throw new System.NotImplementedException();
          }

          public void StartDetection()
          {
              canDetect = true;
          }

          public void StopDetection()
          {
              canDetect = false;
              isFirstTrigger = true;
          }

          private void DetectionCompleteCheck()
          {
              if (isFirstTrigger)
              {
                  timer += Time.deltaTime;
                  if (timer > detectTimer)
                  {
                      OnDetectionComplete.Invoke();

                      if (detectAudioHandler)
                          detectAudioHandler.PlayDetectEffect();

                      timer = 0;
                      isFirstTrigger = false;
                  }
              }
          }

          public void OnHover( )
          {
              if (rightGrabInteractor.Interactable != null &&
                  CheckInteractable(rightGrabInteractor.Interactable.gameObject))
              {
                  if (objectName != String.Empty)
                      GameManager.Instance.UIManager.ShowRightLabel(objectName);
                  _currentDominantHand = DominantHand.RightHand;
                  _currentHandType = CheckForDominantHand()
                      ? HandType.DominantHand
                      : HandType.NonDominantHand;
              }
              else
              {
                  if (objectName != String.Empty)
                      GameManager.Instance.UIManager.HideRightLabel();
              }

              if (leftGrabInteractor.Interactable != null &&
                  CheckInteractable(leftGrabInteractor.Interactable.gameObject))
              {
                  if (objectName != String.Empty)
                      GameManager.Instance.UIManager.ShowLeftLabel(objectName);
                  _currentDominantHand = DominantHand.LeftHand;
                  _currentHandType = CheckForDominantHand()
                      ? HandType.DominantHand
                      : HandType.NonDominantHand;
              }
              else
              {
                  if (objectName != String.Empty)
                      GameManager.Instance.UIManager.HideLeftLabel();
              }

              if (detectAssignedHand)
              {
                  OnHoverDetect?.Invoke(CheckForAssignedHand());
              }
              else
              {
                  OnHoverDetect?.Invoke(true);
              }
          }

          private bool CheckInteractable(GameObject obj)
          {
            *//*  if (interactables.Count < 2)
                  interactables = GetComponentsInChildren<>().ToList();
  *//*
              foreach (var interactable in interactables)
              {
                  if (obj == interactable.gameObject)
                  {
                      return true;
                  }
              }

              return false;
          }

          public void OnUnHover( )
          {
              OnUnHoverDetect?.Invoke();
          }

          public void OnGrab()
          {
              if (canDetect)
              {
                  OnDetect?.Invoke();

                  if (rightGrabInteractor.Interactable != null &&
                      CheckInteractable(rightGrabInteractor.Interactable.gameObject))
                  {
                      _currentDominantHand = DominantHand.RightHand;
                      _currentHandType = CheckForDominantHand()
                          ? HandType.DominantHand
                          : HandType.NonDominantHand;
                  }

                  if (leftGrabInteractor.Interactable != null &&
                      CheckInteractable(leftGrabInteractor.Interactable.gameObject))
                  {
                      _currentDominantHand = DominantHand.LeftHand;
                      _currentHandType = CheckForDominantHand()
                          ? HandType.DominantHand
                          : HandType.NonDominantHand;
                  }

                  if (CheckForDominantHand())
                  {
                      OnDetectDominantHand?.Invoke();
                  }

                  if (CheckForAssignedHand())
                  {
                      IsGrabbedWithAssignedHand = true;
                      OnDetectCorrectHand?.Invoke();
                  }

                  if (!CheckForAssignedHand())
                  {
                      IsGrabbedWithAssignedHand = false;
                      OnDetectInCorrectHand?.Invoke();
                  }

                  if (detectAudioHandler)
                      detectAudioHandler.PlayDetectEffect();
              }

              //GameManager.Instance.UIManager.canLabel = false;
              GameManager.Instance.UIManager.HideAllLabels();
          }

          public void OnRelease()
          {
              _currentDominantHand = DominantHand.None;
              _currentHandType = HandType.None;
              IsGrabbedWithAssignedHand = false;

              OnUnDetectHand?.Invoke();
              GameManager.Instance.UIManager.canLabel = true;
          }

          private bool CheckForDominantHand()
          {
              return GameManager.Instance.PlayerManager.DominantHand == _currentDominantHand;
          }

          private bool CheckForAssignedHand()
          {
              if (detectAssignedHand)
              {
                  return detectAssignedHand.assignedHand == _currentHandType;
              }
              return false;
          }
      }
  }*/
    }
}
