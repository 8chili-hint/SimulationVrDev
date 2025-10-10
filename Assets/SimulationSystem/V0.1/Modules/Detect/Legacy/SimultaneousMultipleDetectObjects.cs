using System;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect.Legacy
{
    public class SimultaneousMultipleDetectObjects : DetectAbstract
    {
        [Serializable]
        public struct DetectObjectPerState
        {
            public int stateIndex;
            public List<DetectObject> detectObjects;
        }

        [SerializeField] private List<DetectObjectPerState> detectObjectPerStateList;
    
        public readonly Dictionary<int, Dictionary<DetectObject, bool>> detectObjectPerState = new Dictionary<int, Dictionary<DetectObject, bool>>();
    
        private void Start()
        {
            Initialize();
        }

        private void CreateDetectObjectDictionary()
        {
            foreach (var detectObject in detectObjectPerStateList)
            {
                detectObjectPerState.Add(detectObject.stateIndex, new Dictionary<DetectObject, bool>());

                foreach (var detectObjectComponent in detectObject.detectObjects)
                {
                    detectObjectPerState[detectObject.stateIndex].Add(detectObjectComponent, false);
                }    
            }
        }
        
        public void OnDetectObjectStart(DetectObject detectObject)
        {
            detectObjectPerState[SimulationManager.instance.currentStateIndex][detectObject] = true;

            if (detectObjectPerState[SimulationManager.instance.currentStateIndex].ContainsValue(false)) return;
        
            OnDetectionStart();
        }
        
        public void OnDetectObjectStop(DetectObject detectObject)
        {
            if (!detectObjectPerState[SimulationManager.instance.currentStateIndex].ContainsValue(false))
            {
                OnDetectionStop();
            }

            detectObjectPerState[SimulationManager.instance.currentStateIndex][detectObject] = false;
        }
        
        protected override void Initialize()
        {
            base.Initialize();

            CreateDetectObjectDictionary();
        }
    }
}