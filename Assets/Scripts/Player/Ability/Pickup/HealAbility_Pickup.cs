using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility_Pickup : MonoBehaviour
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            //Enable the HealAbility Script on the player object
            if (collision.TryGetComponent<HealAbility>(out var healAbility))
            {
                healAbility.enabled = true;
            }

            //Destroy object after pick up.
            Destroy(gameObject);
        }
    }

}
