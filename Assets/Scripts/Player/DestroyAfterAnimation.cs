using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This classed is used to destroy a gameOject after a certain amount of time.
public class DestroyAfterAnimation : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
