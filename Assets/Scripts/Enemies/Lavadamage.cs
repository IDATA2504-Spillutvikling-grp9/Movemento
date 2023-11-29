using UnityEngine;

public class Lavadamage : MonoBehaviour
{
    public float damageInterval = 2.0f; // The interval in seconds between each damage
    public float damageAmount = 1.0f;   // The amount of damage to deal

    private float damageTimer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (damageTimer <= 0f)
            {
                // Damage the player
                other.GetComponent<PlayerDamageController>().TakeDamage(damageAmount);

                // Reset the timer
                damageTimer = damageInterval;
            }
        }
    }

    private void Update()
    {
        // If the timer is above 0, decrease it over time
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }
}
