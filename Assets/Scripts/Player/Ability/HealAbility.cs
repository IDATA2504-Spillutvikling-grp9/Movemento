using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] private float timeToHeal;
    [SerializeField] GameObject healingVFX;
    private PlayerController playerController;
    private float healTimer;
    private bool hasSpawnedVFX;

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
            if (!hasSpawnedVFX)
            {
                playerController.pState.healing = true;
                playerController.anim.SetBool("Healing", true);
                GameObject _healingVFX = Instantiate(healingVFX, new Vector2(transform.position.x, transform.position.y - 1.2f), Quaternion.identity);
                Destroy(_healingVFX, timeToHeal);
                hasSpawnedVFX = true;
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
            if (hasSpawnedVFX)
            {
                playerController.pState.healing = false;
                playerController.anim.SetBool("Healing", false);
                hasSpawnedVFX = false;
            }
            
            healTimer = 0;
        }
    }
}