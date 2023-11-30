using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] private float timeToHeal;
    [SerializeField] GameObject healingVFX;
    private GameObject _healingVFX; // Store a reference to the spawned healing VFX
    private PlayerController pc;
    private PlayerMana pm;
    private PlayerHealth ph;
    private float healTimer;

    private GameManager gameManager;



    private void Start()
    {
        pc = GetComponent<PlayerController>();
        pm = GetComponent<PlayerMana>();
        ph = GetComponent<PlayerHealth>();
        gameManager = FindObjectOfType<GameManager>();

        if (TryGetComponent<HealAbility>(out var healAbility))
        {
            healAbility.enabled = false;
        }
    }



    private void Update()
    {
        // Check if the healing button is pressed and if health is less than maxHealth
         if (Input.GetButton("Healing") && ph.Health < ph.maxHealth && !pc.pState.jumping && !pc.pState.dashing && pm.Mana != 0) //pm.mana used to see if the char has enough mana to cast a full heal.
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
            //use mana while healing
            pm.Mana -= Time.deltaTime * pm.manaDrainSpeed;
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

        void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Healing") {
            gameManager.TurnOnAndOfHealingAbilityScreen();
        }
    }

}
