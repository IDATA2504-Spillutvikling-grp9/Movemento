using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonKnightEvents : MonoBehaviour
{


    void SlashDamagePlayer()
    {
        // Check if the player's x position is not equal to the object's x position (indicating they are not vertically aligned)
        if (PlayerController.Instance.transform.position.x - transform.position.x != 0)
        {
            // Call Hit method for a side attack, passing the transform and area for the side attack
            Hit(BossDragonKnight.Instance.SideAttackTransform, BossDragonKnight.Instance.SideAttackArea);
        }
        // Check if the player is above this object
        else if (PlayerController.Instance.transform.position.y > transform.position.y)
        {
            // Call Hit method for an upward attack, passing the transform and area for the upward attack
            Hit(BossDragonKnight.Instance.UpAttackTransform, BossDragonKnight.Instance.UpAttackArea);
        }
        // Check if the player is below this object
        else if (PlayerController.Instance.transform.position.y < transform.position.y)
        {
            // Call Hit method for a downward attack, passing the transform and area for the downward attack
            Hit(BossDragonKnight.Instance.DownAttackTransform, BossDragonKnight.Instance.DownAttackArea);
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        // Check for a collision in a specified area and shape (box)
        Collider2D _objectsToHit = Physics2D.OverlapBox(_attackTransform.position, _attackArea, 0);

        Debug.Log("Hit method in");
        // If the collided object is the player, apply damage to them
        if (_objectsToHit.GetComponent<PlayerController>() != null)
        {
        Debug.Log("Conditional correct");
            // Access the PlayerDamageController component and call TakeDamage with the boss's damage value
            _objectsToHit.GetComponent<PlayerDamageController>().TakeDamage(BossDragonKnight.Instance.damage);
            Debug.Log("Taken dmg");
        }
    }


    
    void Parrying()
    {
        BossDragonKnight.Instance.parrying = true;
    }
}
