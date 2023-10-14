using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalMovement : MonoBehaviour
{
    public float rotSpeed;              
    private float angle;               

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        MoveRotate();

    }


    void MoveRotate()
    {
         
        angle = transform.rotation.eulerAngles.z;

        angle += rotSpeed * Time.deltaTime;
       
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
