using UnityEngine;

public class Flicker : MonoBehaviour
{
    // minimum and maximum time in seconds between each toggle
    [SerializeField] private float minTime = 0.1f;
    [SerializeField] private float maxTime = 0.5f;

    void Start()
    {
        // invoke the Toggle method at a random interval
        Invoke("Toggle", Random.Range(minTime, maxTime));
    }

    void Toggle()
    {
        // toggle the game object's active state
        gameObject.SetActive(!gameObject.activeSelf);
        // invoke the Toggle method at a new random interval
        Invoke("Toggle", Random.Range(minTime, maxTime));
    }
}