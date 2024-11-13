using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCanvas : MonoBehaviour
{
    bool dentro = false;
    [SerializeField] GameObject centro;


    void Update()
    {
        if (dentro && Input.GetMouseButtonDown(1))
        {

            gameObject.SetActive(false);
            centro.SetActive(false);

            print("le diste al cuadrado");

        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dentro = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dentro = false;
    }

}
