using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFrog : Enemy
{
    [Header("Mob variables")]
    [SerializeField] private float flipWaitTime;                // Time to wait before flipping direction.
    [SerializeField] private float ledgeCheckX;                 // Distance to check for ledges horizontally.
    [SerializeField] private float ledgeCheckY;                 // Distance to check for ledges vertically.
    [SerializeField] private LayerMask whatIsGround;            // LayerMask to determine what is considered ground.
    private float timer;                                        // Timer used for timing the flips.



    protected override void Start()
    {
        base.Start(); // Call the Start method of the base class (Enemy).
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f; // Set a higher gravity scale for the Small Frog.
    }



    protected override void UpdateEnemyStates()
    {
        if(health <= 0)
        {
            Death(0.05f);
        }


        switch (GetCurrentEnemyState)
        {
            case EnemyStates.SmallFrog_Idle:
                Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround)
                || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    ChangeState(EnemyStates.SmallFrog_Flip);
                }

                if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                break;

            case EnemyStates.SmallFrog_Flip:
                timer += Time.deltaTime;

                if(timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.SmallFrog_Idle);
                }
                break;
        }
    }
}
