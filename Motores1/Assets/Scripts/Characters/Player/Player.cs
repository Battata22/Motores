using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class Player : BaseCharacter
{
    [SerializeField] GameObject _mesh;

    public int money;
    public bool blueTeam;

    public delegate void VoidDelegateFloat2(float a, float b);
    public VoidDelegateFloat2 _playerMovemente;

    PlayerControl myControl;

    protected override void Awake()
    {
        base.Awake();
        _playerMovemente = Movement;
        myControl = new PlayerControl(this);
    }

    private void Update()
    {
        myControl.FakeUpdate();
        _myStaminaControl.FakeUpdate(ref stamina);
    }

    protected void Movement(float xAxis, float zAxis)
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

    public void RunningState(bool coso)
    {
        running = coso;
    }

    protected override void EnterCombat()
    {
        _mesh.SetActive(false);
        _playerMovemente = delegate { };
        inCombat = true;

        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;
    }

    protected override void ExitCombat()
    {
        _mesh.SetActive(true);
        _playerMovemente = Movement;
        inCombat = false;

        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;
    }

    public void CheckPickUp()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, 0.25f);
        foreach (Collider collider in objects)
        {
            if(collider.gameObject.TryGetComponent<IPickeable>(out IPickeable pickeable))
            {
                pickeable.PickUp(this);
            }
        }
    }
}
