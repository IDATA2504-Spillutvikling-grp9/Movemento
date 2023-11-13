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
    protected Rigidbody2D rb;
    

    protected enum EnemyStates
    {
        //SmallFrog
    }



    protected EnemyStates currentEnemyState;



    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    protected virtual void Update()
    {
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
            if(PlayerController.Instance.pState.alive)
            {
                PlayerDamageController.Instance.HitStopTime(0, 5, 0.5f);
            }
            //Debug.Log("Attack done");
            //add animation for bleed / freezeframe animation
        }
    }
    protected virtual void Attack()
    {
        PlayerDamageController.Instance.TakeDamage(damage);
    }
}
