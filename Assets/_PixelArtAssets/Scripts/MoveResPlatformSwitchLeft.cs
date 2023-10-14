using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveResPlatformSwitchLeft : MonoBehaviour
{
    private MoveResPlatformTwo moveResPlatformTwo;
    // Use this for initialization
    void Awake()
    {
        moveResPlatformTwo = FindObjectOfType<MoveResPlatformTwo>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {

            moveResPlatformTwo.moveSwitch = true;
            gameObject.SetActive(false);

        }
    }
}
