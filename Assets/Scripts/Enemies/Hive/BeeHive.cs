using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;

    float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.BeeHive_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (currentEnemyState)
        {
            case EnemyStates.BeeHive_Idle:
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.BeeHive_Chase);
                }
                break;

            case EnemyStates.BeeHive_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));

                //FlipBeeHive();
                break;

            case EnemyStates.BeeHive_Stunned:
                timer += Time.deltaTime;

                if (timer > stunDuration)
                {
                    ChangeState(EnemyStates.BeeHive_Idle);
                    timer = 0;
                }
                break;
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if (health > 0)
        {
            ChangeState(EnemyStates.BeeHive_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.BeeHive_Death);
        }
    }


//Logic to turn the sprites, but sprite is not on object, so idk how to fix this atm.
/*     void FlipBeeHive()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    } */
}
