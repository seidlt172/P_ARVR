using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called at fixed intervals (physics)
    private void FixedUpdate()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Read keyboard input using the modern Input System
        if (Keyboard.current != null)
        {
            // Left/right movement
            if (Keyboard.current.leftArrowKey.isPressed)
                moveX = 1f;
            else if (Keyboard.current.rightArrowKey.isPressed)
                moveX = -1f;

            // Forward/backward movement
            if (Keyboard.current.upArrowKey.isPressed)
                moveZ = -1f;
            else if (Keyboard.current.downArrowKey.isPressed)
                moveZ = 1f;
        }

        // Create the movement vector (after the input was read)
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.AddForce(movement * speed, ForceMode.Force);
    }
}
