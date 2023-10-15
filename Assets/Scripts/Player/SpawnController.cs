using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
	private Vector3 respawnPoint;
	public GameObject fallDetector;
	public string sceneName;
	public GameObject endLevelScreen;

    void Start()
    {
        respawnPoint = transform.position;
		endLevelScreen.SetActive(false);
    }

    void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "FallDetector") {
			transform.position = respawnPoint;
		}
		else if(collision.tag == "CheckPoint") {
			 respawnPoint = transform.position;
		}
		else if(collision.tag == "NextLevelPoint") {
			PauseGame();
		}
	}

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (endLevelScreen != null)
        {
            endLevelScreen.SetActive(true);
        }
    }
}
