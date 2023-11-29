using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    
   [SerializeField] private float chargStopTime;
   [SerializeField] private float timeBetweenCharges;
   [SerializeField] private float chaseDistance;

   private bool isCharging = false;
   private float chargeTimer = 0f;
   private float stopTimer = 0f;

   protected override void Start() {
    base.Start();
    currentEnemyState = EnemyStates.Boss_Idle;
   }

   protected override void UpdateEnemyStates() {
    float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
    switch (currentEnemyState) {
        case EnemyStates.Boss_Idle:
        if(_dist < chaseDistance) {
            BossIdleState();
        }
        break;

        case EnemyStates.Boss_Chase:
        BossChaseState();
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
            Debug.Log("Moving");
            Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
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
            float timeBetweenCharges = 5f;
            if (stopTimer < timeBetweenCharges)
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                stopTimer = 0f;
                isCharging = false;
                currentEnemyState = EnemyStates.Boss_Idle;
            }
        }
    }
}