using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform component
    public float followSpeed = 5.0f; // Adjust the follow speed as needed

    private void Update()
    {
        if (player != null) // Check if the player Transform reference is set
        {
            // Get the current position of the GameObject and the player
            Vector2 currentPosition = transform.position;
            Vector2 playerPosition = player.position;

            // Keep the Y position the same, only update the X position
            currentPosition.x = Mathf.Lerp(currentPosition.x, playerPosition.x, followSpeed * Time.deltaTime);

            // Update the position of the GameObject
            transform.position = currentPosition;
        }
    }
}
