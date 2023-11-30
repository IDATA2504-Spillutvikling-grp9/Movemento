using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : Enemy
{
    [SerializeField] private float chaseDistance;       // Distance at which the BeeHive starts chasing the player.
    [SerializeField] private float stunDuration;        // Duration for which the BeeHive remains stunned.
    private float timer;                                // General purpose timer used for different states.



    protected override void Start()
    {
        base.Start(); // Call the Start method of the base class (Enemy).
        ChangeState(EnemyStates.BeeHive_Idle); // Initialize state to Idle.
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }



    protected override void UpdateEnemyStates()
    {
        // Calculate distance between BeeHive and Player.
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.BeeHive_Idle:
                // Transition to Chase state if player is within chase distance.
                if (dist < chaseDistance)
                {
                    ChangeState(EnemyStates.BeeHive_Chase);
                }
                break;

            case EnemyStates.BeeHive_Chase:
                // Move towards the player.
                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
                FlipBeeHive(); // Currently unused logic for flipping the BeeHive.
                break;

            case EnemyStates.BeeHive_Stunned:
                // Increment timer and check if stun duration has passed.
                timer += Time.deltaTime;
                if (timer > stunDuration)
                {
                    ChangeState(EnemyStates.BeeHive_Idle); // Reset to Idle state after stun.
                    timer = 0;
                }
                break;

            case EnemyStates.BeeHive_Death:
                // Handle death with a random delay.
                Death(Random.Range(5, 10));
                break;
        }
    }



    protected override void Death(float destroyTime)
    {
        rb.gravityScale = 12; // Increase gravity upon death.
        base.Death(destroyTime); // Call the Death method of the base class.
    }

    public override void EnemyHit(float damageDone, Vector2 hitDirection, float hitForce)
    {
        base.EnemyHit(damageDone, hitDirection, hitForce); // Call the EnemyHit method of the base class.

        // Change state based on health.
        if (health > 0)
        {
            ChangeState(EnemyStates.BeeHive_Stunned); // If still alive, become stunned.
        }
        else
        {
            ChangeState(EnemyStates.BeeHive_Death); // If no health left, transition to Death state.
        }
    }



    //ANIMATIONS BEING WORKED ON.
    /*     protected override void ChangeCurrentAnimation()
        {
            // Update animation parameters based on current state.
            anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.BeeHive_Idle);
            anim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.BeeHive_Chase);
            anim.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.BeeHive_Stunned);

            // Trigger death animation if in Death state.
            if(GetCurrentEnemyState == EnemyStates.BeeHive_Death)
            {
                anim.SetTrigger("Death");
            }
        } */



    // Logic to turn the sprites, but sprite is not on object, so idk how to fix this atm.
    void FlipBeeHive()
    {
        // Determine the current facing direction of the object
        bool isFacingRight = transform.localScale.x > 0;

        // Check the position of the PlayerController's instance relative to this object
        bool isPlayerOnTheLeft = PlayerController.Instance.transform.position.x < transform.position.x;

        // Flip the object only if the player is on the same side as the object's facing direction
        if ((isFacingRight && !isPlayerOnTheLeft) || (!isFacingRight && isPlayerOnTheLeft))
        {
            // Flip the object by multiplying the x scale by -1
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }



}
