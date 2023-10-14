using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    public float delayStart;

    public float laserLoopIntenal;
    public float laserDelayCollider;
    public float laserDelayClose;

    public Transform laser;
    public Collider2D laserCollider;

    // Start is called before the first frame update
    void Start()
    {
        if(laser)
        {
            laser.gameObject.SetActive(false);
            StartCoroutine(colDelayStart(delayStart));
        }
    }

    private IEnumerator colDelayStart(float t)
    {
        yield return new WaitForSeconds(t);

        laser.gameObject.SetActive(true);

        StartCoroutine(colDelayClose(laserDelayClose));
        StartCoroutine(colDelayCollider(laserDelayCollider));

        StartCoroutine(colDelayStart(laserLoopIntenal));
    }


    private IEnumerator colDelayClose(float t)
    {
        yield return new WaitForSeconds(t);

        laser.gameObject.SetActive(false);
        if(laserCollider)
        {
            laserCollider.enabled = false;
        }
    }

    private IEnumerator colDelayCollider(float t)
    {
        yield return new WaitForSeconds(t);

        if (laserCollider)
        {
            laserCollider.enabled = true;
        }
    }
}
