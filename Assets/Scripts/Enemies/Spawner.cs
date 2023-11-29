using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject spawn;
    [SerializeField] private float xCordinates;
    [SerializeField] private float yCordinates;
    [SerializeField] private float zCordinates;
    private GameObject spawnedObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            spawnedObject = Instantiate(spawn, new Vector3(xCordinates, yCordinates, zCordinates), Quaternion.identity);
        }
    }   


    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // Check if the spawned object exists and destroy it
            if (spawn != null)
            {
                Destroy(spawnedObject);
            }
        }
    }
}
