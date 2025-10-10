using System;
using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Modules.Detect.Utility;
using SimulationSystem.V0._1.Simulation;
using Unity.Collections;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility
{
    public class GuidanceArrow : MonoBehaviour
    {
        private SimulationManager simulationManager;
        [ReadOnly] private Transform _targetTransform;
        private Camera cam;
        [SerializeField] private Transform arrow;
        private bool DisableGuidanceArrow;


      
        private void Start()
        {
            if (PlayerManager.Instance.mainCamera != null)
            {
                cam = PlayerManager.Instance.mainCamera.GetComponent<Camera>();
            }
            else
            {
                
                cam = GetComponent<Camera>();
            }
            simulationManager = GameManager.Instance.SimulationManager;
            foreach (SimulationState s in simulationManager.simulationStates)
            {
                s.onStateStart.AddListener(() => transform.GetChild(0).gameObject.SetActive(false));
                s.onStateComplete.AddListener(() => transform.GetChild(0).gameObject.SetActive(true));
                
            }

        }
        private void Update()
        {
            if (simulationManager.currentState == null)
                return;
            #region CheckStateType

            

            switch (simulationManager.currentState.stateType)
            {
              
                case SimulationState.StateType.DetectWithGrab:
                   
                    if (SimulationSystem.V0._1.Manager.GameManager.Instance.PlayerManager.LeftGrabInteractor.isSelectActive || SimulationSystem.V0._1.Manager.GameManager.Instance.PlayerManager.RightGrabInteractor.isSelectActive)
                    {
                        if (!DisableGuidanceArrow)
                        {
                            if (simulationManager.currentState.GetComponent<SeriallyToggleDetect>())
                            {
                                int activeDetect = simulationManager.currentState.GetComponent<SeriallyToggleDetect>().Activedetect;
                                _targetTransform = simulationManager.currentState.GetComponent<SeriallyToggleDetect>()
                                    .StateDetects[activeDetect].transform;

                            }
                            else
                            {
                                _targetTransform = simulationManager.currentState.objectToDetectList[0].detectObject.transform.GetChild(0).transform;
                            }
                        }
                        else
                        {
                            _targetTransform = null;
                            //_DisableGuidanceArrow();
                            _DisableMesh();
                        }
                    }
                    else if (!SimulationSystem.V0._1.Manager.GameManager.Instance.PlayerManager.LeftGrabInteractor.isSelectActive && !SimulationSystem.V0._1.Manager.GameManager.Instance.PlayerManager.RightGrabInteractor.isSelectActive)
                    {   

                        if (simulationManager.currentState.stateGrabbables.Count > 0)
                        {
                            if (!simulationManager.currentState.GetComponent<SeriallyToggleDetect>())
                            {
                                foreach (var detect in simulationManager.currentState.objectToDetectList)
                                {
                                    if (detect.detectObject.ThisObjectIsDetectedSuccessfully)
                                    {
                                        _targetTransform = null;
                                        _DisableMesh();
                                        return;
                                    }
                                }
                            }
                            _targetTransform = simulationManager.currentState.stateGrabbables[0].transform;
                        }
                        else
                        {
                            if (!DisableGuidanceArrow)
                            {
                                if (simulationManager.currentState.GetComponent<SeriallyToggleDetect>())
                                {
                                    int activeDetect = simulationManager.currentState.GetComponent<SeriallyToggleDetect>().Activedetect;
                                    _targetTransform = simulationManager.currentState.GetComponent<SeriallyToggleDetect>()
                                        .StateDetects[activeDetect].transform;
                                }
                                else
                                {
                                    _targetTransform = simulationManager.currentState.objectToDetectList[0].detectObject.transform;
                                }
                            }
                            else
                            {
                                _targetTransform = null;
                                //_DisableGuidanceArrow();
                                _DisableMesh();
                            }
                        }
                    }

                    break;
              
            }
            #endregion
        }
      
        public void _DisableMesh()
        {
            arrow.gameObject.SetActive(false);
        }
        public void _EnableMesh()
        {
            arrow.gameObject.SetActive(true);
        }
    }
}