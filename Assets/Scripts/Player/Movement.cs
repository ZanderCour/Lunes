using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Movement : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey;
    
    CharacterController controller;
    [SerializeField] private Transform playerCamera;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float turnSmoothTime;
    float turnSmoothVelocity;

    [Header("Jumping")]
    [SerializeField] private float jumpMultiplier;

    [Header("Axis")]
    float Horizontal;
    float Vertical;

    [Header("Physics")]
    public Transform groundCheck;
    [SerializeField] private bool isGrounded;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 playerVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleJumping();
        HandlePhysics();
    }

    private void HandleJumping()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                playerVelocity.y = jumpMultiplier;
            }
        }
    }

    private void HandleMovement()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0);
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
    }

    private void HandlePhysics()
    {
        //Gravity
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }


}
