using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AttackCenter : MonoBehaviour
{
    [SerializeField] float rotVel;
    float rotation = 0;
    bool dentro = false;
    //float normalAttackSpeed;

    private void Start()
    {
        //normalAttackSpeed = GameManager.Instance.enemyInCombat._attackSpeed;
    }

    void Update()
    {
        rotation += Time.deltaTime;
        if (rotation >= 360) rotation = 0;
        transform.rotation = Quaternion.Euler(0, 0, rotation * rotVel);

        if (dentro && Input.GetMouseButtonDown(1))
        {

            gameObject.SetActive(false);

            

            //GameManager.Instance.enemyInCombat._attackSpeed = GameManager.Instance.enemyInCombat._attackSpeed - 0.1f;
            //restar tiempo de attack speed del enemigo

            print("le diste al centro");

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

    private void OnDisable()
    {
        dentro = false;
    }
}
