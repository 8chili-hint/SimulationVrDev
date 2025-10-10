using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Modules.Detect.Utility
{
    public class SeriallyToggleDetect : MonoBehaviour
    {
        [Header("These detects will be turned on Sequentially as per the order in list below")]
        public List<DetectObject> StateDetects;

        public int Activedetect = 0;

        [Header("All Detect Overrides")]
        public float DetectTimer;

        [Header("Last Detect OverRides")]
        public bool DontDoAnything;
        public ObjectBehaviourOnDetect objectBehaviourOnDetect;
        public Transform DestinationTransform;
        public bool ForceUngrab;
        public bool deRegisterThisObjectOnDetect;
        public bool NextStateOnDetectionComplete;

        public UnityEvent Ondetected;
        public UnityEvent<float> Ondetecting;
        public UnityEvent onDetectionComplete;
        private void Awake()
        {
            foreach (DetectObject obj in StateDetects)
            {
                obj.detectTimer = DetectTimer;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            foreach (var detect in StateDetects)
            {
                detect.onDetectionComplete.AddListener(() => OnDetectioncompleted());
            }
            AddListeners();
        }

        void AddListeners()
        {
            GetComponent<SimulationState>().onStateStart.AddListener(() => { toggleAllOff(); });
        }

        void toggleAllOff()
        {
            for (int i = 0; i < StateDetects.Count; i++)
            {
                if (i == 0)
                {

                    continue;
                }
                else
                {
                    StateDetects[i].DontDoAnything = true;
                    StateDetects[i].gameObject.SetActive(false);
                    StateDetects[i].GetComponent<DetectAbstract>().detectTimer = DetectTimer;

                }

            }

        }


        void OnDetectioncompleted()
        {
            Activedetect++;
            if (StateDetects.Count > 1 && Activedetect < StateDetects.Count)
            {
                //Applying the Overrides for Last Detect
                if (Activedetect == StateDetects.Count - 1)
                {
                    StateDetects[Activedetect].DontDoAnything = DontDoAnything;
                    StateDetects[Activedetect].ForceUngrabThisObject = ForceUngrab;
                    StateDetects[Activedetect].ObjectBehaviourOnDetect = objectBehaviourOnDetect;
                    StateDetects[Activedetect].DestinationTransform = DestinationTransform;

                    StateDetects[Activedetect].deRegisterThisObjectOnDetect = deRegisterThisObjectOnDetect;
                    StateDetects[Activedetect].onDetectingEvent.AddListener((float a) => { Ondetecting.Invoke(a); });
                    StateDetects[Activedetect].onDetectionComplete.AddListener(() => { SimulationManager.instance.NextState(); onDetectionComplete.Invoke(); });
                }

                DetectBehaviourAndStateListener(Activedetect);
            }


        }

        void DetectBehaviourAndStateListener(int i)
        {
            StateDetects[i - 1].onDetectionComplete.RemoveListener(() => OnDetectioncompleted());

            StateDetects[i].gameObject.SetActive(true);
            
            SimulationManager.instance.currentState.SwitchStateDetects(true);
            foreach (var detect in StateDetects)
            {
                if (StateDetects[i] == detect)
                {
                    continue;
                }
                else
                {
                    detect.gameObject.SetActive(false);
                }
            }

            if (i == StateDetects.Count - 1)
            {
                StateDetects[i].onDetectionComplete.AddListener(() => { SimulationManager.instance.NextState(); });
            }

        }

    }
}
