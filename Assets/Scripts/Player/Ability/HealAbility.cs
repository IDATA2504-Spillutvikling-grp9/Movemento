using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] private float timeToHeal;
    [SerializeField] GameObject healingVFX;
    private GameObject _healingVFX; // Store a reference to the spawned healing VFX
    private PlayerController pc;
    private PlayerHealth ph;
    private float healTimer;



    private void Start()
    {
        pc = GetComponent<PlayerController>();
        ph = GetComponent<PlayerHealth>();

        if (TryGetComponent<HealAbility>(out var healAbility))
        {
            healAbility.enabled = false;
        }
    }



    private void Update()
    {
        // Check if the healing button is pressed and if health is less than maxHealth
        if (Input.GetButton("Healing") && ph.Health < ph.maxHealth)
        {
            if (_healingVFX == null) // If the VFX is not already spawned, spawn it
            {
                pc.pState.healing = true;
                pc.anim.SetBool("Healing", true);
                _healingVFX = Instantiate(healingVFX, new Vector2(transform.position.x, transform.position.y - 1.2f), Quaternion.identity);
                Destroy(_healingVFX, timeToHeal);
            }

            // Healing
            healTimer += Time.deltaTime;

            if (healTimer >= timeToHeal)
            {
                ph.Health++;
                healTimer = 0;
            }
        }
        else
        {
            if (_healingVFX != null) // If the VFX is spawned, destroy it
            {
                Destroy(_healingVFX);
            }

            pc.pState.healing = false;
            pc.anim.SetBool("Healing", false);
            healTimer = 0;
        }
    }
}
