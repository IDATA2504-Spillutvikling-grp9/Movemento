using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDragonKnight : Enemy
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;    //point at which ground check happens
    [SerializeField] private float groundCheckX = 0.5f;     //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private float groundCheckY = 0.2f;     //how far down from ground chekc point is Grounded() checked
    [SerializeField] private LayerMask whatIsGround;        //sets the ground layer
    [Space(2)]

    [Header("Attack Settings")]
    [SerializeField] public Transform SideAttackTransform;         //position - the middle of the side attack area
    [SerializeField] public Transform UpAttackTransform;           //position - the middle of the up attack area
    [SerializeField] public Transform DownAttackTransform;         //position - the middle of the down attack area
    [Space(2)]

    [SerializeField] public Vector2 SideAttackArea;                //Size - of the side attack area
    [SerializeField] public Vector2 UpAttackArea;                  //Size - of the up attack area 
    [SerializeField] public Vector2 DownAttackArea;                //Size - of the down attack are
    [Space(2)]

    [Header("Running Settings")]
    [SerializeField] public float runSpeed;                // Runspeed of player used for run state.
    [Space(2)]

    [Header("Lunge Settings")]
    [SerializeField] public float LungeSpeed;
    [Space(2)]

    [HideInInspector] public bool facingRight;              // used to check which way the boss is facing
    public float attackRange;                               // range of the boss attacks
    public float attackTimer;                               // timer for boss attacks

    [Header("Particle Settings")]
    [SerializeField] GameObject slashEffect;                // sprite / effect used for the attack of the boss
    [Space(3)]
    [HideInInspector] public bool damagedPlayer = false;    // checks for damage to player
    public static BossDragonKnight Instance;                // Setting up a singleton instance of the Boss.
    private bool isDeathTriggered = true;
    bool alive;


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
        // DontDestroyOnLoad(gameObject);
    }



    /*
    Method is only used for Drawing the "hitboxes" onto the screen.
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Draw wireframe cubes to visualize the attack area
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }



    // Initialize the BossDragonKnight
    protected override void Start()
    {
        base.Start();
        // Grab components from child objects
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        ChangeState(EnemyStates.DragonKnight_Stage1);       // Set initial enemy state
        alive = true;
    }



    protected override void Update()
    {
        if (health <= 0 && isDeathTriggered)
        {
            Death(0);

        }
        // Decrement attack countdown if not currently attacking
        if (!attacking)
        {
            attackCountDown -= Time.deltaTime;
        }
    }



    // Handle changes in enemy states
    protected override void UpdateEnemyStates()
    {
        if (PlayerController.Instance != null)
        {
            switch (GetCurrentEnemyState)
            {
                case EnemyStates.DragonKnight_Stage1:
                    break;

                case EnemyStates.DragonKnight_Stage2:
                    break;

                case EnemyStates.DragonKnight_Stage3:
                    break;

                case EnemyStates.DragonKnight_Stage4:
                    break;

            }
        }
    }



    // Check if the boss is on the ground
    public bool Grounded()
    {
        //if raycast finds an object tagged with ground, return true.
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
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



    // Flip the boss to face the player
    public void Flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
            facingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
            facingRight = true;
        }
    }



    // Collision handling (inherits from base Enemy class)
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }



    #region attacking
    #region variables
    [HideInInspector] public bool attacking;                    // Flag for if the boss is currently attacking
    [HideInInspector] public float attackCountDown;             // Countdown timer for attacks
    [HideInInspector] public bool parrying;                     // Parry bool
    [HideInInspector] public Vector2 moveToPosition;
    [HideInInspector] public bool diveAttack;
    public GameObject divingCollider;
    private bool isFlashing = false;
    public GameObject pillar;
    private float hitFlashSpeed;

    #endregion

    #region Control

    // Handle the boss's attack logic
    public void AttackHandler()
    {
        // Check if the current state is DragonKnight Stage 1 and if the player is within attack range
        if (currentEnemyState == EnemyStates.DragonKnight_Stage1)
        {
            if (Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= attackRange)
            {
                // Start the TripleSlash attack coroutine
                StartCoroutine(TripleSlash());
            }
            else
            {
                StartCoroutine(Lunge());
                //DiveAttackJump();
            }
        }
    }


    // Reset all attack states and stop the TripleSlash coroutine
    public void ResetAllAttacks()
    {
        attacking = false;

        StopCoroutine(TripleSlash());
        StopCoroutine(Lunge());
        StopCoroutine(Parry());
        StopCoroutine(Slash());
    }


    #endregion

    #region Stage 1
    // Coroutine for executing a triple slash attack
    IEnumerator TripleSlash()
    {
        attacking = true;                       // Set attacking flag to true
        rb.velocity = Vector2.zero;             // Stop movement
        float RandonValueUnder1 = UnityEngine.Random.Range(0.5f, 0.8f);

        // Perform three slashes with delays in between
        anim.SetTrigger("Slash");

        SlashAngle();
        yield return new WaitForSeconds(RandonValueUnder1);
        anim.ResetTrigger("Slash");

        anim.SetTrigger("Slash");
        SlashAngle();
        yield return new WaitForSeconds(RandonValueUnder1);
        anim.ResetTrigger("Slash");

        anim.SetTrigger("Slash");
        SlashAngle();
        yield return new WaitForSeconds(RandonValueUnder1);
        anim.ResetTrigger("Slash");

        // Reset attack state
        ResetAllAttacks();
    }


    void SlashAngle()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x ||
            PlayerController.Instance.transform.position.x < transform.position.x)
        {
            Instantiate(slashEffect, SideAttackTransform);
        }
        if (PlayerController.Instance.transform.position.y > transform.position.y)
        {
            SlashEffectAtAngle(slashEffect, 80, UpAttackTransform);
        }
        if (PlayerController.Instance.transform.position.y < transform.position.y)
        {
            SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);
        }
    }



    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }


    IEnumerator Lunge()
    {
        Flip();
        attacking = true;

        anim.SetBool("Lunge", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Lunge", false);
        damagedPlayer = false;
        ResetAllAttacks();
    }

    IEnumerator Parry()
    {
        attacking = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("Parry", true);
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("Parry", false);
        parrying = false;
        ResetAllAttacks();
    }

    IEnumerator Slash()
    {
        attacking = true;                       // Set attacking flag to true
        rb.velocity = Vector2.zero;             // Stop movement

        // Perform the slash / reposte
        anim.SetTrigger("Slash");
        //SlashAngle();
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Slash");

        ResetAllAttacks();
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        if (!parrying)
        {
            base.EnemyHit(_damageDone, _hitDirection, _hitForce);
            ResetAllAttacks();
            anim.SetTrigger("TookDamage");


            ResetAllAttacks(); //cancel any current attack to avoid bugs 
            StartCoroutine(Parry());


        }
        else
        {
            StopCoroutine(Parry());
            parrying = false;
            ResetAllAttacks();
            StartCoroutine(Slash());  //riposte
        }
    }

    #endregion

    #region DEATH

    protected override void Death(float _destroyTime)
    {
        ResetAllAttacks();
        rb.velocity = new Vector2(rb.velocity.x, -25);
        anim.SetBool("Parry", false);
        anim.SetBool("Lunge", false);
        anim.SetTrigger("Die");
        Debug.Log("Enemy Die trigger hit");

        isDeathTriggered = false;

    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }


    #endregion




    /* #region Stage 2

    void DiveAttackJump()
    {
        attacking = true;
        moveToPosition = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y + 10);
        Debug.Log("Dive attack jump called");
        diveAttack = true;
        anim.SetBool("Jump", true);
    }


    public void Dive()
    {
        anim.SetBool("Dive", true);
        anim.SetBool("Dive", false);
    }


    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.GetComponent<PlayerController>() != null && diveAttack)
        {
            _other.GetComponent<PlayerDamageController>().TakeDamage(damage * 2);
            PlayerController.Instance.pState.recoilingX = true;
        }
    }

    public void DivingPillars()
    {
        Vector2 _impactPoint = groundCheckPoint.position;
        float _spawnDistance = 5;

        for(int i = 0; i < 10; i++)
        {
            Vector2 _pillarSpawnPointRight = _impactPoint + new Vector2(_spawnDistance, 0);
            Vector2 _pillarSpawnPointLeft = _impactPoint - new Vector2(_spawnDistance, 0);
            Instantiate(pillar, _pillarSpawnPointRight, Quaternion.Euler(0, 0, -90));
            Instantiate(pillar, _pillarSpawnPointLeft, Quaternion.Euler(0, 0, -90));

            _spawnDistance += 5;
        }
        ResetAllAttacks();
    }


    #endregion */
    #endregion
}

