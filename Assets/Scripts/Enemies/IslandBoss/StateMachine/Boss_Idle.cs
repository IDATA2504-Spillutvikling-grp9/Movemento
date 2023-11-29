using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Idle : StateMachineBehaviour
{
    [SerializeField] Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponentInParent<Rigidbody2D>();
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Reset the GameObject's velocity to zero to stop any ongoing movement
        rb.velocity = Vector2.zero;

        // Call the RunToPlayer function to potentially move towards the player
        RunToPlayer(animator);


        // Check if the boss is ready to attack (based on a countdown timer reaching zero)
        if(BossDragonKnight.Instance.attackCountDown <= 0)
        {
            // Trigger the boss's attack and reset the attack countdown timer
            BossDragonKnight.Instance.AttackHandler();
            BossDragonKnight.Instance.attackCountDown = Random.Range(BossDragonKnight.Instance.attackTimer - 1, BossDragonKnight.Instance.attackTimer + 1);
        }
    }

    void RunToPlayer(Animator animator)
    {
        // Calculate the distance to the player
        if(Vector2.Distance(PlayerController.Instance.transform.position, rb.position) >= BossDragonKnight.Instance.attackRange)
        {
            // If the boss is outside its attack range, set the animator to trigger a running state
            animator.SetBool("Run", true);
        }
        else
        {
            // If not, return
            return;
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }











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
