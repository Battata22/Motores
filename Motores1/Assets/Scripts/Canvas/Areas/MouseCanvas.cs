using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCanvas : MonoBehaviour
{

    private void Awake()
    {
        //GameManager.Instance.mouseCenterGO = gameObject;
    }


    void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
    }

}
