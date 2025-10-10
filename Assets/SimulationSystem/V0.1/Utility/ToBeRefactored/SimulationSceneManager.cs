using UnityEngine;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class SimulationSceneManager : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetButtonDown("Menu"))   
                SceneHandler.Instance.ChangeScene("HomeScene");
            
            if(Input.GetKeyDown(KeyCode.Backspace))
                SceneHandler.Instance.ChangeScene("NeonatalSimulation");
        }
    }
}