using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpCode;
    public KeyCode SprintCode;
    public KeyCode vaultCode;
    public KeyCode crouchKey;

    [Header("Movement")]
    [SerializeField] private float LocomotionAcceleration;
    [SerializeField] private float LocomotionDecceleration;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;

    public float jumpVelocity;
    public float turnSmoothTime;

    [Header("Info")]
    public bool isWalking;
    public bool isSprinting;
    public bool isIdle;

    [Header("Hidden values")]
    float turnSmoothVelocity;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    private Rigidbody rb;
    public GameObject camWind;

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
    [SerializeField] private float LocomotionanimatorFloat;
    [SerializeField] private float CrouchLocomotionanimatorFloat;
    [Header("States")]
    public bool CrouchState;
    public bool StandingState;

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
<<<<<<< HEAD:Assets/Scripts/Player/PlayerController.cs
=======

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
>>>>>>> parent of 1783e75 (Character og oprydet i filer):Assets/Scripts/Player/Movement.cs
    }

    public void Update()
    {
        GetInput();
        if (!isVaulting)
            MovePlayer();

        HandleLogic();
        if (!isVaulting && !CrouchState)
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
            if (isWalking)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);


                if (isGrounded) {
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized;
                }
            }

            if (isGrounded && !CrouchState)
            {
                if (isWalking && !isSprinting)
                {
                    if (currentSpeed < walkSpeed)
                    {
                        currentSpeed += LocomotionAcceleration;
                    }
                    else { currentSpeed = walkSpeed; }
                }
                else if (isWalking && isSprinting)
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
            else if (isGrounded && CrouchState) 
            {
                if (isWalking && currentSpeed < crouchSpeed)
                {
                    currentSpeed += LocomotionAcceleration;
                }
                else if(isIdle && currentSpeed > 0)
                {
                    currentSpeed -= LocomotionDecceleration;
                }

                else if (currentSpeed < 0) { currentSpeed = 0; }

            }


            characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        isWalking = true ? (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) : isWalking = false;

        if (!CrouchState)
            isSprinting = true ? Input.GetKey(SprintCode) && isWalking : isSprinting = false;

        isIdle = true ? !isSprinting && !isWalking : isIdle = false;

        if (!CrouchState)
        {
            if (Input.GetKeyDown(crouchKey))
            {
                CrouchState = true;

                if (currentSpeed > crouchSpeed)
                {
                    currentSpeed = 1f;
                }
            }
        }
        else
        if (CrouchState)
        {
            if (Input.GetKeyDown(jumpCode) || Input.GetKeyDown(crouchKey))
            {
                StartCoroutine(Stand());
                currentSpeed = walkSpeed;
            }
        }

        if (currentSpeed >= runSpeed)
            camWind.SetActive(true);
        else
            camWind.SetActive(false);
    }

    IEnumerator Stand()
    {
        yield return new WaitForSeconds(0.15f);
        if (CrouchState) {
            CrouchState = false;
        }
    }

    public void Jump()
    { 
        moveVelocity.y = jumpVelocity;
    }


    private void HandleJumping()
    {
        if (Input.GetKey(jumpCode))
        {
            if (isGrounded)
            {
                Jump();
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
            Debug.Log("Vaulting");
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
        if (!isVaulting)
        {
            if (!CrouchState)
            {
                HandleMovementAnimation();
            }
            else
            {
                HandleCrouchAnimation();
            }
        }

        animator.SetFloat("Movement", LocomotionanimatorFloat);
        animator.SetFloat("CrouchMovement", CrouchLocomotionanimatorFloat);
        animator.SetBool("IsCrouching", CrouchState);
        animator.SetBool("WallClimb", isVaulting);
        animator.SetBool("Air", !isGrounded);
        animator.SetBool("Moving", !isIdle);

        GameObject playermodel = GameObject.FindWithTag("Humanoid");
        if (CrouchState)
        {
            playermodel.transform.localRotation = Quaternion.Euler(0, 25, 0);
        }
        else
        {
            playermodel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }


    public void HandleMovementAnimation()
    {
        if (isGrounded)
        {
            if (isWalking && LocomotionanimatorFloat < 0.5f)
            {
                LocomotionanimatorFloat += Time.deltaTime * acceleration;
            }

            if (isSprinting && LocomotionanimatorFloat < 1.0f)
            {
                LocomotionanimatorFloat += Time.deltaTime * acceleration;
            }

            if (!isSprinting && isWalking && LocomotionanimatorFloat > 0.5f)
            {
                LocomotionanimatorFloat -= Time.deltaTime * deceleration;
            }

            if (isIdle && LocomotionanimatorFloat > 0.0f)
            {
                LocomotionanimatorFloat -= Time.deltaTime * deceleration;
            }

            if (isWalking && LocomotionanimatorFloat < 0.0f)
            {
                LocomotionanimatorFloat = 0f;
            }
        }
    }

    public void HandleCrouchAnimation() 
    {
        if (isWalking && CrouchLocomotionanimatorFloat < 1)
        {
            CrouchLocomotionanimatorFloat += Time.deltaTime * acceleration;
        }
        else if(isIdle && CrouchLocomotionanimatorFloat > 0)
        {
            CrouchLocomotionanimatorFloat -= Time.deltaTime * deceleration;
        }
    }
}