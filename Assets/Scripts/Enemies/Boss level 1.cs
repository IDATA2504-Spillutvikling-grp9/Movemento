using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosslevel1 : Enemy // Inherit from the Enemy class
{
    [Header("Boss Specific Settings")]
    // Add boss-specific attributes here
    [SerializeField] private float attackCooldown;  // Time between attacks
    private float attackTimer;                     // Timer to track attack cooldown

    // Use the Start method to initialize boss-specific attributes
    protected override void Start()
    {
        base.Start(); // Call the base class Start method
        // Initialize boss-specific attributes here
    }

    // Override the Update method if the boss has specific behavior patterns
    protected override void Update()
    {
        base.Update(); // Call the base class Update method

        // Boss-specific update logic here
        if (health <= 0)
        {
            // You might want a different death behavior for the boss
            Death(0.5f); // Call the death method with a delay
        }

        // Handle attack cooldown
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            // Perform an attack
            Attack();
            attackTimer = attackCooldown; // Reset the attack timer
        }
    }

    // You can override the Attack method to implement boss-specific attacks
    protected override void Attack()
    {
        base.Attack(); // Call the base class Attack method if you want the boss to deal damage in the same way
        // Additional boss-specific attack logic here
    }

    // Override any other methods as needed to provide specific boss behaviors
    protected override void Death(float _destroyTime)
    {
        // Boss-specific death logic here
        base.Death(_destroyTime); // Optionally call the base class Death method
        // For example, trigger a cutscene or an explosion effect
    }
}
