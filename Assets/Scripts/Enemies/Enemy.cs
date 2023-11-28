using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Serialized fields for enemy attributes
    [SerializeField] protected float health;                        // Current health of the enemy.
    [SerializeField] protected float maxHealth;                     // Maximum health of the enemy.
    [SerializeField] protected float recoilLength;                  // Duration of the recoil effect after being hit.
    [SerializeField] protected float recoilFactor;                  // Factor by which the enemy recoils when hit.
    [SerializeField] protected bool isRecoiling = false;            // Flag to check if the enemy is currently recoiling.
    [SerializeField] public float speed;                            // Movement speed of the enemy.
    [SerializeField] public float damage;                        // Damage dealt by the enemy.

    [Header("Blood Effect")]
    [SerializeField] protected GameObject enemyBloodSpatter;        // Prefab for the blood spatter effect.
    [SerializeField] private float removeBloodVFX = 5.5f;           // Time after which the blood effect is removed.
    [Space(5)]

    private HealthBar healthBar;                                    // Reference to the health bar UI component.

    // Internal variables
    protected float recoilTimer;                                    // Timer to track the duration of recoil.
    protected Animator anim;                                        // Reference to the animator component.
    protected SpriteRenderer sr;                                    // Reference to the sprite renderer component.
    [SerializeField] protected Rigidbody2D rb;                                       // Reference to the Rigidbody2D component.
    protected EnemyStates currentEnemyState;                        // Current state of the enemy.



    // Enumeration of possible enemy states.
    protected enum EnemyStates
    {
        Onion_Idle, Onion_Charge, Onion_Spot,                                                   // States for the Onion enemy.
        SmallFrog_Idle, SmallFrog_Flip,                                                         // States for the Small Frog enemy.
        BeeHive_Idle, BeeHive_Chase, BeeHive_Stunned, BeeHive_Death,                            // States for the Bee Hive enemy.
        Boss_Idle, Boss_Chase, Boss_Death,                                                      // States for the Level 2 Boss enemy.
        DragonKnight_Stage1, DragonKnight_Stage2, DragonKnight_Stage3,DragonKnight_Stage4,      // States for the dragonknight boss stages.
    }



    // Property to get and set the current enemy state. Changes animation on state change.
    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currentEnemyState; }
        set
        {
            if(currentEnemyState != value)
            {
                currentEnemyState = value;
                ChangeCurrentAnimation();
            }
        }
    }



    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        maxHealth = health;
    }



    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);                // Destroy the enemy if health is 0 or less.
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;  // Incrementing the recoil timer.
            }
            else
            {
                isRecoiling = false;            // Resetting recoil state.
                recoilTimer = 0;                // Resetting the recoil timer.
            }
        }
        else
        {
            UpdateEnemyStates();                // Update the enemy states if not recoiling.
        }
    }



    // Method called when the enemy is hit.
    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone; // Reducing health by the damage done.
        healthBar.UpdateHealthBar(health, maxHealth); // Updating the health bar UI.
        if (!isRecoiling)
        {
            GameObject _blood = Instantiate(enemyBloodSpatter, transform.position, Quaternion.identity);        // Creating blood spatter effect.
            Destroy(_blood, removeBloodVFX);                                                                    // Destroying the blood effect after a delay.
            rb.velocity = _hitForce * recoilFactor * _hitDirection;                                             // Applying recoil force.
        }
    }



    // Method called when the enemy stays in collision.
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !PlayerController.Instance.pState.invincible && health > 0)
        {
            Attack(); // Perform attack.
            if (PlayerController.Instance.pState.alive)
            {
                PlayerDamageController.Instance.HitStopTime(0, 5, 0.5f);            // Apply hit stop time effect.
            }
        }
    }



    // Method to handle enemy death.
    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);                      // Destroy the enemy after a delay.
    }



    // Method to update enemy states. To be overridden in derived classes.
    protected virtual void UpdateEnemyStates() 
    {

    }



    // Method to change the current animation. To be overridden in derived classes.
    protected virtual void ChangeCurrentAnimation() 
    {

    }



    // Method to change the enemy's state.
    protected virtual void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;                           // Set the new state.
    }



    // Method to handle enemy attacks. To be overridden in derived classes.
    protected virtual void Attack()
    {
        PlayerDamageController.Instance.TakeDamage(damage);         // Deal damage to the player.
    }
}
