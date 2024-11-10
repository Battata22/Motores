using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class Player : BaseCharacter
{
    [SerializeField] GameObject _mesh;

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
    }

    public override void Movement(float xAxis, float zAxis)
    {
        //base.Movement();
        if (!_canMove) return;
        var dir = (transform.right * xAxis) + (transform.forward * zAxis).normalized;
        _rb.position += dir * _speed * Time.deltaTime;
    }

    public void UsePotion()
    {
        Debug.Log($"<color=green> Player Use Potion </color>");
    }

    public override void Kick()
    {
        var pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        if (Physics.Raycast(pos, transform.forward, out RaycastHit hit) && hit.transform.TryGetComponent<IKickable>(out IKickable target))
        {
            target.GetKicked();

        }
        base.Kick();
    }

    protected override void EnterCombat()
    {
        _mesh.SetActive(false);

        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;
    }

    protected override void ExitCombat()
    {
        _mesh.SetActive(true);

        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;
    }
}
