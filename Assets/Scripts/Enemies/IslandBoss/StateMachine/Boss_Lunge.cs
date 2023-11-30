using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Lunge : StateMachineBehaviour
{
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponentInParent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = 0;
        //set _dir to 1 if facingright is true, -1 if not.
        int _dir = BossDragonKnight.Instance.facingRight ? 1 : -1;
        //sets the boss's velocity in the direction of dir, times the lungespeed, Y velocity is set to 0, so it wont move up or down.
        rb.velocity = new Vector2(_dir * (BossDragonKnight.Instance.LungeSpeed * 1), 0f);

        //calculates distance between player and boss, and checks if this is less than attackrange and that player hasnt already been damaged, to stop infinite dmg loop.
        //since there is no hitbox here you need to change the attackrange to change the hitbox, needs refactoring.
        if(Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= BossDragonKnight.Instance.attackRange-3 && 
            !BossDragonKnight.Instance.damagedPlayer)
        {
            //calls the take damage methods and pass in the boss damage as a parameter.
            PlayerDamageController.Instance.TakeDamage(BossDragonKnight.Instance.damage);
            //sets the bool damaged player to true to avoid conditional being triggered again.
            BossDragonKnight.Instance.damagedPlayer = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
