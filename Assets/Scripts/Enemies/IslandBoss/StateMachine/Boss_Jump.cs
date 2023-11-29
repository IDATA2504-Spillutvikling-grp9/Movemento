using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Jump : StateMachineBehaviour
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
        //DiveAttack();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }



/* void DiveAttack()
{
    // Check if the dive attack condition is true for the BossDragonKnight instance
    if (BossDragonKnight.Instance.diveAttack)
    {
        // Flip the BossDragonKnight instance's orientation
        BossDragonKnight.Instance.Flip();

        // Calculate the new position for the boss by moving towards a target position
        Vector2 _newPos = Vector2.MoveTowards(rb.position, BossDragonKnight.Instance.moveToPosition,
        BossDragonKnight.Instance.speed * 3 * Time.fixedDeltaTime);
        // Move the boss to the new position
        rb.MovePosition(_newPos);

        // Calculate the distance between the boss's current position and the new pos
        float _distance = Vector2.Distance(rb.position, _newPos);
        // Check if the boss is close enough to the target position
        if (_distance < 0.1f)
        {
            // Execute the dive action
            BossDragonKnight.Instance.Dive();
        }
    }
} */



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
