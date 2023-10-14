using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class will be used later on for animations based on game events. 
// We can trigger events that sets the state of the player, eg jumping, hasDied, doubleJump etc.
// These events can trigger an animation when detected.
public class PlayerStateList : MonoBehaviour
{
    public bool jumping = false;
    public bool dashing = false;
}
