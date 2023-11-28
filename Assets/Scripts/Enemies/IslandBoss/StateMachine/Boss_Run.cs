using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
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
        // Call the TargetPlayerPosition function to handle boss movement and orientation
        TargetPlayerPosition(animator);

        // Check if the boss is ready to attack (based on a countdown timer reaching zero)
        if(BossDragonKnight.Instance.attackCountDown <= 0)
        {
            // Trigger the boss's attack and reset the attack countdown timer
            BossDragonKnight.Instance.AttackHandler();
            BossDragonKnight.Instance.attackCountDown = Random.Range(BossDragonKnight.Instance.attackTimer - 1, BossDragonKnight.Instance.attackTimer + 1);
        }
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // When leaving the state, set the 'Run' boolean in the animator to false
        animator.SetBool("Run", false);
    }


    // Custom method to handle logic for orienting and moving the boss towards the player's position
    void TargetPlayerPosition(Animator animator)
    {
        // Check if the boss is grounded
        if(BossDragonKnight.Instance.Grounded())
        {
            // Correct the boss's orientation to face the player
            BossDragonKnight.Instance.Flip();

            // Calculate the target position with the player's X position and boss's Y position
            Vector2 _target = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y);

            // Calculate the new position to move towards the player at the boss's run speed
            Vector2 _newPos = Vector2.MoveTowards(rb.position, _target, BossDragonKnight.Instance.runSpeed * Time.fixedDeltaTime);
            //Debug.Log("Falling");

            // Set the boss's running speed
            //BossDragonKnight.Instance.runSpeed = BossDragonKnight.Instance.speed;
            //Debug.Log("Set run speed");

            // Move the Rigidbody to the new position
            rb.MovePosition(_newPos);
            //Debug.Log("new_pos");
        }
        else
        {
            // If not grounded, apply a downward velocity to simulate falling
            rb.velocity = new Vector2(rb.velocity.x, -25);
        }


        // Check if the boss is within attack range of the player
        if
        (Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= BossDragonKnight.Instance.attackRange)
        {
            // If within attack range, stop running
            animator.SetBool("Run", false);
        }
        else
        {
            return;
        }
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
