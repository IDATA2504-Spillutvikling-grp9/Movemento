using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonKnightEvents : MonoBehaviour
{
    void SlashDamagePlayer()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x ||
            PlayerController.Instance.transform.position.x < transform.position.x)
        {
            Hit(BossDragonKnight.Instance.SideAttackTransform, BossDragonKnight.Instance.SideAttackArea);
        }
        else if (PlayerController.Instance.transform.position.y > transform.position.y)
        {
            Hit(BossDragonKnight.Instance.UpAttackTransform, BossDragonKnight.Instance.UpAttackArea);
        }
        else if (PlayerController.Instance.transform.position.y < transform.position.y)
        {
            Hit(BossDragonKnight.Instance.DownAttackTransform, BossDragonKnight.Instance.DownAttackArea);
        }
    }
    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D _objectsToHit = Physics2D.OverlapBox(_attackTransform.position, _attackArea, 0);

        if (_objectsToHit.GetComponent<PlayerController>() != null)
        {
            _objectsToHit.GetComponent<PlayerDamageController>().TakeDamage(BossDragonKnight.Instance.damage);
        }
    }
}
