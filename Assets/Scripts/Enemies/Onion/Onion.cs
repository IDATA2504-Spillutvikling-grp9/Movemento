using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Enemy
{
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    private float timer;



    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Onion_Idle);
        rb.gravityScale = 12f; // Setting higher gravity for Onion type enemy.
    }



    protected override void UpdateEnemyStates()
    {
        // Check for death
        if (health <= 0)
        {
            Death(0.05f);
            return; // Ensures no further processing if dead.
        }

        // Determine ledge check start based on direction the onion is facing.
        Vector3 ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Onion_Idle:
                HandleIdleState(ledgeCheckStart, wallCheckDir);
                break;

            case EnemyStates.Onion_Spot:
                // Jump and transition to charge state.
                rb.velocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Onion_Charge);
                break;

            case EnemyStates.Onion_Charge:
                HandleChargeState();
                break;
        }
    }


    
    private void HandleIdleState(Vector3 ledgeCheckStart, Vector2 wallCheckDir)
    {
        // Check for ledge or wall to turn around.
        if (!Physics2D.Raycast(transform.position + ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround)
            || Physics2D.Raycast(transform.position, wallCheckDir, ledgeCheckX, whatIsGround)
            || Physics2D.Raycast(transform.position + ledgeCheckStart, wallCheckDir, ledgeCheckX).collider.CompareTag("Ground"))
        {
            // Flip direction.
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        // Spotting the player.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + ledgeCheckStart, wallCheckDir, ledgeCheckX * 10);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            ChangeState(EnemyStates.Onion_Spot);
        }

        // Movement.
        rb.velocity = new Vector2(speed * Mathf.Sign(transform.localScale.x), rb.velocity.y);
    }



    private void HandleChargeState()
    {
        timer += Time.deltaTime;

        if (timer < chargeDuration && Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
        {
            // Charging movement.
            rb.velocity = new Vector2(speed * chargeSpeedMultiplier * Mathf.Sign(transform.localScale.x), rb.velocity.y);
        }
        else
        {
            // Reset timer and return to idle.
            timer = 0;
            ChangeState(EnemyStates.Onion_Idle);
        }
    }



    protected override void ChangeCurrentAnimation()
    {
        // Update animation based on current state.
        if (GetCurrentEnemyState == EnemyStates.Onion_Idle)
        {
            anim.speed = 1;
        }
        else if (GetCurrentEnemyState == EnemyStates.Onion_Charge)
        {
            anim.speed = chargeSpeedMultiplier;
        }
    }
}
