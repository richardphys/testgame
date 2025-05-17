using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    public float speed = 5f;
    public float sprintSpeed = 10f;
    private float currentSpeed;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    [Header("Footstep Audio")]
    public AudioClip footstepClip;
    private AudioSource footstepAudio;
    private float footstepTimer = 0f;
    private float footstepInterval = 0.4f;
    private InputManager inputManager;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        footstepAudio = GetComponent<AudioSource>();
    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        currentSpeed = inputManager.isSprinting ? sprintSpeed : speed;
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(IsGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    public void Jump()
    {
        if (IsGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    void Update()
    {
        IsGrounded = controller.isGrounded;
        Vector2 moveInput = inputManager.onFoot.Movement.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > 0.1f;
        if (IsGrounded && isMoving)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer > footstepInterval)
            {
                footstepAudio.PlayOneShot(footstepClip);
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
