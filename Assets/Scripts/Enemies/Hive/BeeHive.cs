using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : Enemy
{
    [SerializeField] private float chaseDistance;       // Distance at which the BeeHive starts chasing the player.
    [SerializeField] private float stunDuration;        // Duration for which the BeeHive remains stunned.
    [SerializeField] private SpriteRenderer[] sra;
    private float timer;                                // General purpose timer used for different states.

    protected override void Start()
    {
        base.Start(); // Call the Start method of the base class (Enemy).
        ChangeState(EnemyStates.BeeHive_Idle); // Initialize state to Idle.
        sra = GetComponentsInChildren<SpriteRenderer>();
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
                FlipBeeHive(); // Logic for flipping the BeeHive.
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

    // Logic to turn the sprites.
    void FlipBeeHive()
    {
        foreach (SpriteRenderer sr in sra)
        {
            // Determine if the player is to the left or right of the beehive
            bool shouldFlip = PlayerController.Instance.transform.position.x < transform.position.x;

            // Get the current scale
            Vector3 currentScale = sr.transform.localScale;

            // Set the x component of the scale to 1 or -1 based on the player's position
            currentScale.x = shouldFlip ? -1 : 1;

            // Apply the new scale
            sr.transform.localScale = currentScale;
        }
    }
}
