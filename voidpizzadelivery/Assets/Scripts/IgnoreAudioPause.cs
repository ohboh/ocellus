using UnityEngine;

public class IgnoreAudioPause : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }
}
