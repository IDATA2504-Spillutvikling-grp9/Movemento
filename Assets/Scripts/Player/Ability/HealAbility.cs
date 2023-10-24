using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] private float timeToHeal;
    [SerializeField] GameObject healingVFX;
    private PlayerController playerController;
    private float healTimer;
    private GameObject _healingVFX; // Store a reference to the spawned healing VFX

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
        // Check if the healing button is pressed and if health is less than maxHealth
        if (Input.GetButton("Healing") && playerController.Health < playerController.maxHealth)
        {
            if (_healingVFX == null) // If the VFX is not already spawned, spawn it
            {
                playerController.pState.healing = true;
                playerController.anim.SetBool("Healing", true);
                _healingVFX = Instantiate(healingVFX, new Vector2(transform.position.x, transform.position.y - 1.2f), Quaternion.identity);
                Destroy(_healingVFX, timeToHeal);
            }

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
            if (_healingVFX != null) // If the VFX is spawned, destroy it
            {
                Destroy(_healingVFX);
            }

            playerController.pState.healing = false;
            playerController.anim.SetBool("Healing", false);
            healTimer = 0;
        }
    }
}
