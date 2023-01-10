using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    // key to store and retrieve the volume value in PlayerPrefs
    const string VOLUME_KEY = "volume";

    void Start()
    {
        // Get the saved volume value from PlayerPrefs, or use 0.5 as the default value if no value was saved
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.5f);
        // set the volume
        AudioListener.volume = volume;
        // set the value of the slider
        volumeSlider.value = volume;
    }

    public void SetVolume(float volume)
    {
        // Set the volume of the audio listener and save it to PlayerPrefs
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
    }
}