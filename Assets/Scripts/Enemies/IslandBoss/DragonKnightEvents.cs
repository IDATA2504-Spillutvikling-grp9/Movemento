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
            BossDragonKnight.Instance.SlashAngle();
        }
        // Check if the player is above this object
        else if (PlayerController.Instance.transform.position.y > transform.position.y)
        {
            // Call Hit method for an upward attack, passing the transform and area for the upward attack
            Hit(BossDragonKnight.Instance.UpAttackTransform, BossDragonKnight.Instance.UpAttackArea);
            BossDragonKnight.Instance.SlashAngle();
        }
        // Check if the player is below this object
        else if (PlayerController.Instance.transform.position.y < transform.position.y)
        {
            // Call Hit method for a downward attack, passing the transform and area for the downward attack
            Hit(BossDragonKnight.Instance.DownAttackTransform, BossDragonKnight.Instance.DownAttackArea);
            BossDragonKnight.Instance.SlashAngle();
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        // Check for all collisions in a specified area and shape (box)
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0);

        // Iterate through all colliders detected
        foreach (Collider2D collider in objectsToHit)
        {
            // Check if the collider has the tag "Player"
            if (collider.CompareTag("Player"))
            {
                // Access the PlayerDamageController component and call TakeDamage with the boss's damage value
                collider.GetComponent<PlayerDamageController>().TakeDamage(BossDragonKnight.Instance.damage);
                Debug.Log("Player hit and damaged");

                // Break out of the loop after damaging the player
                break;
            }
        }

        // Log for debugging - this will log the number of colliders found in the hitbox area
        //Debug.Log("Hitboxes collided: " + objectsToHit.Length);
    }


    void Parrying()
    {
        BossDragonKnight.Instance.parrying = true;
    }


    void DestroyAfterDeath()
    {
        BossDragonKnight.Instance.DestroyAfterDeath();
    }
}
