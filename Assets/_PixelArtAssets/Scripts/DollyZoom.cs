using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyZoom : MonoBehaviour
{
    public bool inDollyZoomZone;  
    public bool dollyZoom;  
    [Range(0.0f, 20.0f)] 
    public float dollySmoothSpeed = 5f;  
    [Range(0.0f, 20.0f)] 
    public float dollySpeed = 10f;  
    public float targetOrtho;  
    public float minOrtho; 
    public float maxOrtho; 
    public Camera controlCamera;

   // Start is called before the first frame update
    void Awake()
    {

        targetOrtho = controlCamera.orthographicSize; 
    }

    // Update is called once per frame
    void Update()
    {

        if (inDollyZoomZone == true)
        {
            dollyZoom = true;
            targetOrtho = maxOrtho;  
            controlCamera.orthographicSize = Mathf.SmoothStep(controlCamera.orthographicSize, targetOrtho, dollySmoothSpeed * Time.deltaTime);  
            targetOrtho = Mathf.MoveTowards(targetOrtho, minOrtho, maxOrtho);

        }
         
        if (inDollyZoomZone == false)
        {
 
            controlCamera.orthographicSize = Mathf.MoveTowards(controlCamera.orthographicSize, targetOrtho, dollySpeed * Time.deltaTime);
            targetOrtho = Mathf.MoveTowards(targetOrtho, minOrtho, maxOrtho);

        }

        if (inDollyZoomZone == false && controlCamera.orthographicSize == minOrtho)
        {

            dollyZoom = false;
        }
    }
}
