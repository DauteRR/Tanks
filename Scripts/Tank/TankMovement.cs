using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int playerIdentifier;
    public float speed = 12f;
    public float turnSpeed = 180f;
    public AudioSource movementAudioSource;
    public AudioClip engineIdlingAudio;
    public AudioClip engineDrivingAudio;
    public float pitchRange = 0.2f;

    private string movementAxisName;
    private string turnAxisName;
    private Rigidbody rigidbody;
    private float movementInputValue;
    private float turnInputValue;
    private float originalPitch;
    private const float EPSILON = 0.1f;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        rigidbody.isKinematic = false;
        movementInputValue = 0f;
        turnInputValue = 0f;
    }


    private void OnDisable ()
    {
        rigidbody.isKinematic = true;
    }


    private void Start()
    {
        movementAxisName = "Vertical" + playerIdentifier;
        turnAxisName = "Horizontal" + playerIdentifier;

        originalPitch = movementAudioSource.pitch;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(movementInputValue) < EPSILON && Mathf.Abs(turnInputValue) < EPSILON)
        {
            if (movementAudioSource.clip == engineDrivingAudio)
            {
                movementAudioSource.clip = engineIdlingAudio;
                movementAudioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudioSource.Play();
            }
        }
        else
        {
            if (movementAudioSource.clip == engineIdlingAudio)
            {
                movementAudioSource.clip = engineDrivingAudio;
                movementAudioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudioSource.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = turnInputValue * turnSpeed * Time.deltaTime; // Amount of degrees
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
}