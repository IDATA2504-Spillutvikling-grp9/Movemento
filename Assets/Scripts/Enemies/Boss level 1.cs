using System.Collections;
using UnityEngine;

public class BossLevel1 : Enemy
{
    [Header("Boss Phase Settings")]
    [SerializeField] private float phaseTwoThreshold = 0.66f; // Two-thirds health
    [SerializeField] private float phaseThreeThreshold = 0.33f; // One-third health
    [SerializeField] private GameObject minionPrefab; // The prefab for spawned minions

    [Header("Movement Settings")]
    private bool isMovingLeft = true; // Initial movement direction

    [Header("Player Tracking Settings")]
    [SerializeField] private Transform playerTransform; // Assign the player's transform in the inspector

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f; // Cooldown time between attacks

    [Header("Proximity Attack Settings")]
    [SerializeField] private float attackRange = 5f; // Range within which the boss will attack the player

    private float attackTimer; // Timer to track attack cooldown
    [Header("Door Settings")]
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

        // Phase logic
        if (health / maxHealth <= phaseThreeThreshold && currentPhase < 3)
        {
            EnterPhaseThree();
        }
        else if (health / maxHealth <= phaseTwoThreshold && currentPhase < 2)
        {
            EnterPhaseTwo();
        }

        FollowPlayer();

        // Proximity attack logic
        if (IsPlayerInRange())
        {
            anim.SetTrigger("Attack");
        }
        else { anim.SetTrigger("StopAttack"); }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime; // Decrease timer only if an attack is not happening
        }

        if (health <= 0)
        {
            Death(10000); // Open the door if the boss's health is 0 or less
        }
    }


    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            bool isPlayerCloseEnough = distanceToPlayer <= attackRange;

            // Determine if the player is in front of the boss
            float directionToPlayer = playerTransform.position.x - transform.position.x;
            bool isPlayerInFront = isMovingLeft ? directionToPlayer < 0 : directionToPlayer > 0;

            return isPlayerCloseEnough && isPlayerInFront;
        }
        return false;
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
        SpawnMinions();
    }

    private void EnterPhaseThree()
    {
        currentPhase = 3;
        speed *= 2; // Increase speed further for phase three
        SpawnMinions();
    }

    private void SpawnMinions()
    {
        if (minionPrefab != null)
        {
            // Offset values to space out the minions
            float xOffset = 1.0f; // Horizontal offset
            float yOffset = 3.0f; // Vertical offset (adjust this to spawn minions higher)

            // Position for the first minion (to the left and a bit higher)
            Vector3 spawnPosition1 = new Vector3(transform.position.x - xOffset, transform.position.y + yOffset, transform.position.z);
            Instantiate(minionPrefab, spawnPosition1, Quaternion.identity);

            // Position for the second minion (to the right and a bit higher)
            Vector3 spawnPosition2 = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
            Instantiate(minionPrefab, spawnPosition2, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Minion prefab is not assigned or has been destroyed.");
        }
    }


    protected override void Death(float _destroyTime)
    {
        OpenDoor();
        anim.SetTrigger("Dead");
        Debug.Log("Enemy Die trigger hit");

    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }


}