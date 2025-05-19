using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    // For picking the camera.
    public Camera cam;
    // Zero the camera.
    private float xRotation = 0f;
    // These two are for defining the sensitivity.
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    // This is for actually looking. Vector 2 again cause you're moving the mouse on (x,y) map.
    public void ProcessLook(Vector2 input)
    {
        // Get the x and y inputs for later use.
        float mouseX = input.x;
        float mouseY = input.y;
        // Gets y input, timed by time and then changed based on the sensitivity. I don't really understand why, but y inputs are weird so you need to do this seperately unlike with x inputs.
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        // Restricts the values to between -80 and 80.
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // Quaternion.Euler is used for rotation of the angle as you need to translate x y values to actual angles of the camera.
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // Rotates the camera for x inputs.
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
    // When the game starts, it locks and hides the mouse.
    void Start()
    {
        // Locks mouse in the middle of screen.
        Cursor.lockState = CursorLockMode.Locked;
        // Hides mouse.
        Cursor.visible = false;
    }
}