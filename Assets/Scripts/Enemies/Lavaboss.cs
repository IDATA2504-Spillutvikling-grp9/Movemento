using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavaboss : MonoBehaviour
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

    public int health = 20;

    void Start()
    {
        // Save the starting position
        _startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

}
