using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect.Legacy
{
    [RequireComponent(typeof(Detectable))]
    public class DetectMultiple : DetectAbstract
    {
        private float detectTimer2 = 3;
        [SerializeField] List<DetectObject> detectObjects = new List<DetectObject>();

        [Space(10)]
        public UnityEvent onDetect;
        public UnityEvent<float> onAllDetecting;
        public UnityEvent onAllDetectComplete;

        private float timer = 0;
        private bool canDetect = true;
        private bool firstDetect = true;

        public void Initialize()
        {
            timer = 0;
            
            detectObjects = GetComponentsInChildren<DetectObject>().ToList();
        }

        public void UpdateLoop()
        {
            if (canDetect)
            {
                if (detectObjects.All(g => g.state == DetectStates.Detect))
                {
                    if (firstDetect)
                    {
                        onDetect?.Invoke();
                        firstDetect = false;
                    }
                    timer += Time.deltaTime;
                    
                    if (timer > detectTimer2)
                    {
                        onAllDetectComplete?.Invoke();
                        timer = 0;
                        canDetect = false;
                    }
                    else 
                        onAllDetecting?.Invoke(Mathf.InverseLerp(0, detectTimer2, timer));
                }
            }
        }

        public void OnObjectEnter(Collider other)
        {
            //
        }

        public void OnObjectStay(Collider other)
        {
            //
        }

        public void OnObjectExit(Collider other)
        {
        }

        public void StartDetection()
        {
            canDetect = true;
        }

        public void StopDetection()
        {
            canDetect = false;
        }

        public void ResetDetect()
        {
            timer = 0;
            firstDetect = true;
            StopDetection();
        }
    }
}
