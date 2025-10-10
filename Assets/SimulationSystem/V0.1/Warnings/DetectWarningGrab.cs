using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using UnityEngine;

namespace SimulationSystem.V0._1.Warnings
{
    [RequireComponent(typeof(DetectHand), typeof(DetectAssignedHand))]
    public class DetectWarningGrab : DetectWarning
    {
        /*  public bool CanShow { get; set; }
          public DetectHand wrapper;

          private void Start()
          {
              if (!wrapper) return;
              wrapper.OnDetectInCorrectHand.AddListener(OnDetectWarning);
              wrapper.OnUnDetectHand.AddListener(OnUnDetect);
              wrapper.OnDetectCorrectHand.AddListener(OnUnDetect);
          }

          public void OnDetectWarning()
          {
              hasDetected = true;
          }

          public void OnUnDetect()
          {
              hasDetected = false;
          }

          private void Update()
          {
              UpdateState();
          }
      }*/
    }
}