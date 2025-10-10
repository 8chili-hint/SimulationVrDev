using System.Collections.Generic;
using System.IO;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class CSVMaker : MonoBehaviour
    {
        public List<States> stateList = new List<States>();
    
        private void Awake()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.TryGetComponent<SimulationState>(out SimulationState state))
                    {
                        States stateTexts = new States();
                        stateTexts.name = child.gameObject.name;
                        StateName.Add(child.gameObject.name);
                        stateTexts.promptText = state.textPrompt;
                        StatePromptText.Add(state.textPrompt);
                        stateList.Add(stateTexts);
                    }
                }
            }

            StatesList statelist = new StatesList();
            statelist.states = stateList;
            string json = JsonUtility.ToJson(statelist);
            CSVMake();
        }
        public List<string> StateName = new List<string>();
        public List<string> StatePromptText = new List<string>();
    
        void CSVMake()
        {
            string filePath = Application.persistentDataPath + "Saved_Inventory.csv";
 
            StreamWriter writer = new StreamWriter(filePath);
 
            writer.WriteLine("StateName,StatePromptText");
        
            for (int i = 0; i < Mathf.Max(StateName.Count, StatePromptText.Count); ++i)
            {
                if (i < StateName.Count) writer.Write(StateName[i]);
                writer.Write(",");
                if (i < StatePromptText.Count) writer.Write(StatePromptText[i]);
                writer.Write(System.Environment.NewLine);
            }
        
            writer.Flush();
            writer.Close();
        }
 
        private string getPath()
        {
#if UNITY_EDITOR
            return Application.dataPath + "/Data/"  + "Saved_Inventory.csv";
            //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_Inventory.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
#else
        return Application.dataPath +"/"+"Saved_Inventory.csv";
#endif
        }
    }

    public class StatesList
    {
        public List<States> states;
    }

    public class States
    {
        public string name;
        public string promptText;
    }
}