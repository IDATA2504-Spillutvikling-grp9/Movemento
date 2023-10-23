using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int airJumpCounter = 0;                         //keeps track of how many times the player has jumped in the air
    private float gravity;                                  //stores the gravity scale at start
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

    [Header("Attack Settings")]
    [SerializeField] Transform SideAttackTransform;         //position - the middle of the side attack area
    [SerializeField] Transform UpAttackTransform;           //position - the middle of the up attack area
    [SerializeField] Transform DownAttackTransform;         //position - the middle of the down attack area
    [Space(5)]
    [SerializeField] Vector2 SideAttackArea;                //Size - of the side attack area
    [SerializeField] Vector2 UpAttackArea;                  //Size - of the up attack area 
    [SerializeField] Vector2 DownAttackArea;                //Size - of the down attack are
    [Space(5)]
    [SerializeField] private float damage;                  //Damage the player does to an enemy
    [SerializeField] private GameObject slashEffect;        //Effects - Slash effect.
    [SerializeField] LayerMask attackableLayer;             //Layer - for the player to attack on
    float timeBetweenAttack, timeSinceAttack;
    float restoreTimeSpeed;
    bool attack = false;
    bool restoreTime;
    [Space(5)]

    [Header("Recoil Settings:")]
    [SerializeField] private int recoilXSteps = 5;          //how many FixedUpdates() the player recoils horizontally for
    [SerializeField] private int recoilYSteps = 5;          //how many FixedUpdates() the player recoils vertically for
    [SerializeField] private float recoilXSpeed = 100;      //the speed of horizontal recoil
    [SerializeField] private float recoilYSpeed = 100;      //the speed of vertical recoil
    private int stepsXRecoiled, stepsYRecoiled;             //the number of steps recoiled horizontally and verticall
    [Space(5)]

    [Header("Health Settings")]
    [SerializeField] GameObject bloodSpurt;                 // Bloodspurt effect created with unity's particle system
    [SerializeField] float hitFlashSpeed;
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;
    public delegate void OnHealthChangedDelegate();
    public int health;                                      // health stat of the player
    public int maxHealth;                                   // maximum health the player can have
    [Space(5)]



    [HideInInspector] public PlayerStateList pState;
    private float xAxis;
    private float yAxis;
    Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool canDash = true;
    private bool dashed;


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
        Health = maxHealth;
    }

    // Start is called before the first frame update
    // Sets the rigidbody to get the current objects RigidBody2D
    //
    void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
    }


    // Method is only used for Drawing the "hitboxes" onto the screen.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing) return;
        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
        restoreTimeScale();
        FlashWhileInvincible();
    }

    private void FixedUpdate() 
    {
        if (pState.dashing) return;
        Recoil();
    }


    // Sets the xAxis = to the input of the controller from -1 to 1 in horizontal direction.
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
    }

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


    //Setting the rigidbody2d components velocity in x and y direction. Vector2(x,y)
    //Where x = walkSpeed and xAxis input (-1 to 1)
    //and y = the standard velocity in the vertical axis. 
    private void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());        //Sets the Walking bool in animator to true, when conditions is met.
    }

    //checking if bools are true and jump is pressed, if not it runs the Dash coroutine.
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
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
                Instantiate(slashEffect, SideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, 60, UpAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);
            }
        }
    }

    //Function used to check if an object is hittable
    //The functions collects an array of objects that are within the position and area of the parameters passed in. Also checks if the attackable layer is added in.
    // Loops through the array to check if it has hit something, if it has, do damage.

    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit
                (damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
        }
    }


    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }


    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {

            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }


    //Checks if the user is grounded by using Raycasts.
    //Takes the parameters as follows (raycast(from position, direction of ray, how long the ray should travel, Layer))
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


    public void TakeDamage(float _damage)
    {
        Health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }

    void FlashWhileInvincible() 
    {
        sr.material.color = pState.invincible ?
        Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : //Pingpong changes it from white to black and black to white as long as parameters are forfilled
        Color.white;
    }

    void restoreTimeScale()
    {
        if(restoreTime)
        {
            if(Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;

        if(_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if(onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }


    void Jump()
    {

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
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
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            pState.jumping = false;
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