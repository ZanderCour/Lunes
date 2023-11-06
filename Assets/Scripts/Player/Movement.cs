using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpCode;
    public KeyCode SprintCode;
    public KeyCode vaultCode;

    float speed;
    public float walkSpeed;
    public float runSpeed;
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

    [Header("Animations")]
    public Animator animator;
    [Space(5)]
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float animatorFloat;

    [Header("Vaulting")]
    public Transform vaultcheck;
    public LayerMask groundLayer;
    [SerializeField] private bool canVault;
    private bool isVaulting;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        GetInput();
        MovePlayer();
        HandleLogic();
        HandleJumping();
        HandleAnimations();
        HandleVaulting();

    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized;

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
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
                if (isGrounded)
                {
                    Jump();
                    isGrounded = false;
                }
            }
        }

        if (!isVaulting) {
            moveVelocity.y += gravity * Time.deltaTime;
            characterController.Move(moveVelocity * Time.deltaTime);
            transform.Rotate(turnVelocity * Time.deltaTime);
        }
    }

    private void HandleVaulting()
    {
        canVault = Physics.CheckSphere(vaultcheck.position, 0.5f, vaultLayer);

        if (Input.GetKeyDown(vaultCode) && canVault) {
            isVaulting = true;
        }
        else if(Input.GetKeyDown(vaultCode) && isVaulting) {
            isVaulting = false;
        }
        else if (Input.GetKeyDown(jumpCode) && isVaulting) {
            
        }




    }

    private void HandleLogic()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        float OriginalSpeed = walkSpeed;
        if (isSprinting)
        {
            speed = runSpeed;
        }
        else if (isMoving) { speed = OriginalSpeed; }
        else if (isIdle) { speed = OriginalSpeed; }
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