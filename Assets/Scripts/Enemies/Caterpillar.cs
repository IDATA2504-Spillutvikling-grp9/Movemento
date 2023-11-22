using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caterpillar : MonoBehaviour
{

    //Damage & health
    public int damage;
    public float damageDelay;
    private bool isCooldown = false;
    public int currentHealth;
    public int maxHealth;
    public PlayerDamageController playerDamageController;

    //Patroling
    public Transform posA, posB;
    public int speed;
    Vector2 targetPos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isCooldown)
        {
            playerDamageController.TakeDamage(damage);
            StartCoroutine(DamageCooldown());
        }
    }

    // Existing methods...

    IEnumerator DamageCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(damageDelay);
        isCooldown = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        targetPos = posB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < .1f)
        {
            targetPos = posB.position;
            FlipCaterpillar(targetPos);
        }

        if (Vector2.Distance(transform.position, posB.position) < .1f)
        {
            targetPos = posA.position;
            FlipCaterpillar(targetPos);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    void FlipCaterpillar(Vector2 targetPosition)
    {
        bool shouldFaceRight = targetPosition.x < transform.position.x;

        Vector3 scale = transform.localScale;

        if ((shouldFaceRight && scale.x < 0) || (!shouldFaceRight && scale.x > 0))
        {
            scale.x = -scale.x;
        }
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to destroy the caterpillar
    void Die()
    {
        Destroy(gameObject);
    }
}
