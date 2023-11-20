using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpCode;
    public KeyCode SprintCode;
    public KeyCode vaultCode;

    [Header("Movement")]
    [SerializeField] private float LocomotionAcceleration;
    [SerializeField] private float LocomotionDecceleration;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    public float jumpVelocity;
    public float turnSmoothTime;

    [Header("Info")]
    public bool isMoving;
    public bool isSprinting;
    public bool isIdle;

    [Header("Hidden values")]
    float turnSmoothVelocity;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    private Rigidbody rb;

    [Header("Vectors")]
    Vector3 moveVelocity;
    Vector3 turnVelocity;
    Vector3 moveDirection;

    [Header("Camera")]
    public Transform cam;

    [Header("Physics")]
    private bool isGrounded;
    public float gravity;
    public Transform groundCheck;
    public LayerMask vaultLayer;
    public LayerMask wallLayer;

    [Header("Animations")]
    public Animator animator;
    [Space(5)]
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float animatorFloat;

    [Header("Vaulting")]
    public Transform Headcheck;
    public Transform FootCheck;
    public LayerMask groundLayer;
    [SerializeField] private bool canVault;
    private bool isVaulting;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        GetInput();
        if (!isVaulting)
            MovePlayer();

        HandleLogic();
        if (!isVaulting)
            HandleJumping();

        HandleAnimations();
        HandleVaulting();

    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(!isVaulting)
        {
            if (isMoving)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);


                if (isGrounded) {
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized;
                }
            }

            if (isGrounded)
            {
                if (isMoving && !isSprinting)
                {
                    if (currentSpeed < walkSpeed)
                    {
                        currentSpeed += LocomotionAcceleration;
                    }
                    else { currentSpeed = walkSpeed; }
                }
                else if (isMoving && isSprinting)
                {
                    if (currentSpeed < runSpeed)
                    {
                        currentSpeed += LocomotionAcceleration;
                    }
                    else { currentSpeed = runSpeed; }
                }
                else if (isIdle)
                {
                    if (currentSpeed > 0)
                    {
                        currentSpeed -= LocomotionDecceleration;
                    }
                    else if (currentSpeed < 0) { currentSpeed = 0; }
                }
            }
            characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) { isMoving = true; }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) { isMoving = false; isSprinting = false; }

        if (isMoving && Input.GetKey(SprintCode))
        {
            isSprinting = true;
        }
        else if (isMoving && !Input.GetKey(SprintCode))
        {
            isSprinting = false;
        }

        if (!isSprinting && !isMoving)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    public void Jump()
    {
        moveVelocity.y = jumpVelocity;
    }

    private void HandleJumping()
    {
        if (isGrounded)
        {
            if (Input.GetKey(jumpCode))
            {
                Jump();
                isGrounded = false;
            }
        }

        if (!isVaulting) {
            moveVelocity.y += gravity * Time.deltaTime;
            transform.Rotate(turnVelocity * Time.deltaTime);
        }

        characterController.Move(moveVelocity * Time.deltaTime);
    }
    [SerializeField] private bool Null = false;
    private void HandleVaulting()
    {
        canVault = Physics.CheckSphere(Headcheck.position, 0.25f, vaultLayer);
        Null = !Physics.CheckSphere(FootCheck.position, 0.25f, wallLayer);

        if (Input.GetKeyDown(vaultCode) && canVault) {
            isVaulting = true;
        }

        if (Null) {
            isVaulting = false;
        }
        else if(isVaulting) {
            characterController.Move(moveDirection.normalized * walkSpeed * 1.5f * Time.deltaTime);
        }

        if (isVaulting) {
            characterController.Move(transform.up.normalized * walkSpeed * Time.deltaTime);
        }
    }

    private void HandleLogic()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleAnimations()
    {
        if (isMoving && animatorFloat < 0.2f)
        {
            animatorFloat += Time.deltaTime * acceleration;
        }

        if (isSprinting && animatorFloat < 1.0f)
        {
            animatorFloat += Time.deltaTime * acceleration;
        }

        if (!isSprinting && isMoving && animatorFloat > 0.2f)
        {
            animatorFloat -= Time.deltaTime * deceleration;
        }

        if (isIdle && animatorFloat > 0.0f)
        {
            animatorFloat -= Time.deltaTime * deceleration;
        }

        if (isMoving && animatorFloat < 0.0f)
        {
            animatorFloat = 0f;
        }
    }
}