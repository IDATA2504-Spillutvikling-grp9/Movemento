using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 10;          //sets the players movement speed on the ground
    [Space(5)]

    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 35;          //sets how hight the player can jump
    [SerializeField] private int jumpBufferFrames;          //sets the max amount of frames the jump buffer input is stored
    private int jumpBufferCounter = 0;                      //stores the jump button input
    [SerializeField] private float coyoteTime;              //sets the max amount of frames the Grounded() bool is stored
    private float coyoteTimeCounter = 0;                    //stores the Grounded() bool
    [SerializeField] private int maxAirJumps;               //the max no. of air jumps
    public int airJumpCounter = 0;                          //keeps track of how many times the player has jumped in the air
    public float gravity;                                   //stores the gravity scale at start
    [Space(5)]

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;    //point at which ground check happens
    [SerializeField] private float groundCheckX = 0.5f;     //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private float groundCheckY = 0.2f;     //how far down from ground chekc point is Grounded() checked
    [SerializeField] private LayerMask whatIsGround;        //sets the ground layer
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;               //speed of the dash
    [SerializeField] private float dashTime;                //amount of time spent dashing
    [SerializeField] private float dashCooldown;            //amount of time between dashes
    [SerializeField] GameObject dashEffect;                 //Lets us put in an empty game object with an Animation for the dash Effect on the ground.
    [Space(5)]

    [HideInInspector] public PlayerStateList pState;        // State of the Player
    public static PlayerController Instance;                // Singleton instance
    private Rigidbody2D rb;
    public Animator anim;
    private bool canDash = true;
    private bool dashed;
    private float xAxis;
    private float yAxis;

    private void Awake()
    {
        if (Instance != null && Instance != this)   // Check if an instance already exists and if it's not the current one
        {
            Destroy(gameObject);                    // Destroy this GameObject to maintain Singleton pattern          
        }
        else
        {
            Instance = this;                        // Set this instance as the Singleton instance
        }
        DontDestroyOnLoad(gameObject);
    }



    /*
        Start is called before the first frame update
        Sets the rigidbody to get the current objects RigidBody2D
    */
    void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
    }



    /*
        Update is called once per frame
    */
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing) return;
        Flip();
        Move();
        Jump();
        StartDash();
    }



    /*
        Sets the xAxis = to the input of the controller from -1 to 1 in horizontal direction.
    */
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }



    /*
        Matches direction player is facing with what the sprite is showing.
    */
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            pState.lookingRight = true;
        }
    }



    /*
        Setting the rigidbody2d components velocity in x and y direction. Vector2(x,y)
        where x = walkSpeed and xAxis input (-1 to 1)
        and y = the standard velocity in the vertical axis. 
    */
    private void Move()
    {
        if (!pState.healing)
        {
            rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
            anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());      //Sets the Walking bool in animator to true, when conditions is met.
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);                    //quick fix, to stop the player from moving while healing. REFACTOR LATER.
        }
    }



    /*
        checking if bools are true and jump is pressed, if not it runs the Dash coroutine.
    */
    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            // Starts the coroutine (basically the loop) of the Dash clash (Ienumerator).
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }



    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        int _dir = pState.lookingRight ? 1 : -1;
        rb.velocity = new Vector2(_dir * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }



    /*
    Checks if the user is grounded by using Raycasts.
    Takes the parameters as follows (raycast(from position, direction of ray, how long the ray should travel, Layer))
    */
    public bool Grounded()
    {
        //if raycast finds an object tagged with ground, return true.
        if
            (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    void Jump()
    {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !pState.jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);

                pState.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;

                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);

            }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 3)
        {
            pState.jumping = false;

            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        anim.SetBool("Jumping", !Grounded());
    }



    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}