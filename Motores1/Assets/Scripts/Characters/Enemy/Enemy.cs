using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : BaseCharacter
{
    public bool blueTeam;

    [SerializeField] protected BaseCharacter _target;
    [SerializeField] protected float _attackDist;

    //Combat CD
    [SerializeField] protected float _enterCombatCD;
    float _lastCombatTime = -1;

    private void FixedUpdate()
    {
        if (!_inCombat && (Time.time - _lastCombatTime > _enterCombatCD) && Vector3.SqrMagnitude(_target.transform.position- transform.position) <= (_attackDist * _attackDist))
        {
            GameManager.Instance.EnterCombat();
        }
        ChaseTarget();
    }

    protected override void EnterCombat()
    {
        Debug.Log($"{gameObject.name} Enter combat mode");
        _canMove = false;
        _inCombat = true;

        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;

        transform.LookAt(_target.transform);

    }

    protected override void ExitCombat()
    {
        Debug.Log($"{gameObject.name} Exit combat mode");
        _canMove = true;
        _inCombat = false;  

        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        _lastCombatTime = Time.time;    
    }

    protected void ChaseTarget()
    {
        if (!_canMove) return;
        var dir = _target.transform.position - transform.position;
        _rb.position += dir.normalized * _speed * Time.fixedDeltaTime;

        transform.LookAt(_target.transform);
    }
}
