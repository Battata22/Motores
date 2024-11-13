using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSaver
{
    private Transform myOwner;

    public LifeSaver(Transform newOwner)
    {
        myOwner = newOwner;
    }

    public void FakeUpdate()
    {
        if (myOwner.position.y <= -10f)
        {
            myOwner.position = new Vector3(0, 0, 0);
        }
    }
}
