using System.Collections;
using UnityEngine;

public class BossLevel1 : Enemy
{
    [Header("Boss Phase Settings")]
    [SerializeField] private float phaseTwoThreshold = 0.66f; // Two-thirds health
    [SerializeField] private float phaseThreeThreshold = 0.33f; // One-third health
    [SerializeField] private GameObject minionPrefab; // The prefab for spawned minions

    [Header("Movement Settings")]
    [SerializeField] private float speed = 2.0f; // Speed of the boss
    private bool isMovingLeft = true; // Initial movement direction

    [Header("Player Tracking Settings")]
    [SerializeField] private Transform playerTransform; // Assign the player's transform in the inspector

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f; // Cooldown time between attacks
    private float attackTimer; // Timer to track attack cooldown
    [SerializeField] private Animator doorAnimator; // Drag the door's Animator component here in the inspector


    private int currentPhase = 1;

    protected override void Start()
    {
        base.Start();
        // Initialize boss-specific attributes here
        attackTimer = attackCooldown;
        if (playerTransform == null)
        {
            // Automatically find the player by tag if not set in the inspector
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        // Check and update the boss phase based on current health
        if (health / maxHealth <= phaseThreeThreshold && currentPhase < 3)
        {
            EnterPhaseThree();
        }
        else if (health / maxHealth <= phaseTwoThreshold && currentPhase < 2)
        {
            EnterPhaseTwo();
        }

        FollowPlayer(); // Add this line to handle movement

        // Attack logic
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        if (health <= 0)
        {
            OpenDoor(); // Open the door if the boss's health is 0 or less
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }


    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            // Determine the direction to the player
            float directionToPlayer = playerTransform.position.x - transform.position.x;

            // Move towards the player
            rb.velocity = new Vector2(Mathf.Sign(directionToPlayer) * speed, rb.velocity.y);

            // If the direction to the player is different from the moving direction, flip the boss
            if ((isMovingLeft && directionToPlayer > 0) || (!isMovingLeft && directionToPlayer < 0))
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Flip the moving direction
        isMovingLeft = !isMovingLeft;

        // Flip the sprite by scaling in the X direction
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        // Boss-specific hit logic
        if (health <= maxHealth * phaseTwoThreshold && currentPhase == 1)
        {
            EnterPhaseTwo();
        }
        else if (health <= maxHealth * phaseThreeThreshold && currentPhase == 2)
        {
            EnterPhaseThree();
        }

        // Boss-specific hit reactions
        StartCoroutine(FlashSprite());
    }

    private IEnumerator FlashSprite()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    private void EnterPhaseTwo()
    {
        currentPhase = 2;
        speed *= 1.5f; // Increase speed for phase two
        // Any other changes for phase two, e.g., jump height, aggression level
    }

    private void EnterPhaseThree()
    {
        currentPhase = 3;
        speed *= 2; // Increase speed further for phase three
        SpawnMinions();
    }

    private void SpawnMinions()
    {
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
    }

    protected override void Attack()
    {
        // Implement attack patterns based on the current phase
        switch (currentPhase)
        {
            case 1:
                // Phase 1 attack pattern
                break;
            case 2:
                // Phase 2 attack pattern
                break;
            case 3:
                // Phase 3 attack pattern
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Replace with your player damage logic
            // Make sure your player object has a tag of "Player" and a method to handle damage
            collision.gameObject.SendMessage("TakeDamage", damage);
        }
    }
}