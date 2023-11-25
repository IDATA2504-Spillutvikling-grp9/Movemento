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
        rb.gravityScale = 12f; // Set a higher gravity scale for the Small Frog.
    }



    protected override void UpdateEnemyStates()
    {
        // Check if the enemy's health is depleted and trigger death if so.
        if (health <= 0)
        {
            Death(0.05f); // Call the Death method with a slight delay.
        }
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.SmallFrog_Idle:
                // Check for ledges or walls and switch to Flip state if detected.
                Vector3 ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector3 wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                if (!Physics2D.Raycast(transform.position + ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround)
                    || Physics2D.Raycast(transform.position, wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    ChangeState(EnemyStates.SmallFrog_Flip); // Change to Flip state.
                }

                // Move the frog based on its current facing direction.
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
                // Increment the timer and flip direction when the timer exceeds the flip wait time.
                timer += Time.deltaTime;

                if (timer > flipWaitTime)
                {
                    timer = 0; // Reset the timer.
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); // Flip the direction.
                    ChangeState(EnemyStates.SmallFrog_Idle); // Change back to Idle state.
                }
                break;
        }
    }
}
