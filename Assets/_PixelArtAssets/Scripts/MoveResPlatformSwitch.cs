using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveResPlatformSwitch : MonoBehaviour
{
    private MoveResPlatform moveResPlatform;
    // Use this for initialization
    void Awake()
    {
        moveResPlatform = FindObjectOfType<MoveResPlatform>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {

            moveResPlatform.moveSwitch = true;
            gameObject.SetActive(false);

        }
    }
}
