using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Assessment.Assessment_Types;
using SimulationSystem.V0._1.Modules.Detect.Legacy;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect
{
    [RequireComponent(typeof(Collider))]
    public class DetectObject : DetectAbstract
    {
        public readonly Dictionary<int, Dictionary<Collider, bool>> objectsToDetectPerStateDictionary =
            new Dictionary<int, Dictionary<Collider, bool>>();

        private SimultaneousMultipleDetectObjects _simultaneousMultipleDetectObjectComponent;
        private int _lastStateObjectWasDetectedIn;
        private List<GameObject> _hintVisuals = new List<GameObject>();

        [SerializeField] private List<Collider> _wrongDetectObjects;

        public UnityEvent onWrongDetectionComplete;


        //We are using this to make sure a wrong object is not Passed instead of the correct one;
        [HideInInspector] public bool ThisIsCorrectObject;
        [HideInInspector] public bool ThisObjectIsDetectedSuccessfully;
        [HideInInspector] public bool ThisIsWrongDetect;
        #region IdetectImplementation
      
        private void Start()
        {
            Initialize();


            onDetectionComplete.AddListener(() => { if (ThisIsWrongDetect) { AssessmentManager.DeductScore(AssessmentType.Detect); onWrongDetectionComplete?.Invoke(); } });

            //Adding Listener Here for checking if the Correct object is detected and if it is then only it will go to the next one!!
            onDetectionComplete.AddListener(() => {
                if (ThisIsCorrectObject) { ThisObjectIsDetectedSuccessfully = true; }
                if (SimulationManager.instance.isAssessmentMode)
                {
                    SimulationManager.instance._hintButtonComponent.interactable = false;
                }
            });
            onDetected.AddListener(() => {
                if (SimulationManager.instance.isAssessmentMode)
                {
                    SimulationManager.instance._hintButtonComponent.interactable = false;
                }
            });

            onUnDetect.AddListener(() =>
            {
                if (SimulationManager.instance.isAssessmentMode)
                {
                    SimulationManager.instance._hintButtonComponent.interactable = true;
                }
            });
            onDetectionComplete.AddListener(()
              =>
            {
                ScaleDownDetectHintVisual();
            });
            //if this above line is not really doing anything for the Simultaneous objects, I would suggest=> use undetect and turn both the bools off and probably turn it on after the simultaneous Detection complete. 
            onWrongDetectionComplete.AddListener(() => { disableThisDetect(); });

        }
        public void HintViz()
        {
            //This is Called in "Setmode" in SimManager 
          

            foreach (Transform child in transform)
            {
               if(SimulationManager.instance.TurnOffDetectVizUsing == DetectVizmode.Renderer)
               {
                    if (child.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    {
                        _hintVisuals.Add(renderer.gameObject);
                    }

               }
                else
                {
                    _hintVisuals.Add(transform.GetChild(0).gameObject);
                }
            }
        }
        public void SetupWrongDetects(Transform T)
        {
            HintViz();
            if (!ThisIsWrongDetect)
            {
                if (SimulationManager.instance.isAssessmentMode && T.TryGetComponent(out DetectAssessment detectassessment))
                {
                    var CurrentState = T.GetComponent<SimulationState>();
                    foreach (var tempDetects in detectassessment.WrongDetects)
                    {
                       // tempDetects._wrongDetectObjects = new List<GameObject>();

                        if (CurrentState.objectToDetectList.Count > 0)
                        {
                            foreach (var StateGrabbables in CurrentState.objectToDetectList)
                            {
                                foreach (var WrongGameObject in StateGrabbables.gameObjectsToDetect)
                                {
                                    tempDetects._wrongDetectObjects.Add(WrongGameObject);
                                }
                            }
                        }
                        tempDetects.gameObject.SetActive(false);
                    }

                    detectassessment.AddListeners();
                }
            }


        }

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            onDetectingEvent.AddListener((float a) =>  ThisiscorrectObjectPatch(a) );
            ThisObjectIsDetectedSuccessfully = false;

            if (TryGetComponent<Collider>(out var col))
            {
                col.isTrigger = true;
            }

            if (TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }



            /* if (transform.parent.TryGetComponent<SimultaneousMultipleDetectObjects>(out var component))
             {
                 _simultaneousMultipleDetectObjectComponent = component;
             }*/


        }

        public void SwitchDetectHintVisuals(bool shouldActivate)
        {
            foreach(var hintVisual in _hintVisuals)
            {
                hintVisual.SetActive(shouldActivate);
            }
        }
        public void ScaleDownDetectHintVisual()
        {
            foreach (var hintVisual in _hintVisuals)
            {
                hintVisual.transform.localScale = Vector3.zero;
            }
        }
        private void ThisiscorrectObjectPatch(float a)
        {
            ThisIsCorrectObject = true;

        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (SimulationManager.instance.isAssessmentMode && SimulationManager.instance.currentState.TryGetComponent<DetectAssessment>(out DetectAssessment assessment))
            {
               
                if (_wrongDetectObjects.Contains(other))
                {
                    OnWrongDetectObjectEnter();
                    
                  
                    return;
                }
            }
            if (!ThisIsWrongDetect)
            {

                if (ShouldDetectObjectInCurrentState(other))
                {
                    OnObjectToDetectEnter(other, IsSimultaneousDetectInCurrentState());
                    SwitchDetectHintVisuals(false);
                }
            }
        }
        
        public void OnTriggerExit(Collider other)
        {
            if (SimulationManager.instance.isAssessmentMode && SimulationManager.instance.currentState.TryGetComponent<DetectAssessment>(out DetectAssessment assessment))
            {
                if (_wrongDetectObjects.Contains(other))
                {
                    OnwrongDetectionStop();
                }
            }
            SimulationManager sim = SimulationManager.instance;

            if (ShouldDetectObjectInCurrentState(other))
            {
                OnObjectToDetectExit(other, IsSimultaneousDetectInCurrentState());

                if (hintIsTaken)
                {
                    SwitchDetectHintVisuals(true);
                }
                else
                {
                    if (!sim.isAssessmentMode)
                    {
                        SwitchDetectHintVisuals(true);
                    }

                    else if (sim.isAssessmentMode && sim.currentState.GetComponent<AssessmentController>().turnOffDetectVizThisStep)
                    {
                        //dont do anything 
                        SwitchDetectHintVisuals(false);

                    }

                    else if (sim.isAssessmentMode && !sim.currentState.GetComponent<AssessmentController>().turnOffDetectVizThisStep)
                    {
                        SwitchDetectHintVisuals(true);
                    }
                }
            }
        }

        private bool ShouldDetectObjectInCurrentState(Collider detectedGameObject)
        {
            if (!objectsToDetectPerStateDictionary.ContainsKey(SimulationManager.instance.currentStateIndex))
                return false;

            return objectsToDetectPerStateDictionary[SimulationManager.instance.currentStateIndex].ContainsKey(detectedGameObject);
        }

        private bool IsSimultaneousDetectInCurrentState()
        {
            if (_simultaneousMultipleDetectObjectComponent == null) return false;
            
            if (_simultaneousMultipleDetectObjectComponent.detectObjectPerState.ContainsKey(SimulationManager.instance.currentStateIndex))
            {
                return true;
            }
            
            if (_simultaneousMultipleDetectObjectComponent.detectObjectPerState[SimulationManager.instance.currentStateIndex].ContainsKey(this))
            {
                return true;
            }

            return false;
        }

        private void OnObjectToDetectEnter(Collider detectedGameObject, bool isSimultaneousDetectInCurrentState)
        {
            objectsToDetectPerStateDictionary[SimulationManager.instance.currentStateIndex][detectedGameObject] = true;

            if (objectsToDetectPerStateDictionary[SimulationManager.instance.currentStateIndex].ContainsValue(false)) return;

            if (isSimultaneousDetectInCurrentState) _simultaneousMultipleDetectObjectComponent.OnDetectObjectStart(this);
            else
            {
                OnDetectionStart(); 
                ThisIsCorrectObject = true;
            }
        }

        private void OnWrongDetectObjectEnter()
        {
            OnWrongDetectionStart();
        }


        private void OnwrongDetectionStop()
        {
            OnWrongDetectionStop();
        }
        private void OnObjectToDetectExit(Collider detectedGameObject, bool isSimultaneousDetectInCurrentState)
        {
            if (!objectsToDetectPerStateDictionary[SimulationManager.instance.currentStateIndex].ContainsValue(false))
            {
                if (isSimultaneousDetectInCurrentState) _simultaneousMultipleDetectObjectComponent.OnDetectObjectStop(this);
                else OnDetectionStop();
            }
            
            objectsToDetectPerStateDictionary[SimulationManager.instance.currentStateIndex][detectedGameObject] = false;
        }

        private void OnDisable()
        {
            ResetDetect();
        }

        private void disableThisDetect()
        {
            if (ThisIsWrongDetect)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void OnGazeInitiated()
        {
            Debug.Log("Gaze initiated");
            OnDetectionStart();
        }

        public void OnGazeEnded()
        {
            Debug.Log("Gaze Ended");
            OnDetectionStop();
        }
    }
}