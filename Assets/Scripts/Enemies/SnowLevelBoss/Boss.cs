using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Boss : Enemy
{
    
   [SerializeField] private float chargStopTime;
   [SerializeField] private float timeBetweenCharges;
   [SerializeField] private float chaseDistance;
   [SerializeField] private Animator doorAnimator;

   private bool isCharging = false;
   private float chargeTimer = 0f;
   private float stopTimer = 0f;

   private bool isDead = false;

   protected override void Start() {
    base.Start();
    currentEnemyState = EnemyStates.Boss_Idle;
    anim.SetBool("Idle", true);
   }

   protected override void Update() {
    if(health <= 0) {
        isDead = true;
        OpenDoor();
        anim.SetTrigger("Death");
        base.Death(2f);
    }
    UpdateEnemyStates();
   }

   private void OpenDoor() {
    if(doorAnimator != null) {
        doorAnimator.SetTrigger("Open");
    }
   }

   protected override void UpdateEnemyStates() {
    float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
    switch (currentEnemyState) {
        case EnemyStates.Boss_Idle:
        anim.SetBool("Walk", false);
        if(_dist < chaseDistance) {
            BossIdleState();
            anim.SetBool("Idle", true);
        }
        break;

        case EnemyStates.Boss_Chase:
        if(isDead == true) {
            return;
        }
        anim.SetBool("Idle", false);
        BossChaseState();
        anim.SetBool("Walk", true);
        
        break;
    }
   }

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

    private void BossChaseState()
    {
        float chargeStopTime = 2f;

        if (!isCharging)
        {
            Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;

            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            }
            else
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            rb.velocity = directionToPlayer * speed;

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
            if (stopTimer < timeBetweenCharges)
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                stopTimer = 0f;
                isCharging = false;

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