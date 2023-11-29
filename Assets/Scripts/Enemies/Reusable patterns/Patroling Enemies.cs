using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform leftBoundary;  // Left boundary of the patrol area.
    [SerializeField] private Transform rightBoundary; // Right boundary of the patrol area.
    [SerializeField] private float patrolSpeed = 2.0f; // Speed at which the enemy patrols.

    private bool movingLeft = true; // Determines the direction of patrol.

    protected override void Start()
    {
        base.Start();
        speed = patrolSpeed; // Set the speed of the enemy for patrolling.
    }

    protected override void Update()
    {
        base.Update();
        Patrol(); // Add this line to handle patrolling movement.
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            // Move left
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);

            // Check if reached the left boundary
            if (transform.position.x <= leftBoundary.position.x)
            {
                movingLeft = false;
                Flip();
            }
        }
        else
        {
            // Move right
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);

            // Check if reached the right boundary
            if (transform.position.x >= rightBoundary.position.x)
            {
                movingLeft = true;
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Flip the moving direction
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
