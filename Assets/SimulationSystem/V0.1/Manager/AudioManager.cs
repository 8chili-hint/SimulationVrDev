using UnityEngine;

namespace SimulationSystem.V0._1.Manager
{
    public class AudioManager : MonoBehaviour
    {
        // Audio players components.
        public AudioSource EffectsSource;
        public AudioSource SoundSource;

        [SerializeField] private AudioClip detectClip;
        [SerializeField] private AudioClip successClip;

        // Play a single clip through the sound effects source.
        public void PlayEffect(AudioClip clip)
        {
            EffectsSource.clip = clip;
            EffectsSource.Play();
        }

        public void PlayDetectEffect()
        {
            EffectsSource.clip = detectClip;
            EffectsSource.Play();
        }
    
        public void PlaySuccessEffect()
        {
            EffectsSource.clip = successClip;
            EffectsSource.Play();
        }

        // Play a single clip through the sound source.
        public void PlaySound(AudioClip clip)
        {
            SoundSource.clip = clip;
            SoundSource.Play();
        }
    }
}