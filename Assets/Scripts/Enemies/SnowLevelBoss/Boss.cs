using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Boss class inherits from the Enemy class
public class Boss : Enemy
{
    // Serialized fields can be viewed and modified in the Unity Editor

    [Header("Boss Properties")]
    [SerializeField] private float chargStopTime;
    [SerializeField] private float timeBetweenCharges;
    [SerializeField] private float chaseDistance;
    [SerializeField] private Animator doorAnimator;

    // Private variables for internal use
    private bool isCharging = false;
    private float chargeTimer = 0f;
    private float stopTimer = 0f;
    private bool isDead = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Initialize boss-specific properties
        currentEnemyState = EnemyStates.Boss_Idle;
        anim.SetBool("Idle", true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Check if the boss is dead
        if (health <= 0)
        {
            isDead = true;
            OpenDoor();
            anim.SetTrigger("Death");
            // Call the Death method from the base class with a delay
            base.Death(2f);
        }
        // Update the boss's state
        UpdateEnemyStates();
    }

    // Method to open the door (if a door animator is specified)
    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }

    // Method to update the boss's state based on distance to the player
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        switch (currentEnemyState)
        {
            case EnemyStates.Boss_Idle:
                anim.SetBool("Walk", false);
                if (_dist < chaseDistance)
                {
                    BossIdleState();
                    anim.SetBool("Idle", true);
                }
                break;

            case EnemyStates.Boss_Chase:
                if (isDead == true)
                {
                    return;
                }
                anim.SetBool("Idle", false);
                BossChaseState();
                anim.SetBool("Walk", true);
                break;
        }
    }

    // Method representing the boss's idle state
    private void BossIdleState()
    {
        if (chargeTimer >= timeBetweenCharges)
        {
            chargeTimer = 0f;
            currentEnemyState = EnemyStates.Boss_Chase;
        }
        else
        {
            chargeTimer += Time.deltaTime;
        }
    }

    // Method representing the boss's chasing state
    private void BossChaseState()
    {
        float chargeStopTime = 2f;

        if (!isCharging)
        {
            Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;

            // Flip the boss's scale based on the player's position
            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            }
            else
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            rb.velocity = directionToPlayer * speed;

            // Stop charging after a certain duration
            if (stopTimer < chargeStopTime)
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                stopTimer = 0f;
                isCharging = true;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            float timeBetweenCharges = 3f;
            // Resume charging after a certain duration
            if (stopTimer < timeBetweenCharges)
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                stopTimer = 0f;
                isCharging = false;

                // If the boss is almost stationary, switch back to idle state
                if (rb.velocity.magnitude <= 0.01f)
                {
                    currentEnemyState = EnemyStates.Boss_Idle;
                    chargeTimer = 0f;
                    anim.SetBool("Walk", false);
                }
            }
        }
    }
}