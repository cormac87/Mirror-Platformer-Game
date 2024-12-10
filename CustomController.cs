using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomController : MonoBehaviour
{
    public Transform camera; // Reference to the camera
    private Rigidbody rb; // Reference to the Rigidbody component
    public float movementForce = 50f; // Movement force for forward movement
    public float mouseSensitivity = 100f; // Mouse sensitivity for rotation
    public float gravity = 9.81f; // Gravity force (positive value)
    public float jumpForce = 5f; // Jump force
    public Transform groundCheck; // Reference to the ground check object
    public float groundDistance = 0.2f; // Distance to check for ground
    private float verticalVelocity = 0f; // Vertical velocity (affected by gravity)
    private bool isGrounded; // Track if the character is grounded

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so that AddForce doesn't rotate the Rigidbody
    }

    void Update()
    {
        // Mouse rotation for the character (Y-axis rotation)
        float mouseX = Input.GetAxis("Mouse X");
        float rotationAmount = mouseX * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount, Space.Self);

        // Mouse rotation for the camera (X-axis rotation)
        float mouseY = Input.GetAxis("Mouse Y");
        float vertRotationAmount = mouseY * mouseSensitivity * Time.deltaTime;
        camera.transform.Rotate(-Vector3.right, vertRotationAmount, Space.Self);
    }

    void FixedUpdate()
    {
        // Check if the character is grounded using a Raycast
        isGrounded = Physics.Raycast(groundCheck.position, -transform.up, groundDistance);

        // Reset vertical velocity when grounded
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        // Check for jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange); // Apply jump force
        }

        // Apply gravity to the vertical velocity
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            verticalVelocity = 0f;
        }

        // Handle movement input
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward * movementForce;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += -transform.forward * movementForce;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += -transform.right * movementForce;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += transform.right * movementForce;
        }

        // Apply force for movement
        rb.AddForce(movement * Time.fixedDeltaTime, ForceMode.Acceleration);

        // Apply gravity force to the Rigidbody
        if (!isGrounded)
        {
            rb.AddForce(-transform.up * gravity, ForceMode.Acceleration);
        }

    }
}
