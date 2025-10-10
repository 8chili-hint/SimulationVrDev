using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private OVRScreenFade OVR;
    [SerializeField] private AudioSource promptAudioSrc;
    private CharacterControllerDriver movementDriver;
    private CharacterController Cc;
    private bool isFadeAudio=true;
    public void UpdatePlayerPos(Transform newPos)
    {
        StartCoroutine(SyncPlayerPosition(newPos));
    }
    void Start()
    {
        movementDriver = GetComponent<CharacterControllerDriver>();
        Cc = GetComponent<CharacterController>();
    }
    
    IEnumerator SyncPlayerPosition(Transform pos)
    {
        OVR.FadeOut();
        Cc.enabled = false;
        movementDriver.enabled = false;
        FadeAudio(true);
        yield return new WaitForSeconds(OVR.fadeTime +1f);
        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
        OVR.FadeIn();
        Cc.enabled = true;
        movementDriver.enabled = true;
        yield return new WaitForSeconds(OVR.fadeTime);
        FadeAudio();
    }

    public void EnableAudioFade(bool enable)
    {
        isFadeAudio = enable;
    }

    void FadeAudio(bool fadeout=false)
    {
        if (isFadeAudio) 
        {
            if (fadeout == true)
            {
                promptAudioSrc.mute = true;
            }
            else
            {
                promptAudioSrc.time = 0;
                promptAudioSrc.mute = false;
                promptAudioSrc.Play();
                
            }
        }
    }
}
