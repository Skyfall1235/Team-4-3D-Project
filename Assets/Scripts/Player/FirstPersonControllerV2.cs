using UnityEngine;

public class FirstPersonControllerV2 : Health
{
    [Header("Assign in Editor")]
    public CapsuleCollider playerCollider;
    public Transform orientation;
    public Rigidbody rb;
    [Header("Health")]
    [SerializeField]
    int startingMaxHealth = 100;
    [SerializeField]
    int startingCurrentHealth = 100;
    [SerializeField]
    bool startingInvulnerabilityState = false;
    [SerializeField]
    float startingInvulnerabilityTimeAfterDamage = 0.01f;
    [Header("Movement Options")]
    public float walkMoveSpeed;
    public float sprintMoveSpeed;
    public float crouchMoveSpeed;
    public float maxGroundAcceleration;
    public float maxGroundDeceleration;
    public float maxAirAcceleration;
    public float maxAirDeceleration;
    public float maxSlideDeceleration;
    public float jumpHeight;
    public float crouchDistance;
    public float crouchSpeed;
    [Header("Controller Options")]
    public float maxGroundAngle;
    public float maxUncrouchCheckAngle;
    public LayerMask canStopUncrouch;
    public float steepDetectionTolerance;
    public bool enableGroundSnapping;
    public float snapProbeDistance;
    [Header("Controls")]
    public KeyCode sprintKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;

    Rigidbody currentConnectedBody;
    Rigidbody lastConnectedBody;
    Vector2 playerInput;
    Vector3 desiredVelocity;
    Vector3 modifiedVelocity;
    Vector3 currentContactNormal;
    Vector3 steepNormal;
    Vector3 connectionVelocity;
    Vector3 connectionWorldPosition;
    Vector3 connectionLocalPosition;
    float newX;
    float newZ;
    float desiredSpeed;
    float currentAcceleration;
    float currentDeceleration;
    float playerHeight;
    float crouchedHeight;
    double stepsSinceGrounded;
    double stepsSinceJump;
    int groundContactCount;
    int steepContactCount;
    int uncrouchStopContactCount;
    bool desiredCrouchState;
    bool currentCrouchState;
    bool isSliding;
    bool uncrouchStopped => uncrouchStopContactCount > 0;
    bool isGrounded => groundContactCount > 0;
    bool onSteep => steepContactCount > 0;
    bool wantsToJump;

    public void Start()
    {
        SetHealthVars(startingCurrentHealth, startingMaxHealth, startingInvulnerabilityState, startingInvulnerabilityTimeAfterDamage);
        playerHeight = playerCollider.height;
        crouchedHeight = playerHeight - crouchDistance;
    }
    public void Update()
    {
        HandleInputs();
    }
    public void FixedUpdate()
    {
        UpdateStates();
        MovePlayer();
        //if the player is grounded and wants to jump then jump
        if (wantsToJump && isGrounded)
        {
            Jump();
        }
        //if we want to crouch and we aren't currently crouched then crouch
        if (desiredCrouchState == true)
        {
            Crouch();
        }
        //if we no longer want to be crouched and we are currently crouched then uncrouched
        if (desiredCrouchState == false)
        {
            Uncrouch();
        }
        //if we have a certain amount of speed and we desire to crouch, then slide
        if (desiredCrouchState == true && modifiedVelocity.magnitude > walkMoveSpeed)
        {
            isSliding = true;

        }
        //if we want to stop sliding the player can do so by uncrouching, or if their desire move speed less than or equal to that of the velocity in their desired move direction for smooth transitions between the sliding and crouched state
        if (desiredCrouchState == false && isSliding == true || Mathf.Clamp(Vector3.Dot(modifiedVelocity, desiredVelocity), 0, desiredSpeed) >= modifiedVelocity.magnitude && isSliding == true || modifiedVelocity.magnitude <= 0.01 && isSliding == true)
        {
            isSliding = false;
        }
        //take the calculated modified velocity and apply it to the rigid body
        rb.velocity = modifiedVelocity;
        ClearState();
    }
    void Crouch()
    {
        //move the player height towards the height of the desire crouch state
        playerCollider.height = Mathf.MoveTowards(playerCollider.height, crouchedHeight, Time.deltaTime * crouchSpeed);
        //change the crouch state if we've reached the desired height
        if (playerCollider.height == crouchedHeight)
        {
            currentCrouchState = true;
        }
    }
    void Uncrouch()
    {
        if (!uncrouchStopped)
        {
            //move the player height towards the height of the desire crouch state
            playerCollider.height = Mathf.MoveTowards(playerCollider.height, playerHeight, Time.deltaTime * crouchSpeed);
            //change the crouch state if we've reached the desired height
            if (playerCollider.height == playerHeight)
            {
                currentCrouchState = false;
            }
        }
        else
        {
            desiredCrouchState = true;
        }
    }
    void HandleInputs()
    {
        //get the horizontal and vertical movement axis and use them to create a clamped vector 2 to represent the direction we want the player to move
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        //if the sprint key is pressed and we aren't crouched or sliding, set our desired spreed to the sprint speed
        if (Input.GetKey(sprintKey) && desiredCrouchState == false && isSliding == false)
        {
            desiredSpeed = walkMoveSpeed + (Mathf.Clamp(Vector3.Dot(new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized, orientation.forward), 0, 1) * (sprintMoveSpeed - walkMoveSpeed));
        }
        //if the we are crouching or sliding, change the desired move speed to crouch move speed so we can smoothly transition between the sliding and crouched state
        else if (currentCrouchState == true || isSliding == true)
        {
            desiredSpeed = crouchMoveSpeed;
        }
        //otherwise, set our move speed to our walking speed
        else
        {
            desiredSpeed = walkMoveSpeed;
        }
        if (Input.GetKeyDown(crouchKey))
        {
            desiredCrouchState = !desiredCrouchState;
        }
        //jump when the jump key is pressed
        wantsToJump |= Input.GetKeyDown(jumpKey);
        //use our inputs to create a velocity representing our desired velocity on the horizontal and vertical axis using the player's orientation
        Vector3 forward = orientation.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = orientation.right;
        right.y = 0f;
        right.Normalize();
        //Debug.Log(desiredSpeed);
        desiredVelocity = (forward * playerInput.y + right * playerInput.x) * desiredSpeed;



    }
    void MovePlayer()
    {
        //change the acceleration and deceleration rate based on whether the character is in the air or grounded
        if (isGrounded)
        {
            currentAcceleration = maxGroundAcceleration;
            currentDeceleration = maxGroundDeceleration;
        }
        else
        {
            currentAcceleration = maxAirAcceleration;
            currentDeceleration = maxAirDeceleration;
        }
        //take axis and project them onto the contact plane to use in calculations taking into account the surface normal
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        Vector3 relativeModifiedVelocity = modifiedVelocity - connectionVelocity;
        //get the modified velocity and desired velocity along each axis we created previously taking into account any moving ground's velocity
        float currentX = Vector3.Dot(relativeModifiedVelocity, xAxis);
        float desiredX = Vector3.Dot(desiredVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeModifiedVelocity, zAxis);
        float desiredZ = Vector3.Dot(desiredVelocity, zAxis);

        //create a variable to hold what our calculated maximum acceleration per time loop will be
        float maxAccelerationChange = currentAcceleration * Time.deltaTime;
        float maxDecelerationChange = currentDeceleration * Time.deltaTime;

        if (!isSliding)
        {
            //if the speed along the calculated normal X value is less than the desired speed along the calculated x normal, interpolate using the acceleration value. Otherwise, use the deceleration value
            if (Mathf.Abs(currentX) < Mathf.Abs(desiredX))
            {
                newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxAccelerationChange);
            }
            else
            {
                newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxDecelerationChange);
            }
            //if the speed along the calculated normal Z value is less than the desired speed along the calculated Z normal, interpolate using the acceleration value. Otherwise, use the deceleration value
            if (Mathf.Abs(currentZ) < Mathf.Abs(desiredZ))
            {
                newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxAccelerationChange);
            }
            else
            {
                newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxDecelerationChange);
            }
            //take the modified velocity and add values to it based on calculated surface normal 
            modifiedVelocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }
        else
        {
            //When sliding, our velocity always moves towards 0 but can still be effected by outside sources like slopes
            newX = Mathf.MoveTowards(currentX, 0, maxSlideDeceleration);
            newZ = Mathf.MoveTowards(currentZ, 0, maxSlideDeceleration);
            modifiedVelocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }
        Debug.DrawLine(orientation.position, orientation.position + modifiedVelocity);
    }
    void ClearState()
    {
        //reset the states of all variables after they are used so they can be reaquired
        groundContactCount = 0;
        steepContactCount = 0;
        uncrouchStopContactCount= 0;
        steepNormal = Vector3.zero;
        currentContactNormal = Vector3.zero;
        connectionVelocity = Vector3.zero;
        lastConnectedBody = currentConnectedBody;
        currentConnectedBody = null;
    }
    void UpdateStates()
    {
        //add one to the steps since last grounded and since last jumped
        stepsSinceJump++;
        stepsSinceGrounded++;
        //set the modified velocity to the current velocity so we can modify it and set the rigid body's velocity to the modified value
        modifiedVelocity = rb.velocity;
        //if there is a body that the player is connected to then update the connection state if the detected rigidbody is kinematic or has a greater mass than the player's
        if (currentConnectedBody)
        {
            if (currentConnectedBody.isKinematic || currentConnectedBody.mass >= rb.mass)
                UpdateConnectionState();
        }

        if (isGrounded || SnapToGround() || CheckSteepContacts())
        {
            //when the player hits the ground, set the steps since last grounded to 0 and normalize the contact normal
            stepsSinceGrounded = 0;
            if (groundContactCount > 1)
            {
                currentContactNormal.Normalize();
            }
        }
        else
        {
            //if the player is not grounded, set the contact normal to an upward vector
            currentContactNormal = Vector3.up;
        }
    }
    void Jump()
    {
        stepsSinceJump = 0;
        //if the player's velocity has some sililarity do the direction of the contact normal 
        if (Vector3.Dot(modifiedVelocity, currentContactNormal) > 0f)
        {
            modifiedVelocity += currentContactNormal * Mathf.Max(Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight) - Vector3.Dot(modifiedVelocity, currentContactNormal));
        }
        //otherwise just jump normally
        else
        {
            modifiedVelocity += currentContactNormal * Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        }
        wantsToJump = false;
    }
    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        //returns the input vector projected on the current contact normal's plane
        return vector - currentContactNormal * Vector3.Dot(vector, currentContactNormal);
    }
    bool SnapToGround()
    {
        //only try to snap if snapping is enabled
        if (!enableGroundSnapping)
        {
            return false;
        }
        //only try to snap when the player has been off the ground for more that one physics step and has not jumped in recent steps
        if (stepsSinceGrounded > 1 || stepsSinceJump <= 2)
        {
            return false;
        }
        //only try to snap if our downward raycast gets a hit
        if (!Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, snapProbeDistance))
        {
            return false;
        }
        //only try to snap if our raycast hits a surface within range of the ground angles
        if (hit.normal.y < Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad))
        {
            return false;
        }
        // set the current contact normal to the raycast hit
        currentContactNormal = hit.normal;
        //get the current speed which the player is moving
        float speed = modifiedVelocity.magnitude;
        //determine how close our desired velocity is to aligning with surface normal
        float dot = Vector3.Dot(modifiedVelocity, hit.normal);
        //if the dot product is not already aligned, move the velocity towards aligning with the ground
        if (dot > 0f)
        {
            modifiedVelocity = (modifiedVelocity - hit.normal).normalized * speed;
        }
        currentConnectedBody = hit.rigidbody;
        return true;
    }
    bool CheckSteepContacts()
    {
        //if we see more than one steep contact then normalize the steep normal to use as virtual ground for player to jump off of
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            //if the upward normal of the steep contacts is within the ground angle range, create a virtual ground for the player to jump off
            if (steepNormal.y >= Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad))
            {
                groundContactCount = 1;
                currentContactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
    void UpdateConnectionState()
    {
        if (currentConnectedBody == lastConnectedBody)
        {
            //create a new variable to determine the movement between the last connected world position and the current connected saved in local connection position but converted to world space
            Vector3 connectionMovement = currentConnectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            //determine our connection velocity by taking the movement determined previously and dividing it by Time.deltatime
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        //set the world position variable to the location of the player
        connectionWorldPosition = rb.position;
        //set the local connection position to the position of the body relative to the connected body
        connectionLocalPosition = currentConnectedBody.transform.InverseTransformPoint(connectionWorldPosition);

    }
    public void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }
    public void OnCollisionExit(Collision collision)
    {
        EvaluateCollision(collision);
    }
    public void EvaluateCollision(Collision collision)
    {
        //look through all collisions and see if any match our evaluation parameter
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            //if they do, then set isGrounded to true and add the normals of the collision to calculate the average also keep track of the rigidbody of the object collided with so we can move the player if the surface is moving
            if (normal.y >= Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad))
            {
                groundContactCount++;
                currentContactNormal += normal;
                currentConnectedBody = collision.rigidbody;
            }
            if(normal.y <= -Mathf.Cos(maxUncrouchCheckAngle * Mathf.Deg2Rad) && canStopUncrouch == (canStopUncrouch | (1 << collision.gameObject.layer)))
            {
                uncrouchStopContactCount++;
            }
            //if the upward normal of our steep tolerance is within a certain threshhold then add it to the steep contacts count
            else if (normal.y > steepDetectionTolerance)
            {
                steepContactCount++;
                steepNormal += normal;
                //if there are no ground contacts, set the connected rigidbody in case there are no ground contacts
                if (groundContactCount == 0)
                {
                    currentConnectedBody = collision.rigidbody;
                }
            }

        }
    }
}