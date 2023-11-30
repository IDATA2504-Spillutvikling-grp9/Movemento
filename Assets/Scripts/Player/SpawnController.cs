using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
	private Vector3 respawnPoint; // The position where the player will respawn after falling or hitting spikes.
	public GameObject fallDetector; // The fall detector object that follows the player to detect falls.
    private GameManager gameManager; // Reference to the GameManager script.
    private GameObject player; // Reference to the player GameObject.
    private Vector3 firstSpawn; // The initial spawn point when the level starts.
    private PlayerDamageController playerDamageController; // Reference to the PlayerDamageController script.
    private PlayerHealth playerHealth;


    /// <summary>
    /// Called on the frame when a script is enabled. Used for initialization.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        respawnPoint = transform.position;
        firstSpawn = respawnPoint;
        player = GameObject.FindGameObjectWithTag("Player");
        playerDamageController = GetComponent<PlayerDamageController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Called once per frame. Updates the fall detector's x-position to follow the player.
    /// </summary>
    void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    /// <summary>
    /// Called when a Collider2D enters the trigger zone of this GameObject.
    /// Handles collisions with different objects based on their tags.
    /// </summary>
    /// <param name="collision">The Collider2D that enters the trigger zone.</param>
	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "FallDetector") {
            playerDamageController.TakeDamage(1f);
            if(playerHealth.getHealth() == 0) {
                DelayedPlayerDeathRespawn(1f);
            }
            else {
                player.transform.position = respawnPoint;
            }
            
		}
		else if(collision.tag == "CheckPoint") {
            respawnPoint = transform.position;
		}
        else if(collision.tag == "Spikes") {
            playerDamageController.TakeDamage(1f);
            if(playerHealth.getHealth() == 0) {
                StartCoroutine(DelayedPlayerDeathRespawn(1f));
            }
            else {
                StartCoroutine(DelayedPlayerRespawn(1f)); 
            }
        }
        else if(collision.tag == "ObjectDamage") {
            playerDamageController.TakeDamage(1f);
        }
        else if(collision.tag == "NextLevelPoint") {
            gameManager.EndLevel();
        }
        else if(collision.tag == "BossCheckPoint") {
            firstSpawn = transform.position;
        }
	}

    /// <summary>
    /// Coroutine that delays player respawn by the specified time.
    /// </summary>
    /// <param name="delayTime">The time to delay player respawn.</param>
    /// <returns></returns>
     IEnumerator DelayedPlayerRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.transform.position = respawnPoint;
    }

    IEnumerator DelayedPlayerDeathRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.transform.position = firstSpawn;
    }

    /// <summary>
    /// Respawns the player at the initial spawn point.
    /// </summary>
    public void dieRespawn() {
        respawnPoint = firstSpawn;
        player.transform.position = firstSpawn;
    }
}