using UnityEngine;

public class IgnoreAudioVolume : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AudioSource>().ignoreListenerVolume = true;
    }
}
