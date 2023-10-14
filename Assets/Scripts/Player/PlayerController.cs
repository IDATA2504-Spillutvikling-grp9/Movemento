using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Horizontal Movement Settings")]
    // The walking speed of the character in Horizontal direction.
    [SerializeField] private float walkSpeed = 10;

    [Header("Ground Check Settings")]
    [SerializeField] private float jumpForce = 35;
    [SerializeField] private float jumpForceOnWalls = 30;
    /* [SerializeField] private float jumpOppositeWalls = 30; */
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private LayerMask whatIsGround;


    private float xAxis;
    private Rigidbody2D rbComponent;


    public static PlayerController Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    // Sets the rbComponent to get the current objects RigidBody2D
    void Start()
    {
        rbComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Move();
        Jump();
    }


    // Sets the xAxis = to the input of the controller from -1 to 1 in horizontal direction.
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }


    //Setting the rigidbody2d components velocity in x and y direction. Vector2(x,y)
    //Where x = walkSpeed and xAxis input (-1 to 1)
    //and y = the standard velocity in the vertical axis. 
    private void Move()
    {
        rbComponent.velocity = new Vector2(walkSpeed * xAxis, rbComponent.velocity.y);
    }


    //Checks if the user is grounded by using Raycasts.
    //Takes the parameters as follows (raycast(from position, direction of ray, how long the ray should travel, Layer))
    public bool Grounded(Vector2 direction)
    {
        //if raycast finds an object tagged with ground, return true.
        if
            (Physics2D.Raycast(groundCheckPoint.position, direction, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), direction, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), direction, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*     
        Refactored, not in use anymore.

        public bool SlidingOnWall()
        {
            if
                (Physics2D.Raycast(groundCheckPoint.position, Vector2.left, groundCheckY, whatIsGround) ||
                Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckZ, 0, 0), Vector2.left, groundCheckY, whatIsGround) ||
                Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckZ, 0, 0), Vector2.left, groundCheckY, whatIsGround))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
     */

    void Jump()
    {

        //Variable jump height, if the user lets go of the Jump key early the vertical momentum is halted.
        if (Input.GetButtonUp("Jump") && rbComponent.velocity.y > 0)
        {
            rbComponent.velocity = new Vector2(rbComponent.velocity.x, 0);

           // Debug.Log("Jump button was pressed, vanlig");
        }

        //Checks if the object is near Ground and if jump is pressed, if that is the case, accelerate in vertical derection with jumpForce.
        if (Input.GetButtonDown("Jump") && Grounded(Vector2.down))
        {
            rbComponent.velocity = new Vector3(rbComponent.velocity.x, jumpForce);
            
            
            //Debug.Log("Jump button was pressed");

        }







      // Wall jumping, not will fix later.
       /*  //@@@@@@@@@@@@REMEMBER gliding on walls currently only works because the player physics models friction value is not set to 0. Adjust this friction value if you want to change how fast the player glides down walls
        //TODO: FIX LOGIC that does not let the player continue jumping up on one wall (maybe add )        
        //TRYING TO GET WALLJUMPS TO WORK. Checks if the object is near the wall and if jump is pressed, if that is the case, accelerate in vertical derection with jumpForce.
        if (Input.GetButtonDown("Jump") && Grounded(Vector2.right))
        {
            rbComponent.velocity = new Vector3(-jumpOppositeWalls, jumpForceOnWalls);
        }


        if (Input.GetButtonDown("Jump") && (Grounded(Vector2.left)))
        {
            rbComponent.velocity = new Vector3(jumpOppositeWalls, jumpForceOnWalls);
        } */


    }
}
