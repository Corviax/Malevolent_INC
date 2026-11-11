using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class movementPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float movespeed;

    public float grounddrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readytojump;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

    [Header("GroundCheck")]
    public float Playerheight;

    public float PlayeSize;

    public LayerMask WhatIsGround;
    bool grounded;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readytojump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, Playerheight * 0.5f + 0.02f, WhatIsGround);
        MyInput();
        SpeedControl();



        if (grounded)
            rb.linearDamping = grounddrag;
        else
            rb.linearDamping = 0f;

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpkey) && readytojump && grounded)
        {
            readytojump = false;

            Jump();

            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //grounded
        if (grounded)
            rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Force);
        //in air
        else if (!grounded)
                rb.AddForce(moveDirection.normalized * movespeed * 10f * airMultiplier, ForceMode.Force);

    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

        if(flatVel.magnitude > movespeed)
        {
            Vector3 limitedVel = flatVel.normalized * movespeed;
            rb.angularVelocity = new Vector3(limitedVel.x, rb.angularVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readytojump = true;
    }
        

}



