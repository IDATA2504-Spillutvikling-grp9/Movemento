using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallFrog : Enemy
{
 
 
 
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
    }
}
