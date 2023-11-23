using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
	private Vector3 respawnPoint;
	public GameObject fallDetector;

    private GameManager gameManager;
    private GameObject player;

    private Vector3 firstSpawn;

    private PlayerDamageController playerDamageController;



    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        respawnPoint = transform.position;
        firstSpawn = respawnPoint;
        player = GameObject.FindGameObjectWithTag("Player");
        playerDamageController = GetComponent<PlayerDamageController>();
    }

    void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "FallDetector") {
			player.transform.position = respawnPoint;
            playerDamageController.TakeDamage(1f);
		}
		else if(collision.tag == "CheckPoint") {
			 respawnPoint = transform.position;
		}
        else if(collision.tag == "Spikes") {
            playerDamageController.TakeDamage(1f);
            StartCoroutine(DelayedPlayerRespawn(1f)); 
            
        }
        else if(collision.tag == "NextLevelPoint") {
            gameManager.EndLevel();
        }
	}

     IEnumerator DelayedPlayerRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.transform.position = respawnPoint;
    }

    public void dieRespawn() {
        player.transform.position = firstSpawn;
    }
}
