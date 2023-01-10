using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    // Actors
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject hands;
    [SerializeField] private GameObject handsDone;
    [SerializeField] private GameObject video;
    [SerializeField] private GameObject nameReveal;
    [SerializeField] private GameObject pizza;
    [SerializeField] private GameObject credits;
    private Animator anim;
    private Camera cam;
    private GameObject camGameObject;

    // Chase
    [SerializeField] private float chaseUpdateTime = 0.1f;
    [SerializeField] bool isPatroling, isChasing = false;


    // Patrol
    [SerializeField] private Transform[] waypoints;
    private int destPoint; 

    // States
    [SerializeField] private float hearRange, warnRange;
    [SerializeField] private bool playerInHearRange, playerInWarnRange;
    [SerializeField] private float jumpscareDelay = 1f;

    private Vector3 playerLastPosition;

    private void Awake()
    {
        // initialize NavMesh agent
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        // initialize the animator of the eye
        anim = eye.GetComponent<Animator>();

        // initizalize the camera and the camera game object;
        cam = Camera.main;
        camGameObject = cam.gameObject;

        // disable the hand GameObject
        hands.SetActive(false);

        // disable autobraking for the agent
        agent.autoBraking = false;
    }

    private void Start()
    {
        // play audio "confess" and start patrolling after script is initialized
        PlayAudio("confess");
        Patrol();
    }

    // mmmmm i like lasagna...
    private void Update() {
        UpdateRangeStates();
        CheckForChaseStart();
        UpdateChaseState();
        AfterJumpscareLook();
        UpdateJumpscareState();
        UpdateColliderRadius();
    }

    private void OnTriggerEnter(Collider other) {
        // if the game object collided with has the tag "MainCamera" start the jumpscare coroutine
        if (other.gameObject.tag == "MainCamera")
        {
            StartCoroutine(Jumpscare());
        }
    }

    private void PlayAudio(string audioName)
    {
        audioManager.Play(audioName);
    }

    private void StopAudio(string audioName)
    {
        audioManager.Stop(audioName);
    }

    private void UpdateRangeStates()
    {
        // setup agent awareness range
        playerInHearRange = Physics.CheckSphere(transform.position, hearRange, whatIsPlayer);
        playerInWarnRange = Physics.CheckSphere(transform.position, warnRange, whatIsPlayer);
    }

    private void CheckForChaseStart()
    {
        if (playerInHearRange &! isChasing)
        {
            // find player's last position based on where the player is last heard by the agent
            playerLastPosition = playerTransform.position;
            agent.SetDestination(transform.position);

            // start the chasing coroutine, and set the chasing bool to true
            StartCoroutine(ChaseTarget());
            isChasing = true;
        }

        if (playerInWarnRange)
        {
            PlayAudio("giveth");
        }
    }

    private void UpdateChaseState()
    {
        if (isChasing)
        {
            UpdateEye();
            hands.SetActive(true);
        }

        if (!agent.pathPending && agent.remainingDistance < 2f &! isChasing)
        {
            StopCoroutine(ChaseTarget());
            Patrol();
            isPatroling = true;
            hands.SetActive(false);
        }
    }

    private void AfterJumpscareLook()
    {
        // update both the eye and the camera to look at each other in the x-axis if the player controller script is disabled
        if (player.GetComponent<PlayerController>().enabled == false)
        {
            UpdateCamera();
            UpdateEye();
        }
    }

    private void UpdateJumpscareState()
    {
        if (camGameObject.GetComponent<Collector>().collectiblesCollected == 8)
        {
            GetComponent<CapsuleCollider>().radius = 1f;
        }
        else
        {
            StopCoroutine(ChaseTarget());

            isChasing = false;
        }
    }

    private void UpdateColliderRadius()
    {
        if (camGameObject.GetComponent<Collector>().collectiblesCollected == 8)
        {
            GetComponent<CapsuleCollider>().radius = 1f;
        }
    }

    // find player's last position based on where the player is last heard by the agent and sets the position to be the agent's next destination
    private IEnumerator ChaseTarget()
    {
        StopAudio("monsterPatrol");
        PlayAudio("monsterChase");
        agent.SetDestination(playerLastPosition);

        while (isChasing)
        {
            yield return new WaitForSeconds(chaseUpdateTime);
            agent.SetDestination(playerLastPosition);
        }
    }

    private void Patrol()
    {
        StopAudio("monsterChase");
        PlayAudio("monsterPatrol");

        GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (waypoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = waypoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % waypoints.Length;
    }

    private void UpdateEye()
    {
        // update eye position to look at player
        eye.transform.LookAt(player.transform);
        eye.transform.rotation = Quaternion.Euler(0f, eye.transform.rotation.eulerAngles.y, 0f);
    }

    private void UpdateCamera()
    {
        // update camera position to look at eye
        camGameObject.transform.LookAt(eye.transform);
        camGameObject.transform.rotation = Quaternion.Euler(0f, camGameObject.transform.rotation.eulerAngles.y, 0f);
    }

    private IEnumerator Jumpscare()
    {
        // disable enemy chase behavior and player movement
        isChasing = false;
        player.GetComponent<PlayerController>().enabled = false;

        // if the player has not collected all collectibles
        if (camGameObject.GetComponent<Collector>().collectiblesCollected < 8)
        {
            // play audio and disable audio listener volume
            PlayAudio("itwillbeok");
            PlayAudio("pizzaEnd");
            yield return new WaitForSeconds(0.5f);
            AudioListener.volume = 0;

            // enable the video game object and wait for 3 seconds
            video.SetActive(true);
            yield return new WaitForSeconds(3f);

            // if the player's x position is a multiple of 3, enable the name reveal game object and set its text
            if (Mathf.Round(playerTransform.position.x) % 3 == 0)
            {
                nameReveal.SetActive(true);
                nameReveal.GetComponent<TextMeshProUGUI>().text = "try holding your breath";

                // if the player's x position is a multiple of 6, set the name reveal text to the device name
                if (Mathf.Round(playerTransform.position.x) % 6 == 0)
                {
                    nameReveal.GetComponent<TextMeshProUGUI>().text = SystemInfo.deviceName.ToLower();
                }
            }

            // wait for 5 seconds and then reload the current scene
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // if the player has collected all collectibles
        else
        {
            // disable the enemy agent, destroy the pizza and hands game objects, and enable the hands done game object
            agent.enabled = false;
            Destroy(pizza);
            Destroy(hands);
            handsDone.SetActive(true);
            // disable audio listener volume and wait for 6 seconds
            AudioListener.volume = 0;
            yield return new WaitForSeconds(6f);

            // enable the credits game object
            credits.SetActive(true);
        }
    }
}

