using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController: MonoBehaviour
{
    public float speed = 0.1F;
    public Renderer rend;
   
    void Start()
    {
        rend = GetComponent<Renderer>();
    }


     
    void Update()
    {
        float x = Mathf.Repeat(Time.time * speed, 1);
        rend.material.mainTextureOffset = new Vector2(-x, 0);
    }
 
}
