using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : Health
{
    public bool CanMove { get; private set; } = true;

    //Inspector Variables
    [Header("Functional Options")]
    [SerializeField]
    bool canSprint = true;
    [SerializeField]
    bool canJump = true;
    [SerializeField]
    bool canCrouch = true ;
    [SerializeField]
    bool hasHeadBob = true;
    [SerializeField]
    bool slideDownSteepSlopes = true;

    [Header("Movement Parameters")]
    [SerializeField]
    float walkSpeed = 3f;
    [SerializeField]
    float sprintSpeed = 6f;
    [SerializeField]
    float crouchWalkSpeed = 1.5f;
    [SerializeField]
    float groundAccelerationRate;
    [SerializeField]
    float groundDecelerationRate;
    [SerializeField]
    float airAccelerationRate;
    [SerializeField]
    float airDecelerationRate;

    [Header("Look Parameters")]
    [SerializeField, Range(1f, 10f)]
    float xLookSpeed = 2f;
    [SerializeField, Range(1f, 10f)]
    float yLookSpeed = 2f;
    [SerializeField, Range(1f, 180f)]
    float xLookLimitUpper = 80f;
    [SerializeField, Range(1f, 180f)]
    float xLookLimitLower = 80f;

    [Header("Jump Parameters")]
    [SerializeField]
    float jumpForce;

    [Header("Crouch Parameters")]
    [SerializeField]
    float crouchHeight = 0.5f;
    [SerializeField]
    float standingHeight = 2f;
    [SerializeField]
    float crouchTime = 0.25f;
    [SerializeField]
    Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField]
    Vector3 standingCenter = new Vector3(0, 0, 0);
    [SerializeField]
    float uncrouchCollisionCheckDistance = 1f;
    [SerializeField]
    LayerMask canStopUncrouch;

    [Header("Head Bob Parameters")]
    [SerializeField]
    float walkBobSpeed = 14f;
    [SerializeField]
    float walkBobAmount = 0.05f;
    [SerializeField]
    float sprintBobSpeed = 18f;
    [SerializeField]
    float sprintBobAmount = .1f;
    [SerializeField]
    float crouchWalkBobSpeed = 8f;
    [SerializeField]
    float crouchWalkBobAmount = 0.025f;

    [Header("Slope Sliding Parameters")]
    [SerializeField]
    float maxSlopeSlipSpeed = 8f;
    [SerializeField]
    float slopeSlipAcceleration;
    [SerializeField]
    float slopeRaycastDistance = 2f;

    [Header("Health")]
    [SerializeField]
    int startingHealth;
    [SerializeField]
    int startingMaxHealth;
    [SerializeField]
    bool startingInvulnerableState;
    [SerializeField]
    float startingInvulnerabilityTimeAfterHit;

    //Private Variables
    private float deafaultCameraYPosition;
    private bool isInCrouchAnimation;
    private bool isCrouching = false;
    private float gravity;
    private CharacterController characterController;
    private Camera playerCamera;
    private Vector2 desiredPlayerMovement;
    private Vector3 movementDirection;
    private float xCameraRotation;
    private float headBobTimer;
    private Vector3 hitPointNormal;

    private bool shouldSlide
    {
        get
        {
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, slopeRaycastDistance))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }
    private bool shouldSprint => Input.GetButton("Sprint") && canSprint && characterController.isGrounded;
    private bool shouldJump => Input.GetButtonDown("Jump") && characterController.isGrounded;
    private bool shouldCrouch => Input.GetButtonDown("Crouch") && !isInCrouchAnimation && characterController.isGrounded;

    void Awake()
    {
        OnValidate();
        gravity = Physics.gravity.magnitude;
        if(GetComponentInChildren<Camera>() != null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
        else
        {
            Debug.LogError("No camera found");
        }
        if(GetComponent<CharacterController>() != null)
        {
            characterController = GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogError("No character controller found");
        }
        deafaultCameraYPosition = playerCamera.transform.localPosition.y;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;

    }
    // Update is called once per frame
    void Update()
    {
        if(CanMove)
        {
            HandleMovementInput();
            HandleLookInput();
            if(canJump)
            {
                HandleJump();
            }
            if(canCrouch)
            {
                HandleCrouch();
            }
            if(hasHeadBob)
            {
                HandleHeadBob();
            }
            ApplyCharacterMovement();
        }
    }
    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            SetHealthVars(startingHealth, startingMaxHealth, startingInvulnerableState, startingInvulnerabilityTimeAfterHit);
        }
    }
    void HandleMovementInput()
    {
        float desiredMoveSpeed;
        if(isCrouching)
        {
            desiredMoveSpeed = crouchWalkSpeed;
        }
        else if (shouldSprint)
        {
            desiredMoveSpeed = sprintSpeed;
        }
        else
        {
            desiredMoveSpeed = walkSpeed;
        }

        desiredPlayerMovement = new Vector2(Input.GetAxis("Vertical") * desiredMoveSpeed, Input.GetAxis("Horizontal")).normalized * desiredMoveSpeed;
        float currentVelocityX = Vector3.Dot(characterController.velocity, transform.forward);
        float currentVelocityY = Vector3.Dot(characterController.velocity, transform.right);
        float newX;
        float newY;
        float accelerationRate = characterController.isGrounded ? groundAccelerationRate * Time.deltaTime  : airAccelerationRate * Time.deltaTime;
        float decelerationRate = characterController.isGrounded ? groundDecelerationRate * Time.deltaTime : airDecelerationRate * Time.deltaTime;

        if(Mathf.Abs(currentVelocityX) < Mathf.Abs(desiredPlayerMovement.x))
        {
            newX = Mathf.MoveTowards(currentVelocityX, desiredPlayerMovement.x, accelerationRate);
        }
        else
        {
            newX = Mathf.MoveTowards(currentVelocityX, desiredPlayerMovement.x, decelerationRate);
        }
        if (Mathf.Abs(currentVelocityY) < Mathf.Abs(desiredPlayerMovement.y))
        {
            newY = Mathf.MoveTowards(currentVelocityY, desiredPlayerMovement.y, accelerationRate);
        }
        else
        {
            newY = Mathf.MoveTowards(currentVelocityY, desiredPlayerMovement.y, decelerationRate);
        }

        float movementDirectionY = movementDirection.y;

        movementDirection = (newX * transform.forward) + (newY * transform.right);
        movementDirection.y = movementDirectionY;
    }
    void HandleJump()
    {
        if (shouldJump)
        {
            movementDirection.y = jumpForce;
        }
    }
    void HandleCrouch()
    {
        if(shouldCrouch)
        {
            StartCoroutine(ToggleCrouch());
        }
    }
    void HandleLookInput()
    {
        xCameraRotation -= Input.GetAxis("Mouse Y") * yLookSpeed;
        xCameraRotation = Mathf.Clamp(xCameraRotation, -xLookLimitUpper , xLookLimitLower);
        playerCamera.transform.localRotation = Quaternion.Euler(xCameraRotation, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * xLookSpeed, 0);
    }
    void ApplyCharacterMovement()
    {
        if(!characterController.isGrounded)
        {
            movementDirection.y -= gravity* Time.deltaTime;
        }
        if(slideDownSteepSlopes && shouldSlide)
        {
            Vector3 newMovementVectorForSlide = Vector3.MoveTowards(characterController.velocity, new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * maxSlopeSlipSpeed, Time.deltaTime * slopeSlipAcceleration);
            movementDirection = newMovementVectorForSlide;
        }
        characterController.Move(movementDirection * Time.deltaTime);
    }
    void HandleHeadBob()
    {
        if (!characterController.isGrounded)
        {
            return;
        }
        float headBobSpeed;
        float headBobAmount;
        Vector3 nonVerticalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        if (nonVerticalVelocity.magnitude > 0 && nonVerticalVelocity.magnitude <= crouchWalkSpeed && isCrouching)
        {
            headBobSpeed = (nonVerticalVelocity.magnitude / crouchWalkSpeed) * crouchWalkBobSpeed;
            headBobAmount = (nonVerticalVelocity.magnitude / crouchWalkSpeed) * crouchWalkBobAmount;
        }
        else if(nonVerticalVelocity.magnitude > crouchWalkSpeed && isCrouching)
        {
            headBobSpeed = crouchWalkSpeed;
            headBobAmount= crouchWalkBobAmount;
        }
        else if (nonVerticalVelocity.magnitude > 0 && nonVerticalVelocity.magnitude <= walkSpeed)
        {
            headBobSpeed = (((nonVerticalVelocity.magnitude - crouchWalkSpeed) / (walkSpeed - crouchWalkSpeed)) * (walkBobSpeed - crouchWalkBobSpeed)) + crouchWalkBobSpeed;
            headBobAmount = (((nonVerticalVelocity.magnitude - crouchWalkSpeed) / (walkSpeed - crouchWalkSpeed)) * (walkBobAmount - crouchWalkBobAmount)) + crouchWalkBobAmount;
        }
        else if(nonVerticalVelocity.magnitude > walkSpeed && !shouldSprint)
        {
            headBobSpeed = walkBobSpeed;
            headBobAmount = walkBobAmount;
        }
        else if (nonVerticalVelocity.magnitude > walkSpeed && nonVerticalVelocity.magnitude <= sprintSpeed && shouldSprint)
        {
            headBobSpeed = (((nonVerticalVelocity.magnitude - walkSpeed) / (sprintSpeed - walkSpeed)) * (sprintBobSpeed - walkBobSpeed)) + walkBobSpeed;
            headBobAmount = (((nonVerticalVelocity.magnitude - walkSpeed) / (sprintSpeed - walkSpeed)) * (sprintBobAmount - walkBobAmount)) + walkBobAmount;
        } 
        else if (nonVerticalVelocity.magnitude > sprintSpeed)
        {
            headBobSpeed = sprintBobSpeed;
            headBobAmount = sprintBobAmount;
        }
        else
        {
            return;
        }
        headBobTimer += Time.deltaTime * headBobSpeed;
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, deafaultCameraYPosition + Mathf.Sin(headBobTimer) * headBobAmount, playerCamera.transform.localPosition.z);

    }
    IEnumerator ToggleCrouch()
    {
        isInCrouchAnimation = true;
        float timer = 0f;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenterPoint = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenterPoint = characterController.center;

        while (timer < crouchTime)
        {
            if (!isCrouching || isCrouching && !Physics.CheckBox(playerCamera.transform.position + Vector3.up * (uncrouchCollisionCheckDistance / 2), new Vector3(characterController.radius, uncrouchCollisionCheckDistance, characterController.radius), transform.rotation, canStopUncrouch))
            {
                characterController.height = Mathf.Lerp(currentHeight, targetHeight, timer / crouchTime);
                characterController.center = Vector3.Lerp(currentCenterPoint, targetCenterPoint, timer / crouchTime);
                timer += Time.deltaTime;
            }
            yield return null;
        }
        isCrouching = !isCrouching;
        characterController.height = targetHeight;
        characterController.center = targetCenterPoint;
        isInCrouchAnimation= false;
    }
}
