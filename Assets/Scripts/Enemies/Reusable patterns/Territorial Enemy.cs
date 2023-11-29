using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritorialEnemy : Enemy
{
    [Header("Territory Settings")]
    [SerializeField] private Collider2D territory; // Define the territory using a collider
    [SerializeField] private float attackRange = 2.0f; // Range at which the enemy will start attacking
    [SerializeField] private float chaseSpeed = 3.0f; // Speed when chasing the player

    private bool playerInTerritory = false;
    private Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        if (playerInTerritory)
        {
            ChasePlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTerritory = true;
            // You can also change the enemy state here if needed
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTerritory = false;
            // Reset to default behavior or state
        }
    }

    private void ChasePlayer()
{
    if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
    {
        AttackPlayer(); // Perform attack logic
    }
    else
    {
        // Chase the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

        // Check if the enemy's direction is opposite to that of the player's position
        // Flip the enemy to face the player if necessary
        if ((direction.x > 0 && transform.localScale.x > 0) || (direction.x < 0 && transform.localScale.x < 0))
        {
            Flip();
        }
    }
}

private void Flip()
{
    // Flip the sprite by scaling in the X direction
    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
}




    protected override void Attack()
    {
        // Implement attack logic here
    }

    // Optional: Implement the logic for attacking the player
    private void AttackPlayer()
    {
        // Example: Check if the player is within attack range and then deal damage
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            // Perform attack
            // Example: PlayerDamageController.Instance.TakeDamage(damage);
        }
    }
}
