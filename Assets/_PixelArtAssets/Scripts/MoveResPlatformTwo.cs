using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveResPlatformTwo : MonoBehaviour
{
    public GameObject platform;  
    [Range(0.0f, 20.0f)]  
    public float moveSpeed = 3.0f;  
    public Transform currentPoint;  
    public Transform[] points;  
    public int pointSelection; 
    public Rigidbody2D rb;  

    public string collideTag = "Player";
    public Transform startPos;
    public float ResMoveSpeed = 8f;

    public bool moveSwitch;


    void Awake()
    {

        currentPoint = points[pointSelection];
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        moveSwitch = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (moveSwitch == true)
        {
            MoveObj();
        }

        if (moveSwitch == false)
        {

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, startPos.position, Time.deltaTime * ResMoveSpeed);
            startPos = currentPoint;
            currentPoint = points[pointSelection];

            pointSelection = 0;


        }

    }

    void MoveObj()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
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
