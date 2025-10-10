using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Assessment.Interface;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine;

namespace SimulationSystem.V0._1.Simulation
{
    public partial class SimulationState
    {

        #region Configure Detectables

        public void ConfigureDetects(int index)
        {
            foreach (var objectToDetect in objectToDetectList)
            {
                //if (!objectToDetect.detectObject.TryGetComponent<DetectObject>(out var detect)) continue;

                _stateDetects.Add(objectToDetect.detectObject);

                objectToDetect.detectObject.objectsToDetectPerStateDictionary.Add(index, new Dictionary<Collider, bool>());

                foreach (var gameObjectToDetect in objectToDetect.gameObjectsToDetect)
                {
                    objectToDetect.detectObject.objectsToDetectPerStateDictionary[index].Add(gameObjectToDetect, false);
                }

              /*  foreach (var gameObjectName in objectToDetect.gameObjectToDetectNames)
                {
                    objectToDetect.detectObject.objectsToDetectPerStateDictionary[index]
                        .Add(GameObject.Find(gameObjectName), false);
                }*/
                
                objectToDetect.detectObject.onDetectionComplete.AddListener((() =>
                {

                    if ((objectToDetect.shouldMoveToNextState)&& objectToDetect.detectObject.ThisIsCorrectObject) SimulationManager.instance.NextState();
                }));
            }
        }

        #endregion
        
        #region Toggle State Detects

        public void SwitchStateDetects(bool shouldEnable)
        {
            foreach (var detect in _stateDetects)
            {
                SimulationStateDetectManager.EnableDetectAbstract(detect, shouldEnable);

                if(SimulationManager.instance.isAssessmentMode)
                {
                    if (TryGetComponent<AssessmentController>(out AssessmentController assessmentController))
                    {
                        if (!assessmentController.turnOffDetectVizThisStep)
                        {
                            detect.GetComponent<DetectObject>().SwitchDetectHintVisuals(true);
                        }
                        else
                        {
                            detect.GetComponent<DetectObject>().SwitchDetectHintVisuals(false);
                        }
                    }
                }
                else
                {
                    detect.GetComponent<DetectObject>().SwitchDetectHintVisuals(true);
                }
            }
        }

        public List<ObjectToDetectPerState> getObjectToDetectPerStates()
        {
            return objectToDetectList;
        }

        #endregion

    }
}