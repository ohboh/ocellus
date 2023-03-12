using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;

    private SphereCollider soundSphere;
    private BoxCollider jumpDetector;
    
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isHoldingBreath;
    [SerializeField] private bool stillHoldingBreath;

    [SerializeField] private bool isFatigued;

    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float decreaseRate;
    [SerializeField] private float increaseRate;

    [SerializeField] private float speed;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject contactPoint;

    private CharacterController controller;
    private CapsuleCollider capCollider;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stamina = maxStamina;
        capCollider = GetComponent<CapsuleCollider>();

        soundSphere = GetComponentInChildren<SphereCollider>();
        jumpDetector = GetComponentInChildren<BoxCollider>();

        audioManager.Play("whiteNoise");
    }

    
    void Update()
    {
        if (!GetComponent<Pause>().isPaused)
        {
            UpdateMovement();
            UpdateMouseLook();
            SpeedBasedSFX();
        }
    }

    void UpdateMouseLook()
    {
        float mouseDelta = Input.GetAxisRaw("Mouse X");
        transform.Rotate(Vector3.up * mouseDelta * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // normalize input to prevent faster diagonal movement
        inputDirection.Normalize();

        Vector3 velocity = Vector3.zero;
        isWalking = Convert.ToBoolean(Input.GetAxisRaw("Horizontal")) &! isRunning || Convert.ToBoolean(Input.GetAxisRaw("Vertical")) &! isRunning;
        isRunning = Input.GetKey(KeyCode.LeftShift);
        isHoldingBreath = Input.GetKey(KeyCode.LeftControl);

        stillHoldingBreath = Input.GetKeyDown(KeyCode.LeftControl);

        audioManager.Play("idleBreath");

        if (isRunning &! isFatigued)
        {
            // set velocity to run speed and direction of input
            velocity = (transform.forward * inputDirection.y + transform.right * inputDirection.x) * runSpeed;
            
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
            
            soundSphere.enabled = true;
            soundSphere.radius = 10f;
            jumpDetector.enabled = true;

            RegenerateStamina();
        }

        else if (isHoldingBreath &! isFatigued)
        {
            soundSphere.enabled = false;
            jumpDetector.enabled = false;

            audioManager.Stop("idleBreath");
            if (stillHoldingBreath)
            {
                audioManager.Play("holdBreath");
            }

            DecreaseStamina();
        }

        else
        {
            soundSphere.enabled = true;
            soundSphere.radius = 5f;

            jumpDetector.enabled = true;
            
            if (!isHoldingBreath)
            {
                RegenerateStamina();
            }
        }
        speed = controller.velocity.magnitude;
        controller.Move(velocity * Time.deltaTime);
    }

    private void DecreaseStamina()
    {
        if (stamina > 0)
        {
            stamina -= decreaseRate * Time.deltaTime;
        }
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
        if (stamina <= maxStamina)
        {
            stamina += increaseRate * Time.deltaTime;
            // If the stamina is greater than or equal to half of the max stamina, set isFatigued to false
            if (stamina >= maxStamina / 2)
            {
                isFatigued = false;
            }
        }
        else
        {
            stamina = maxStamina;
        }
    }

    private void SpeedBasedSFX()
    {
        if (speed >= 10)
        {
            audioManager.Play("footsteps");
            
            if (speed < 14)
            {
                audioManager.Slow("footsteps");
            }
            if (speed >= 20)
            {
                audioManager.Run("footsteps");
            }
        }
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
