using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    public GameObject checkPointIcon;
    public GameObject checkPointEffect;

     
    public string collideTag = "Player";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == collideTag)
        {
           
            checkPointEffect.SetActive(true);
            checkPointIcon.SetActive(true);

        }
    }
}
