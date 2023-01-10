using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    // key to store and retrieve the sensitivity value in PlayerPrefs
    const string SENSITIVITY_KEY = "sensitivity";
    [SerializeField] private GameObject player;
    public float sensitivity;

    void Start()
    {
        // get the saved sensitivity value from PlayerPrefs, or use 3 as the default value if no value was saved
        sensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, 3f);
        // set the sensitivity
        sensitivitySlider.value = sensitivity;
    }

    public void SetSensitivity(float sensitivity)
    {
        player.GetComponent<PlayerController>().mouseSensitivity = sensitivity*10;
        // set the sensitivity and save it to PlayerPrefs
        sensitivitySlider.value = sensitivity;
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, sensitivity);
    }
}