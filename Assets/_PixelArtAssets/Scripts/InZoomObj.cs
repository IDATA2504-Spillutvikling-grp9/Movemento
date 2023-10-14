using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InZoomObj : MonoBehaviour
{
    private DollyZoom dolly;

    // Start is called before the first frame update
    void Start()
    {
        dolly = FindObjectOfType<DollyZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "DollyZoom")
        {
            dolly.inDollyZoomZone = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "DollyZoom")
        {
            dolly.inDollyZoomZone = false;
        }
    }


}
