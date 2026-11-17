using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class movementPlayer : MonoBehaviour
{
    [Header("Movement")]
    private float movespeed;
    public float walkspeed;
    public float runspeed;
    public float grounddrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float airSpeed;
    bool readytojump;

    [Header("Crouching")]
    public float crouchspeed;
    public float crouchYscale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode walkKey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.C;

    [Header("GroundCheck")]
    public float Playerheight;

    public LayerMask WhatIsGround;
    bool grounded;

    [Header("Slope handlign")]
    public float maxSlopeAngle;
    private RaycastHit slopehit;
    private bool exitingslope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readytojump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, Playerheight * 0.5f + 0.2f, WhatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.linearDamping = grounddrag;
        else
            rb.linearDamping = 0f;

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpkey) && readytojump && grounded)
        {
            readytojump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        }

        if (Input.GetKeyUp(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }
    }

    private void StateHandler()
    {

        if (grounded && Input.GetKey(walkKey))
        {
            state = MovementState.walking;
            movespeed = walkspeed;
        }

        else if (grounded)
        {
            state = MovementState.sprinting;
            movespeed = runspeed;
        }

        if (Input.GetKey(crouchkey))
        {
            state = MovementState.crouching;
            movespeed = crouchspeed;
        }

        else
        {
            state = MovementState.air;
            //movespeed = airSpeed;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if(OnSlope() && !exitingslope)
        {
            rb.AddForce(GetSlopeMoveDirection() * movespeed * 10f, ForceMode.Force);

            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 50f, ForceMode.Force);
        }

        //grounded
        if (grounded)
            rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Force);
        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * movespeed * airMultiplier, ForceMode.Force);


        rb.useGravity = !OnSlope();

    }


    private void SpeedControl()
    {

        if(OnSlope() && !exitingslope)
        {
            if (rb.linearVelocity.magnitude>movespeed)
                rb.linearVelocity = rb.linearVelocity.normalized * movespeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
            //Vector3 airvel = new Vector3(rb.angularVelocity.y, 0f);

            if (flatVel.magnitude > movespeed)
            {
                Vector3 limitedVel = flatVel.normalized * movespeed;
                rb.angularVelocity = new Vector3(limitedVel.x, rb.angularVelocity.y, limitedVel.z);
            }

        }
    }

    private void Jump()
    {
        exitingslope = true;

        rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readytojump = true;

        exitingslope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopehit, Playerheight * 0.5f + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopehit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopehit.normal).normalized;
    }
        

}



