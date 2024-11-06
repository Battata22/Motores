using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class Player : BaseCharacter
{
    public int money;
    public bool blueTeam;

    PlayerControl myControl;

    protected override void Awake()
    {
        base.Awake();
        myControl = new PlayerControl(this);
    }

    private void Update()
    {
        myControl.FakeUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public override void Movement(float xAxis, float zAxis)
    {
        //base.Movement();
        var dir = (transform.right * xAxis) + (transform.forward * zAxis).normalized;
        _rb.position += dir * _speed * Time.deltaTime;
    }

    public void UsePotion()
    {

    }

    public void Kick()
    {
        Debug.Log("Saliendo de modo Pelea");
    }
}
