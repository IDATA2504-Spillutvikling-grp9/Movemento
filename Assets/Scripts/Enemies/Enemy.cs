using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;

    protected float recoilTimer;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;


    protected enum EnemyStates
    {
        //small Frog
        SmallFrog_Idle,
        SmallFrog_Flip,

        //Bee Hive
        BeeHive_Idle,
        BeeHive_Chase,
        BeeHive_Stunned,
        BeeHive_Death,
    }



    protected EnemyStates currentEnemyState;



    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }



    protected virtual void Update()
    {
        UpdateEnemyStates();


        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }



    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
    }



    protected void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();
            if (PlayerController.Instance.pState.alive)
            {
                PlayerDamageController.Instance.HitStopTime(0, 5, 0.5f);
            }
            //Debug.Log("Attack done");
            //add animation for bleed / freezeframe animation
        }
    }


    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }


    protected virtual void UpdateEnemyStates() 
    {
        
    }

    
    protected virtual void ChangeState(EnemyStates _newState)
    {
        currentEnemyState = _newState;
    }
    protected virtual void Attack()
    {
        PlayerDamageController.Instance.TakeDamage(damage);
    }
}
