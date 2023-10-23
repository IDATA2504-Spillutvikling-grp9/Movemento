using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] private float timeToHeal;
    private float healTimer;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        HealAbility healAbility = GetComponent<HealAbility>();
        if (healAbility != null)
        {
            healAbility.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButton("Healing") && playerController.Health < playerController.maxHealth && !playerController.pState.jumping && !playerController.pState.dashing)
        {
            playerController.pState.healing = true;

            // Healing
            healTimer += Time.deltaTime;

            if (healTimer >= timeToHeal)
            {
                playerController.Health++;
                healTimer = 0;
            }
        }
        else
        {
            playerController.pState.healing = false;
            healTimer = 0;
        }
    }
}
