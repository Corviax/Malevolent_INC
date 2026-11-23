using UnityEngine;
using UnityEngine.EventSystems;

public class MovementAlternate : MonoBehaviour
{
    public CharacterController controller;
    //Rigidbody rb;
    private float movespeed;
    public float speed = 12f;
    public float walkspeed;
    public float runspeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    float horizontalInput;
    float verticalInput;

    public Transform playerBody;

    Vector3 moveDirection;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode walkKey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.C;


    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    Vector3 velocity;

    public float Playerheight;
    public LayerMask WhatIsGround;
    bool grounded;
    private float startYScale;

    [Header("Default state of player")]
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
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, Playerheight * 0.5f + 0.1f, WhatIsGround);
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, WhatIsGround);

        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = transform.right * x + transform.forward* z;

        controller.Move(moveDirection * speed * Time.deltaTime);

        if(Input.GetButton("Jump") && grounded)
        {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);



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

    }
    //Codigo de referencia
}
