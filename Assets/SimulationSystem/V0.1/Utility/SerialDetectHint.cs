using System.Collections;
using System.Collections.Generic;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.UI;
using UnityEngine;

public class SerialDetectHint : MonoBehaviour
{
    public UIAnimationHandler hintButton;
    public UIAnimationHandler instructionPrompt;

    public void ForceShowHintOnly()
    {
        if(SimulationManager.instance.isAssessmentMode)
        {
            hintButton.ScaleUp();
            instructionPrompt.ScaleDown();
        }
    }

    public void ForceHideInstruction()
    {
        if(!SimulationManager.instance.isAssessmentMode)
        {
            instructionPrompt.ScaleDown();
        }
    }

    public void ForceShowInstruction()
    {
        if(!SimulationManager.instance.isAssessmentMode)
        {
            instructionPrompt.ScaleUp();
        }
    }
}
