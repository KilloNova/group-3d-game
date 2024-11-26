using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float mouseSensitivity = 100.0f;

    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;

    private Transform playerCamera; // Reference to the camera

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen

        playerCamera = Camera.main.transform; // Use Camera.main to get the main camera
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to ensure the player stays grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate the camera vertically
        transform.Rotate(Vector3.up * mouseX); // Rotate the player horizontally
    }
}
