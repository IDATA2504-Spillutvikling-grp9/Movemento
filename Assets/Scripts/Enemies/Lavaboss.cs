using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavaboss : Enemy
{
    private PlayerDamageController playerDamageController;
    public GameObject bullet;
    public Transform bulletPos;
    private float timer;
    public float radius = 5.0f; // The radius of the circle
    public float speed = 1.0f; // The speed of the movement
    private GameObject player;
    private float _angle; // The current angle
    private Vector2 _startPosition; // The starting position of the boss

    protected override void Start()
    {
        // Save the starting position
        _startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
    }

    void Update()
    {
        // Update the angle based on the speed and the time
        _angle += speed * Time.deltaTime;

        // Calculate the offset for the circular movement
        Vector2 offset = new Vector2(
            Mathf.Cos(_angle) * radius,
            Mathf.Sin(_angle) * radius
        );

        // Set the new position as an offset from the start position
        transform.position = _startPosition + offset;


        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 35)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                timer = 0;
                shoot();
            }
        }

        if(health <= 0) {
            base.Death(2f);
        }
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        // Boss-specific hit logic
       // if (health <= 0)
        //{
         //   Destroy(gameObject);
       // }
    }

    private IEnumerator FlashSprite()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

}
