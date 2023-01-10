using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Boolean to track if the game is paused
    [HideInInspector] public bool isPaused = false;

    // Boolean to track if a job has been completed
    private bool isJobDone = false;

    // Reference to the pause menu GameObject
    [SerializeField] private GameObject pauseMenu;

    // Reference to the cursor texture
    [SerializeField] private Texture2D cursor; 

    void Awake()
    {
        // Set the cursor to the cursor texture
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        // Reset pause state
        isPaused = false;
        isJobDone = false;

        // Toggle pause state if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1 - Time.timeScale;
        }

        // Set pause state if time scale is 0
        if (Time.timeScale == 0)
        {
            isPaused = true;
        }

        // If the game is paused and a job has not been done yet
        if (isPaused &! isJobDone)
        {
            // Pause audio, show cursor, and activate pause menu
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);

            // Set job as done
            isJobDone = true;
        }

        // If the game is not paused and a job has not been done yet
        if (!isPaused &! isJobDone)
        {
            // Hide cursor, lock cursor, and deactivate pause menu
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            AudioListener.pause = false;

            // Set job as done
            isJobDone = true;
        }
    }

    // Public method to reset the current scene
    public void Reset() {
        // Set time scale to 1 and reload the active scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Public method to quit the application
    public void Quit() {
        Application.Quit();
    }
}
