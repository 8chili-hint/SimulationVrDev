using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUihandler : MonoBehaviour
{
    public TextMeshProUGUI PrimaryText, SecondaryText;
    public void InjectText( SimulationSystem.V0._1.Simulation.SimulationState StepPromt)
    {
        PrimaryText.text = StepPromt.textPrompt;
        GetComponentInChildren<Button>().enabled = true;
    }

    public void InjectSecondarytext(string text)
    {
        SecondaryText.text = text;
    }
}
