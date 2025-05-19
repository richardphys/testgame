using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // Gets Unity's built in controller component.
    private CharacterController controller;
    // Since it's 3D we get Vector3 and we can track the player's velocity with it.
    private Vector3 playerVelocity;
    // Bool outputs basically 1 or 0, yes or no. This checks if the character is grounded or not.
    private bool IsGrounded;
    // For player speed.
    [Header("Player Velocity")]
    public float speed = 5f;
    // For player sprint speed.
    public float sprintSpeed = 10f;
    // Define currentSpeed for later use.
    private float currentSpeed;
    // Gravity.
    public float gravity = -9.8f;
    // Defines the jump height.
    public float jumpHeight = 3f;
    [Header("Footstep Audio")]
    // For audio clips.
    public AudioClip footstepClip;
    // Some kind of audio source.
    private AudioSource footstepAudio;
    // For footstepTimer.
    private float footstepTimer = 0f;
    // For footstepInterval.
    private float footstepInterval = 0.4f;
    // For using inputManager script.
    private InputManager inputManager;
    // We use Start, because it requires the Awake from InputManager to run first. Hierarchy is Awake -> Start, so you need Start to go 2nd essentially, because it relies on InputManager.
    void Start()
    {
        // Grabs controller component to be used later.
        controller = GetComponent<CharacterController>();
        // Grabs insput component.
        inputManager = GetComponent<InputManager>();
        // Grabs footstep audio component.
        footstepAudio = GetComponent<AudioSource>();
    }
    // Player movement WASD happens in 2D space so we call Vector2. This takes inputs from InputManager and applies to the player.
    public void ProcessMove(Vector2 input)
    {
        // Zero the inputs before messing with them.
        Vector3 moveDirection = Vector3.zero;
        // Takes x input from InputManager and makes the character move in x direction.
        moveDirection.x = input.x;
        // Takes y input from InputManager and makes the character move in y direction. In InputManager there is only (x,y) map, but in 3D y is up/down while moving to the side happens on x and y axis, so it has to be translated.
        moveDirection.z = input.y;
        // Checks if player is sprinting and sets it to sprintSpeed, if not the sets it to default walking speed.
        currentSpeed = inputManager.isSprinting ? sprintSpeed : speed;
        // Move makes the player move, moveDirection received from inputs and then transform.TransformDirection actually makes the player move based on those inputs. The movement speed is multiplied with the current speed and then Time.deltaTime is time.
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        // Each unity of time Time.deltaTime adds gravitational effect to the player, -9.8 m/s every s.
        playerVelocity.y += gravity * Time.deltaTime;
        // There needs to be something in place to stop the gravity from simply building, so this checks if the player is on ground and their velocity is negative.
        if (IsGrounded && playerVelocity.y < 0)
        // Which then gives a constant downwards velocity instead of one that keeps gaining.
            playerVelocity.y = -2f;
        // This actually the downwards velocity for the player.
        controller.Move(playerVelocity * Time.deltaTime);
    }
    // This is purely for jumping.
    public void Jump()
    {
        // Make sure that player is touching the ground when jump action is done.
        if (IsGrounded)
        {
            // In physics v=sqrt(2gh), but here instead of 2 -> 3 is used for game "feel".
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    // This is for checking things every frame.
    void Update()
    {
        // Check if player is grounded.
        IsGrounded = controller.isGrounded;
        // Stores movement input.
        Vector2 moveInput = inputManager.onFoot.Movement.ReadValue<Vector2>();
        // Checks that the movement is happening.
        bool isMoving = moveInput.magnitude > 0.1f;
        // Checks that player is grounded and moving.
        if (IsGrounded && isMoving)
        {
            // This bit of code is basically to have the footstep audio play after every few secs. The footstepTimer starts and when the footStep interval is crossed it plays the audio, then it resets and does it again.
            footstepTimer += Time.deltaTime;
            if (footstepTimer > footstepInterval)
            {
                // Play audio.
                footstepAudio.PlayOneShot(footstepClip);
                // Reset it.
                footstepTimer = 0f;
            }
        }
        else
        {
            // Reset it just in case.
            footstepTimer = 0f;
        }
    }
}
