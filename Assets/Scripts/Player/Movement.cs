using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpCode;
    public KeyCode SprintCode;

    float speed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity;
    public float turnSmoothTime;


    [Header("Info")]
    public bool isMoving;
    public bool isSprinting;
    public bool isIdle;
    [Space(15)]
    public bool canJump;

    [Header("Hidden values")]
    float turnSmoothVelocity;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    Rigidbody rb;

    [Header("Vectors")]
    Vector3 moveVelocity;
    Vector3 turnVelocity;
    Vector3 moveDirection;

    [Header("Camera")]
    public Transform cam;

    [Header("Physics")]
    public bool isGrounded;
    public float gravity;
    public float jumpCooldown;

    [Header("Animations")]
    public Animator animator;
    [Space(5)]
    public float acceleration = 0.1f;
    public float SprintAcceleration = 1f;
    public float deceleration = 0.5f;
    private float animatorFloat;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        GetInput();
        HandleLogic();
        MovePlayer();
        HandleJumping();

        HandleAnimations();
    }

    private void MovePlayer()
    {
        //Gets the RAW input axises 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) //Checks if there is a input from the player if not then dont move
        {
            //Handling the rotation of the player acording to the camera rotation via the Cinamachine
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized;

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        isMoving = true ? (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) : isMoving = false;
        isSprinting = true ? Input.GetKey(SprintCode) : isSprinting = false;
        isIdle = true ? !isSprinting && !isMoving : isIdle = false;
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
                if (canJump)
                {
                    Jump();
                    canJump = false;
                }
            }
        }

        moveVelocity.y += gravity * Time.deltaTime;
        characterController.Move(moveVelocity * Time.deltaTime);
        transform.Rotate(turnVelocity * Time.deltaTime);
    }

    private void HandleLogic()
    {
        float OriginalSpeed = walkSpeed;

        speed = runSpeed ?  isSprinting  speed = OriginalSpeed

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

        animator.SetFloat("Velocity", animatorFloat);
    }
}

