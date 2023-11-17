using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public GameObject platform;  
	[Range(0.0f, 20.0f)]  
	public float moveSpeed = 3.0f;  
	public Transform currentPoint;  
	public Transform[] points;  
	public int pointSelection; 
	public Rigidbody2D rb;  
    


	void Awake () {

		currentPoint = points[pointSelection];
		rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {

        MoveObj();

	}

    void MoveObj()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        if (platform.transform.position == currentPoint.position)
        {
            pointSelection++;
            if (pointSelection == points.Length)
            {
                pointSelection = 0;
            }
            currentPoint = points[pointSelection];
        }
    }
}
