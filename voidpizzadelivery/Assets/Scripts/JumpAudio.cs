using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAudio : MonoBehaviour
{
    private AudioSource audioSrc;
    void Awake() {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.ignoreListenerVolume = true;
        audioSrc.Play();
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown() {
       yield return new WaitForSeconds(3f);
       audioSrc.Stop();
    }
}
