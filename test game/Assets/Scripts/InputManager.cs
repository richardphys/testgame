using UnityEngine;
// Makes sure you use the input system.
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Reference PlayerInput script.
    private PlayerInput playerInput;
    // This gets specifically the onFoot map.
    public PlayerInput.OnFootActions onFoot;
    // This is for player movement.
    private PlayerMotor motor;
    // This is for player looking.
    private PlayerLook look;
    public bool isSprinting => onFoot.Sprint.IsPressed() && onFoot.Movement.ReadValue<Vector2>().y > 0.5f;
    // Void awake cause we want to run this script before the game starts.
    void Awake()
    {
        // Create a usable version of the action map.
        playerInput = new PlayerInput();
        // Grabs the OnFoot action map from playerInput.
        onFoot = playerInput.OnFoot;
        // Grabs the motor component.
        motor = GetComponent<PlayerMotor>();
        // Grabs the look component.
        look = GetComponent<PlayerLook>();
        // This checks if the jump is performed, and then calls in the Jump function from the PlayerMotor script.
        onFoot.Jump.performed += ctx => motor.Jump();
    }
    // FixedUpdate cause we want it at a fixed interval.
    void FixedUpdate()
    {
        // Reads the value for movement and makes the player move.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    // Camera stuff is always done in LateUpdate.
    void LateUpdate()
    {
        // Reads the values for looking and makes the player look.
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    // This enables the action map.
    private void OnEnable()
    {
        onFoot.Enable();
    }
    // Since there is an enable there also has to be a disable.
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
