using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpCode;
    public KeyCode SprintCode;
    public KeyCode vaultCode;
    public KeyCode crouchKey;


    public KeyCode Slot1;
    public KeyCode Slot2;
    public KeyCode Slot3;
    public KeyCode ToggleHandsKey;

    [Header("Movement")]
    [SerializeField] private float LocomotionAcceleration;
    [SerializeField] private float LocomotionDecceleration;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float targetSpeed;

    public float jumpVelocity;

    private float rotationVelocity;
    public float turnSmoothTime;

    [Header("Info")]
    public bool CanControll;
    public enum ActiveItem
    {
        hands,
        rifle,
        pisotl,
        sword
    };

    public ActiveItem activeItem = ActiveItem.hands;

    [Header("Components")]
    private CharacterController characterController;
    public Rig RifleAimRig;
    public Rig RifleHolsterRig;
    public List<GameObject> ItemLoadout = new List<GameObject>();
    public Transform ItemContainer;
    public int slotIndex;
    public int itemsCount = 2;

    [Header("Vectors")]
    Vector3 moveVelocity;
    Vector3 targetDirection;


    [Header("Camera")]
    public Transform mainCamera;
    public CinemachineVirtualCamera aimCamera;
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    public bool LockCameraPosition;

    [Header("Physics")]
    public bool isGrounded;
    public float gravity;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Animations")]
    public Animator animator;
    [Space(5)]
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float LocomotionanimatorFloat;
    [SerializeField] private float CrouchLocomotionanimatorFloat;
    [SerializeField] private float AimLayerWeight;

    [Header("Settings")]
    public float Sensitivity;
    public float AimingSensitivity;
    public bool invertX;
    public bool invertY;


    public enum MovementState
    {
        isIdle,
        isWalking,
        isSprinting
    };

    public enum ActionState
    {
        none,
        isCrouching,
        isAiming
    };

    private MovementState movementState = MovementState.isIdle;
    private ActionState actionState = ActionState.none;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        HandleLogic();
        GetInput();

        if (CanControll) {

            HandleCrouching();
            HandleMovement();

            HandleJumping();
            HandleAiming();
            HandleCameraRotation();
            HandleLoadout();
        }

        HandleAnimatorValues();
        HandleRigWeight();
    }


    //Groups -->



    #region Movement
    //Handles the player input
    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            movementState = MovementState.isWalking;
        }
        else
        {
            movementState = MovementState.isIdle;
        }

        if (actionState != ActionState.isCrouching)
        {
            if (movementState == MovementState.isWalking)
            {
                if (Input.GetKey(SprintCode))
                {
                    movementState = MovementState.isSprinting;
                }
                else
                {
                    movementState = MovementState.isWalking;
                }
            }
        }
    }
    //------------------------------------


    //Handles the normal player movement 
    private void HandleMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        targetSpeed = movementState == MovementState.isSprinting ? runSpeed : walkSpeed;

        Vector3 inputDirection = new Vector3(Horizontal, 0, Vertical).normalized;

        float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, turnSmoothTime);

        //Checks for the rotation of the player
        if (actionState != ActionState.isAiming && movementState != MovementState.isIdle && isGrounded)
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);


        //Checks for the direction of the player
        if (isGrounded)
            targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // move the player
        characterController.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime) //Movement
            + new Vector3(0.0f, moveVelocity.y, 0.0f) * Time.deltaTime); //Gravity

    }
    //------------------------------------


    //Handles the crouched player movement 
    private void HandleCrouching()
    {
        //Checks input
        if (Input.GetKeyDown(crouchKey) && actionState != ActionState.isCrouching)
        {
            //Sets state to Crouched state
            actionState = ActionState.isCrouching;
            currentSpeed = crouchSpeed;
        }
        else if ((Input.GetKeyDown(crouchKey) || Input.GetKeyDown(jumpCode)) && actionState == ActionState.isCrouching)//Checks for Input 
        {
            //Sets player out of the crouched state
            StartCoroutine(Stand());
            currentSpeed = walkSpeed;
        }

        //Function for making the player stand 
        IEnumerator Stand()
        {
            yield return new WaitForSeconds(0.15f);
            if (actionState == ActionState.isCrouching)
            {
                actionState = ActionState.none;
            }
        }

    }
    //------------------------------------


    //Handles the juming of the player  
    private void HandleJumping()
    {
        if (Input.GetKeyDown(jumpCode))
        {
            if (isGrounded && actionState != ActionState.isCrouching)
            {
                Jump();
            }
        }
    }
    //------------------------------------

    //Applies the jump
    public void Jump()
    {
        moveVelocity.y = jumpVelocity;
    }
    //------------------------------------
    #endregion

    #region Camera
    //Handles the player camera input, Handles the camera settings
    private void HandleCameraRotation()
    {
        Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        float SensitivityType = actionState == ActionState.isAiming ? AimingSensitivity : Sensitivity;
        float OutputSensitivity = SensitivityType * 5;


        float invertXFactor = invertX ? -1f : 1f;
        _cinemachineTargetYaw += look.x * Time.deltaTime * OutputSensitivity * invertXFactor;

        float invertYFactor = invertY ? -1f : 1f;
        _cinemachineTargetPitch += look.y * Time.deltaTime * OutputSensitivity * invertYFactor;



        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    //------------------------------------




    #endregion

    #region Weapons
    //Handles the player aiming and the rotation for the player for each weapon
    private void HandleAiming()
    {
        if (activeItem == ActiveItem.rifle || activeItem == ActiveItem.pisotl)
        {
            aimCamera.gameObject.SetActive(actionState == ActionState.isAiming);
            GunController gunController = GetComponent<GunController>();

            if (Input.GetKey(KeyCode.Mouse1) && isGrounded)
            {
                actionState = ActionState.isAiming;
            }
            else if (!Input.GetKey(KeyCode.Mouse1) || !isGrounded)
            {
                actionState = ActionState.none;
            }

            //AimDirection
            if (actionState == ActionState.isAiming || gunController.isShooting)
            {
                RotateToAim();
            }
        }
        else
        {
            aimCamera.gameObject.SetActive(false);
            actionState = ActionState.none;
        }
            

    }
    //------------------------------------


    //Applies the rotation 
    public void RotateToAim()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    public void HandleLoadout()
    {
        if (Input.GetKeyDown(Slot1)) {
            slotIndex = 1;
            UpdateHeldItem();
        }
        if (Input.GetKeyDown(Slot2)) {
            slotIndex = 2;
            UpdateHeldItem();
        }
        if (Input.GetKeyDown(Slot3)) {
            slotIndex = 3;
            UpdateHeldItem();
        }
        if (Input.GetKeyDown(ToggleHandsKey)) {
            slotIndex = 0;
            UpdateHeldItem();
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            UpdateHeldItem();

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            slotIndex += 1;

            if (slotIndex > itemsCount)
                slotIndex = 0;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            slotIndex -= 1;

            if (slotIndex < 0)
                slotIndex = itemsCount;
        }
    }

    void UpdateHeldItem()
    {
        GunController gunController = GetComponent<GunController>();
        for (int i = 0; i < ItemLoadout.Count; i++) {
            ItemLoadout[i].SetActive(false);
        }

        if (ItemLoadout.Count > slotIndex)
        {
            ItemLoadout[slotIndex].SetActive(true);
            gunController.UpdateHeldItem(slotIndex);
        }
        else
        {
            ItemLoadout[0].SetActive(true);
            gunController.UpdateHeldItem(0);
        }



        if (gunController.HeldItem.weaponType == GunClass.WeaponType.Pistol)
        {
            activeItem = ActiveItem.pisotl;
        }
        else if (gunController.HeldItem.weaponType == GunClass.WeaponType.Rifle)
        {
            activeItem = ActiveItem.rifle;
        }
        else if (gunController.HeldItem.ItemName == "Hands")
        {
            activeItem = ActiveItem.hands;
        }
    }
    //------------------------------------

    #endregion

    #region physics
    //Applies logic and laws of physics to the player
    private void HandleLogic()
    {
        //GrouchCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        //Gravirt force when grounded
        if (isGrounded && !Input.GetKey(jumpCode)) {
            moveVelocity.y = 0;
        }

        //Gravity
        if (!isGrounded) {
            moveVelocity.y += gravity * Time.deltaTime;
        }



        //Player speed -->
        //------------------------------------
        if (actionState != ActionState.isCrouching && isGrounded)
        {
            if(movementState != MovementState.isIdle)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, LocomotionAcceleration * Time.deltaTime);
            }
            else
            if (movementState == MovementState.isIdle)
            {
                targetSpeed = 0;

                if(currentSpeed < 1)
                {
                    currentSpeed = 0;
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, LocomotionDecceleration * Time.deltaTime);
                }
            }
        }
        //------------------------------------
    }
    //------------------------------------

    #endregion

    #region Animation

    //Animator Values
    private void HandleAnimatorValues()
    {
        //Animtion type 
        if (actionState != ActionState.isCrouching)
        {
            //Handle normal movement
            HandleMovementAnimation();
        }
        else
        {
            //Handle crouched movement
            HandleCrouchAnimation();
        }


        //Animator Values
        //-----------------------------------------------------------------------------------
        animator.SetFloat("Movement", LocomotionanimatorFloat);
        animator.SetFloat("CrouchMovement", CrouchLocomotionanimatorFloat);
        animator.SetBool("IsCrouching", actionState == ActionState.isCrouching);
        animator.SetBool("Air", !isGrounded);
        animator.SetBool("Moving", movementState != MovementState.isIdle);
        animator.SetBool("Aim", actionState == ActionState.isAiming);
        //-----------------------------------------------------------------------------------


        //Animator Layer Weights
        //-----------------------------------------------------------------------------------
        if (activeItem == ActiveItem.hands)
        {
            LerpAnimatorLayer(1, 0, 3);
        }
        else
        {
            LerpAnimatorLayer(1, 1, 3);

        }
        //-----------------------------------------------------------------------------------
    }

    private void LerpAnimatorLayer(int layerIndex, float target, float speedMultiplier)
    {
        float currentWeight = animator.GetLayerWeight(layerIndex);
        float weight = Mathf.Lerp(currentWeight, target, (LocomotionDecceleration / 2) * speedMultiplier * Time.deltaTime);
        animator.SetLayerWeight(layerIndex, weight);
    }
    //------------------------------------



    //WeaponRig
    private void HandleRigWeight()
    {
        //Rifle Rig
        if (activeItem == ActiveItem.rifle)
        {
            //Weapon rig
            GunController gunController = GetComponent<GunController>();
            if (actionState == ActionState.isAiming || gunController.isShooting)
            {
                LerpRig(1, 5, RifleAimRig);
                LerpRig(0, 5, RifleHolsterRig);
            }

            //If idle 
            if (actionState != ActionState.isAiming && !gunController.isShooting)
            {
                LerpRig(0, 5, RifleAimRig);
                LerpRig(1, 5, RifleHolsterRig);
            }
        }
        else if (activeItem == ActiveItem.hands)
        {
            LerpRig(0, 5, RifleAimRig);
            LerpRig(0, 5, RifleHolsterRig);
        }
    }
    //------------------------------------


    //AimRig
    public void LerpRig(float target, float speedMultiplier, Rig rig) 
    {
        rig.weight = Mathf.Lerp(rig.weight, target, (LocomotionDecceleration / 2) * speedMultiplier * Time.deltaTime);
    }
    //------------------------------------


    //Movement Animations 
    public void HandleMovementAnimation()
    {
        if (isGrounded)
        {
            if (movementState == MovementState.isWalking && LocomotionanimatorFloat < 0.5f) {
                LocomotionanimatorFloat += Time.deltaTime * acceleration;
            }

            if (movementState == MovementState.isSprinting && LocomotionanimatorFloat < 1.0f) {
                LocomotionanimatorFloat += Time.deltaTime * acceleration;
            }

            if (movementState == MovementState.isWalking && LocomotionanimatorFloat > 0.5f) {
                LocomotionanimatorFloat -= Time.deltaTime * deceleration;
            }

            if (movementState == MovementState.isIdle && LocomotionanimatorFloat > 0.0f) {
                LocomotionanimatorFloat -= Time.deltaTime * deceleration;
            }

            if (movementState == MovementState.isWalking && LocomotionanimatorFloat < 0.0f) {
                LocomotionanimatorFloat = 0f;
            }
        }
    }
    //------------------------------------



    //Crouched animations
    public void HandleCrouchAnimation() 
    {
        if (movementState == MovementState.isWalking && CrouchLocomotionanimatorFloat < 1)
        {
            CrouchLocomotionanimatorFloat += Time.deltaTime * acceleration;
        }
        else if(movementState == MovementState.isIdle && CrouchLocomotionanimatorFloat > 0)
        {
            CrouchLocomotionanimatorFloat -= Time.deltaTime * deceleration;
        }
    }
    //------------------------------------



    //Clamp Math
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion



    //------------------------------------

}