using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Declare variables for mouse sensitivity, walk speed, and run speed
    public float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;

    // Declare a variable for the sound sphere
    private SphereCollider soundSphere;
    private BoxCollider jumpDetector;
    

    // Declare variables for movement states
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isHoldingBreath;
    [SerializeField] private bool stillHoldingBreath;

    // Declare a variable for fatigue state
    [SerializeField] private bool isFatigued;

    // Declare variables for stamina and stamina management
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float decreaseRate;
    [SerializeField] private float increaseRate;

    // Declare a variable for speed
    [SerializeField] private float speed;

    // Declare variables for audio and contact point objects
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject contactPoint;


    // Declare variables for character controller and capsule collider components
    private CharacterController controller;
    private CapsuleCollider capCollider;

    void Awake()
    {
        // get the CharacterController component attached to this GameObject
        controller = GetComponent<CharacterController>();

        // set the initial stamina to the maximum stamina value
        stamina = maxStamina;

        // get the CapsuleCollider component attached to this GameObject
        capCollider = GetComponent<CapsuleCollider>();

        // get the SphereCollider component attached to a child GameObject of this GameObject
        soundSphere = GetComponentInChildren<SphereCollider>();
        jumpDetector = GetComponentInChildren<BoxCollider>();

        // play the "whiteNoise" audio clip
        audioManager.Play("whiteNoise");
    }

    
    void Update()
    {
        // check if the game is paused
        if (!GetComponent<Pause>().isPaused)
        {
            // update movement and mouse look, and trigger speed based sound effects
            UpdateMovement();
            UpdateMouseLook();
            SpeedBasedSFX();
        }
    }

    void UpdateMouseLook()
    {
        // get raw mouse input
        float mouseDelta = Input.GetAxisRaw("Mouse X");

        // rotate the player based on mouse input and sensitivity
        transform.Rotate(Vector3.up * mouseDelta * mouseSensitivity);
    }

    void UpdateMovement()
    {
        // get raw input for horizontal and vertical movement
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // normalize input to prevent faster diagonal movement
        inputDirection.Normalize();

        // initialize velocity
        Vector3 velocity = Vector3.zero;
        // set isWalking to true if "WASD" is input while not running
        isWalking = Convert.ToBoolean(Input.GetAxisRaw("Horizontal")) &! isRunning || Convert.ToBoolean(Input.GetAxisRaw("Vertical")) &! isRunning;
        // set isRunning to true if left shift is being held
        isRunning = Input.GetKey(KeyCode.LeftShift);
        // set isHoldingBreath to true if left control is being held
        isHoldingBreath = Input.GetKey(KeyCode.LeftControl);

        // set stillHoldingBreath to true if left control was just pressed
        stillHoldingBreath = Input.GetKeyDown(KeyCode.LeftControl);

        // play idle breath audio
        audioManager.Play("idleBreath");

        if (isRunning &! isFatigued)
        {
            // set velocity to run speed and direction of input
            velocity = (transform.forward * inputDirection.y + transform.right * inputDirection.x) * runSpeed;
            // enable sound sphere and jumpscare detector, and also set radius
            soundSphere.enabled = true;
            soundSphere.radius = 20f;
            jumpDetector.enabled = true;

            audioManager.Run("idleBreath");
            DecreaseStamina();
        }

        else if (isWalking)
        {
            // set velocity to walk speed and direction of input
            velocity = (transform.forward * inputDirection.y + transform.right * inputDirection.x) * walkSpeed;
            // enable sound sphere and jumpscare detector, and also set radius of sound sphere to 10
            soundSphere.enabled = true;
            soundSphere.radius = 10f;
            jumpDetector.enabled = true;

            // regenerate stamina
            RegenerateStamina();
        }

        else if (isHoldingBreath &! isFatigued)
        {
            // disable sound sphere and jumpscare detector
            soundSphere.enabled = false;
            jumpDetector.enabled = false;

            // stop playing idle breath audio
            audioManager.Stop("idleBreath");
            if (stillHoldingBreath)
            {
                // play hold breath audio
                audioManager.Play("holdBreath");
            }

            DecreaseStamina();
        }

        else
        {
            // Enable sound sphere and set radius of sound sphere to 5
            soundSphere.enabled = true;
            soundSphere.radius = 5f;

            jumpDetector.enabled = true;
            
            if (!isHoldingBreath)
            {
                RegenerateStamina();
            }
        }
        // set speed to magnitude of velocity
        speed = controller.velocity.magnitude;

        // move character controller
        controller.Move(velocity * Time.deltaTime);
    }

    private void DecreaseStamina()
    {
        // decrease the stamina by the decrease rate multiplied by delta time if the stamina is greater than 0
        if (stamina > 0)
        {
            stamina -= decreaseRate * Time.deltaTime;
        }
        // if the stamina is 0 or less, set isFatigued to true and play the "releaseBreath" sound
        else
        {
            isFatigued = true;
            audioManager.Play("releaseBreath");

            // re-enable sound sphere and jumpscare detector, and also set radius of sound sphere to 20;
            soundSphere.enabled = true;
            soundSphere.radius = 20f;
            jumpDetector.enabled = true;
        }

    }

    private void RegenerateStamina()
    {
        // If the stamina is less than or equal to the max stamina, increase the stamina by the increase rate multiplied by delta time
        if (stamina <= maxStamina)
        {
            stamina += increaseRate * Time.deltaTime;
            // If the stamina is greater than or equal to half of the max stamina, set isFatigued to false
            if (stamina >= maxStamina / 2)
            {
                isFatigued = false;
            }
        }
        // If the stamina is greater than the max stamina, set it to the max stamina
        else
        {
            stamina = maxStamina;
        }
    }

    private void SpeedBasedSFX()
    {
        // if the speed is greater than or equal to 10, play the "footsteps" sound
        if (speed >= 10)
        {
            audioManager.Play("footsteps");
            // if the speed is less than 14, slow down the "footsteps" sound
            if (speed < 14)
            {
                audioManager.Slow("footsteps");
            }
            // if the speed is greater than or equal to 20, play the "footsteps" sound at a faster speed
            if (speed >= 20)
            {
                audioManager.Run("footsteps");
            }
        }
        // if the speed is less than 10, stop playing the "footsteps" sound
        else
        {
            audioManager.Stop("footsteps");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "wall")
        {
            // set the position of the contactPoint to the hit point with an adjusted y position
            contactPoint.transform.position = new Vector3(hit.point.x, 0.65f, hit.point.z);

            // if the speed is greater than or equal to 10 and less than 14.5, play the "hand" sound
            if (speed >= 10 && speed < 14.5)
            {
                audioManager.Play("hand");
            }
        }
    }

}
