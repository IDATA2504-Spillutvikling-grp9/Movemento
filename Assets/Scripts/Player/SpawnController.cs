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

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        respawnPoint = transform.position;
        firstSpawn = respawnPoint;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "FallDetector") {
			player.transform.position = respawnPoint;
		}
		else if(collision.tag == "CheckPoint") {
			 respawnPoint = transform.position;
		}
        else if(collision.tag == "NextLevelPoint") {
            gameManager.EndLevel();
        }
	}

    public bool dieRespawn() {
        player.transform.position = firstSpawn;
        return true;
    }
}
