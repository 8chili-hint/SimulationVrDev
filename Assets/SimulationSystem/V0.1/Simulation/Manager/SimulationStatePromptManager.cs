using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
namespace SimulationSystem.V0._1.Simulation.Manager
{
    public static class SimulationStatePromptManager
    {
        public static TextMeshProUGUI promptText;
        public static bool isFailure = false;

        public static AudioSource promptAudioSource;



        private static SimulationState _CurrentState;
        private static bool _IsAssessmentPrompt;
        private static bool _HasAudioEnded;
        public static bool HasStateAudioEnded;

        public static void SetupPrompt(Transform promptHolder)
        {
            // var videoPlayer = promptHolder.GetChild(0).GetComponentInChildren(typeof(VideoPlayer),true);
            var audioPlayer = promptHolder.GetChild((0)).GetComponentInChildren(typeof(AudioSource), true);

            if (audioPlayer != null)
            {
                promptAudioSource = (AudioSource)audioPlayer;
            }

            var textField = promptHolder.GetChild(0).GetComponentInChildren(typeof(TextMeshProUGUI), true);
            if (textField != null) promptText = (TextMeshProUGUI)textField;
        }

        public static void DisplayPrompts()
        {
            if (isFailure) return;
            promptAudioSource.Stop();
            _CurrentState = SimulationManager.instance.currentState;
            promptText.text = SimulationManager.instance.currentState.textPrompt;
            if (_CurrentState.audioPrompt != null)
            {
                PlayAudioPrompt(_CurrentState.audioPrompt, null, false);
            }
            else if(_CurrentState.audioPrompt == null)
            {
                HasStateAudioEnded = true;
            }
            if (!SimulationManager.instance.isAssessmentMode) return;

            foreach (var grabbable in _CurrentState.stateGrabbables)
            {
                SimulationStateGrabbableManager.EnableGrabVisualisations(true, SimulationStateGrabbableManager.grabbableComponents[grabbable]);
            }

        }

        public static void CLearPrompts()
        {
            promptText.text = "";
            if (promptAudioSource != null) promptAudioSource.clip = null;
        }
        public static async void PlayAudioPrompt(AudioClip clip, Action action, bool isAssessmentPrompt)
        {
            if (!isAssessmentPrompt)
            {
                HasStateAudioEnded = false;
            }
            _IsAssessmentPrompt = isAssessmentPrompt;
            promptAudioSource.clip = clip;
            _HasAudioEnded = false;
            promptAudioSource.Play();
            //Debug.LogError("playing This clip:"+ clip.name + promptAudioSource.isPlaying);
            while (!_HasAudioEnded)
            {
                await Task.Delay(100);
                _HasAudioEnded = !promptAudioSource.isPlaying;

            }
            promptAudioSource.Stop();
            action?.Invoke();
            //Refactor Later
            if (!isAssessmentPrompt)
            {
                HasStateAudioEnded = true;
            }
            if (!_IsAssessmentPrompt)
            {
                OnAudioLoopPointReached();
            }
        }


        private static void OnAudioLoopPointReached()
        {

            if (_CurrentState.nextStateOnPromptEnd)
            {
                SimulationManager.instance.NextState();
            }
        }

    }
}