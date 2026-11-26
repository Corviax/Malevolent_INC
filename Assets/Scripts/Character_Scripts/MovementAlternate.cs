using UnityEngine;
using UnityEngine.EventSystems;
using static movementPlayer;

//script a usarse de movimiento

public class MovementAlternate : MonoBehaviour
{
    public CharacterController controller;
    private float movespeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    //Para controlar modificaciones de agachado
    public int _croch = 0;

    public MovementState state;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode walkKey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.C;

    public Transform playerBody;

    Vector3 moveDirection;


    [Header("GroundCheck")]
    public Transform groundCheck;
    public Transform ceilingCheck;
    public float groundDistance = 0.4f;
    private float ceilingDistance = 0.3f;
    Vector3 velocity;

    public float Playerheight;
    public float CrouchHeight;
    public LayerMask WhatIsGround;
    bool grounded;
    bool ceiling;
    private float startYScale;



    private void Start()
    {
        startYScale = transform.localScale.y;
        Playerheight = 2f;
    }

    // Update is called once per frame
    private void Update()
    {

        StateHandler();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, WhatIsGround);

        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        ceiling = Physics.CheckSphere(ceilingCheck.position,ceilingDistance, WhatIsGround);
        //Cuando se esta agachado

        if (ceiling && velocity.y > 0)
        {
            velocity.y = 0f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = transform.right * x + transform.forward * z;
        controller.Move(moveDirection * movespeed * Time.deltaTime);

        if(Input.GetButton("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchHeight, transform.localScale.z);
            Playerheight = CrouchHeight;

        }

        if (Input.GetKeyUp(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
    }



    private void StateHandler()
    {

        if (grounded && Input.GetKey(walkKey))
        {
            state = MovementState.walking;
            movespeed = walkSpeed;
        }

        else if (grounded)
        {
            state = MovementState.sprinting;
            movespeed = sprintSpeed;
        }

        if (Input.GetKey(crouchkey))
        {
            state = MovementState.crouching;
            movespeed = crouchSpeed;
        }
    }
}
