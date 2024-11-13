using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class Player : BaseCharacter
{
    [SerializeField] GameObject _mesh;
    [SerializeField] float _kickDist;
    [SerializeField] float _pickUpRange;

    public int money;
    public bool blueTeam;

    public delegate void VoidDelegateFloat2(float a, float b);
    public VoidDelegateFloat2 _playerMovemente;

    PlayerControl myControl;

    //Dev
    [Header("<color=green> Developer test </color>")]
    [SerializeField] float dev_damagePlayer;
    [SerializeField] AttackDirectionList dev_attackDirection;
    [SerializeField] ArmorPice.ArmorType[] dev_armorEquiped;
    [SerializeField] ArmorPice.ArmorQuality[] dev_armorQuality;

    protected override void Awake()
    {
        base.Awake();
        _myStaminaControl.OnOutOfBreath += RunningState;
        _playerMovemente = Movement;
        myControl = new PlayerControl(this);
    }

    private void Update()
    {
        myControl.FakeUpdate();
        _myStaminaControl.FakeUpdate(ref stamina);
        _myLifeSaver.FakeUpdate();

        CallDOTs(ref _hp);
        

        #region Dev Inputs
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDamage(dev_damagePlayer, dev_attackDirection);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            for (int i = 0; i < dev_armorEquiped.Length; i++)
            {
                SetArmor(dev_armorEquiped[i], dev_armorQuality[i]);
            }
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log($"<color=#e88ced> Vida al maximo <3 </color>");
            _hp = _maxHp;
        }
        if(Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log($"<color=green>Veneno Activado </color>");
            StartPoison(5f);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log($"<color=red>Sangrado Activado </color>");
            StartBleed(10f);
        }
        if(Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("Frenado Bleed y Posion");
            StopBleed();
            StopPoison();
        }
        #endregion

    }

    protected void Movement(float xAxis, float zAxis)
    {
        //base.Movement();
        if (!_canMove) return;
        var dir = (transform.right * xAxis) + (transform.forward * zAxis).normalized;
        _rb.position += dir * _speed * Time.deltaTime;
    }

    protected void Run(float xAxis, float zAxis)
    {
        if (!_canMove) return;
        var dir = (transform.right * xAxis) + (transform.forward * zAxis).normalized;
        _rb.position += dir * _runningSpeed * Time.deltaTime;

        _myStaminaControl.DecreseStamina(ref stamina, _runningStmCost * Time.deltaTime);
    }

    public override void UsePotion()
    {
        if(potions <= 0)
        {
            //Llamado a accion cuando no tenes pociones
            Debug.Log("<color=red> No te quedan pociones </color>");
            potions = 0;
            return;
        }
        if(_hp >= _maxHp)
        {
            //Llamado a accion cuando tenes vida llena
            Debug.Log("<color=yellow> Vida llena, no se uso pocion </color>");
            return;
        }
        Debug.Log($"<color=green> Player Use Potion </color>");
        _myHealthSystem.CalcHeal(ref _hp ,GameManager.Instance.potionHealAmount);
        potions--;
    }

    public override void Kick()
    {
        var pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        if (Physics.Raycast(pos, transform.forward, out RaycastHit hit, _kickDist) && hit.transform.TryGetComponent<IKickable>(out IKickable target))
        {
            target.GetKicked();

        }
        base.Kick();
    }

    public void RunningState(bool coso)
    {
        running = coso;
        if (running)
            _playerMovemente = Run;
        else
            _playerMovemente = Movement;
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
        Collider[] objects = Physics.OverlapSphere(transform.position, _pickUpRange );
        foreach (Collider collider in objects)
        {
            if(collider.gameObject.TryGetComponent<IPickeable>(out IPickeable pickeable))
            {
                pickeable.PickUp(this);
            }
        }
    }
}
