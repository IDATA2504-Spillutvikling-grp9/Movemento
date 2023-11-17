using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpawnerType
{
	left, right, up, down
}

public class SawSpawner : MonoBehaviour {

	public SpawnerType spawnerType;
	public float time;               
	public float speed;              


	// Use this for initialization
	void Start () {

		Spawn();
		StartCoroutine(TimeBetweenSpawn());    
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator TimeBetweenSpawn()
	{
		yield return new WaitForSeconds(time);
		Spawn();

		StartCoroutine(TimeBetweenSpawn());
	}

 
	void Spawn()
	{
		switch (spawnerType)
		{
		case SpawnerType.left:
			 
			GameObject sawleft = ObjectPooling.instance.GetSpawnedSaw();
			 
			sawleft.GetComponent<Movement>().currentValue = transform.position.x;
			 
			sawleft.GetComponent<Movement>().movement = MovementType.onlyLeft;
		 
			sawleft.transform.position = transform.position;
		 
			sawleft.GetComponent<Movement>().speed = speed;
		 
			sawleft.SetActive(true);
			break;

		case SpawnerType.right:
			GameObject sawright = ObjectPooling.instance.GetSpawnedSaw();
			sawright.GetComponent<Movement>().currentValue = transform.position.x;
			sawright.GetComponent<Movement>().movement = MovementType.onlyRight;
			sawright.transform.position = transform.position;
			sawright.GetComponent<Movement>().speed = speed;
			sawright.SetActive(true);
			break;

		case SpawnerType.down:
			GameObject sawdown = ObjectPooling.instance.GetSpawnedSaw();
			sawdown.GetComponent<Movement>().currentValue = transform.position.y;
			sawdown.GetComponent<Movement>().movement = MovementType.onlyDown;
			sawdown.transform.position = transform.position;
			sawdown.GetComponent<Movement>().speed = speed;
			sawdown.SetActive(true);
			break;

		case SpawnerType.up:
			GameObject sawup = ObjectPooling.instance.GetSpawnedSaw();
			sawup.GetComponent<Movement>().currentValue = transform.position.y;
			sawup.GetComponent<Movement>().movement = MovementType.onlyUp;
			sawup.transform.position = transform.position;
			sawup.GetComponent<Movement>().speed = speed;
			sawup.SetActive(true);
			break;
		}
	}
}
