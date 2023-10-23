using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility_Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            //Enable the HealAbility Script on the player object
            HealAbility healAbility = collision.GetComponent<HealAbility>();
            if (healAbility != null)
            {
                healAbility.enabled = true;
            }

            //Destroy object after pick up.
            Destroy(gameObject);
        }
    }

}
