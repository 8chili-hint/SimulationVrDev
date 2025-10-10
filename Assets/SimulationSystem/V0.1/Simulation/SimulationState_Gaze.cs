using System;
using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Assessment;
using SimulationSystem.V0._1.Assessment.Interface;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Simulation.Manager;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimulationSystem.V0._1.Simulation
{
    public partial class SimulationState
    {
        private GameObject rayObject;
        private void GetRay()
        {
            rayObject = objectToDetectList[0].gameObjectsToDetect[0].gameObject;
        }

        private void StartAllRaycast()
        {
            for(int i = 0; i < objectToDetectList.Count; i++)
            {
                StartCoroutine(StartRaycast(i));
            }
        }

        private void StopAllRaycast()
        {
            for(int i = 0; i < objectToDetectList.Count; i++)
            {
                StopCoroutine(StartRaycast(i));
            }
        }

        IEnumerator StartRaycast(int index)
        {
            if (rayObject == null)
            {
                yield break;
            }
            else
            {
                while (true)
                {
                    Ray ray = new Ray(rayObject.transform.position, rayObject.transform.forward);
                    Debug.DrawRay(rayObject.transform.position, rayObject.transform.forward * 2, Color.red);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 2f))
                    {
                        if (hit.collider.gameObject.name == objectToDetectList[index].detectObject.name)
                        {
                            Debug.Log("Hit object : " + hit.collider.gameObject.name);
                            hit.transform.gameObject.GetComponent<DetectObject>().OnGazeInitiated();
                        }
                        else
                        {
                            Debug.Log(" Did not hit");
                            objectToDetectList[index].detectObject.GetComponent<DetectObject>().OnGazeEnded();
                        }
                    }

                    if(SimulationManager.instance.currentState.stateType!= StateType.Gaze)
                    {
                        yield break;
                    }
                    yield return null;
                }
            }
            
        }
    }
}