using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;
using UnityEngine.UI;

public class Player : BaseCharacter
{
    [SerializeField] GameObject _mesh;
    [SerializeField] float _kickDist;
    [SerializeField] float _pickUpRange;

    [SerializeField] int money;
    public bool blueTeam;

    public delegate void VoidDelegateFloat2(float a, float b);
    public VoidDelegateFloat2 _playerMovemente;

    PlayerControl myControl;

    //Canvas
    [SerializeField] public Image _healthBar, _staminaBar;

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

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.Player = this;
        GameManager.Instance.OnShopActive += EnterShop;
    }

    private void Update()
    {
        myControl.FakeUpdate();
        _myStaminaControl.FakeUpdate(ref _stamina);
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

    private void LateUpdate()
    {
        UpdateHpUI();
        UpdateStaminaUI();
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

        _myStaminaControl.DecreseStamina(ref _stamina, _runningStmCost * Time.deltaTime);
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
        if (inCombat) return;
        if (outOfBreath) return;

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
        ResetAttackSpeed();

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

    public void TryInteract()
    {
        var pos = transform.position + new Vector3(0, 1, 0);
        //pos = transform.position + new Vector3(0, 1, 0);
        if (Physics.Raycast(pos, transform.forward, out RaycastHit hit, _kickDist) && hit.transform.TryGetComponent<IInteractable>(out IInteractable target))
        {
            target.Interact();
        }
    }

    public void UpdateHpUI()
    {
        _healthBar.fillAmount = _hp / _maxHp;
    }
    public void UpdateStaminaUI()
    {
        _staminaBar.fillAmount = _stamina / _maxStamina;
        if(outOfBreath)
            _staminaBar.color = Color.yellow;
        else
            _staminaBar.color = Color.green;
    }

    public void DoBlock()
    {
        _myBlock();
    }

    public float GiveAttackStats(out float _atkSpd, out float _stm, out float _atkStmCost, out bool _outOfBreath)
    {
        _atkSpd = _currentAtkSpd;
        _stm = _stamina;
        _atkStmCost = _staminaCost;
        _outOfBreath = outOfBreath;
        return _damage;
    }

    void EnterShop()
    {
        GameManager.Instance.Shop.GetPlayerInfo(ref money, this, ref _currentWeapon);
    }
    /// <summary>
    /// If Can Pay return True
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool PayAmount(int amount)
    {
        if(amount > money)
        {
            return false;
        }
        else
        {
            money -= amount;
            return true;
        }
    }

    
}
