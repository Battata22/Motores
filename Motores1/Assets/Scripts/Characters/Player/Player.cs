using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player : BaseCharacter
{
    public Weapon.WeaponType playerWeapon { get { return _currentWeapon; } protected set { } }

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

    //[Header("<color=red> DOTs")]
    //[SerializeField, Range(0, 100)] int _bleedChance;
    //[SerializeField] float _bleedDuration;

    //Dev
    [Header("<color=green> Developer test </color>")]
    [SerializeField] float dev_damagePlayer;
    [SerializeField] AttackDirectionList dev_attackDirection;
    [SerializeField] Armor.Type[] dev_armorEquiped;
    [SerializeField] Armor.Quality[] dev_armorQuality;
    bool _inmortal = false;
    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey("potions"))
            AddPotion(PlayerPrefs.GetInt("potions"));
        if (PlayerPrefs.HasKey("chestQuality"))
            SetArmor(Armor.Type.Chestplate, (Armor.Quality)PlayerPrefs.GetInt("chestQuality"));
        if (PlayerPrefs.HasKey("helmetQuality"))
            SetArmor(Armor.Type.Chestplate, (Armor.Quality)PlayerPrefs.GetInt("helmetQuality"));
        if (PlayerPrefs.HasKey("legsQuality"))
            SetArmor(Armor.Type.Chestplate, (Armor.Quality)PlayerPrefs.GetInt("legsQuality"));
        if (PlayerPrefs.HasKey("money"))
            AddMoney(PlayerPrefs.GetInt("money"));
        
        _myStaminaControl.OnOutOfBreath += RunningState;
        _playerMovemente = Movement;
        myControl = new PlayerControl(this);
        //_myChargeAttack += ChanceOfBleed;
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.Player = this;
        //GameManager.Instance.OnShopActive += EnterShop;
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
            //TakeDamage(dev_damagePlayer, dev_attackDirection, false);
            _inmortal = !_inmortal;
            Debug.Log($"<color=cyan> Inmortal = {_inmortal} </color>");
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
        //if(Input.GetKeyDown(KeyCode.F4))
        //{
        //    Debug.Log($"<color=green>Veneno Activado </color>");
        //    StartPoison(5f);
        //}
        //if (Input.GetKeyDown(KeyCode.F5))
        //{
        //    Debug.Log($"<color=red>Sangrado Activado </color>");
        //    StartBleed(10f);
        //}
        if(Input.GetKeyDown(KeyCode.F3))
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

    public override void TakeDamage(float dmg, AttackDirectionList attackDir, bool chargeAttack)
    {
        if (_inmortal) return;
        base.TakeDamage(dmg, attackDir, chargeAttack);
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
        GameManager.Instance.AddToRunStats("Potions Used", 1);
        base.UsePotion();
    }

    public override void AddPotion(int amount = 1)
    {
        base.AddPotion(amount);
        PlayerPrefs.SetInt("potions", potions);
        Debug.Log($"<color=green>Current potions {potions}</color>");
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

    //void ChanceOfBleed()
    //{
    //    if (Random.Range(1, 101) > _bleedChance) return;
    //    //Debug.Log($"<color=red> sangrado aplicado a {GameManager.Instance.EnemyInCombat.name} </color>");
    //    GameManager.Instance.EnemyInCombat.StartPoison(_bleedDuration);
    //}

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

    public override void SetArmor(Armor.Type newArmor, Armor.Quality newArmorQuality)
    {
        base.SetArmor(newArmor, newArmorQuality);
        //Guardar Armor Quality
        switch (newArmor)
        {
            case Armor.Type.Chestplate:
                PlayerPrefs.SetInt("chestQuality", (int)newArmorQuality);
                break;
            case Armor.Type.Helmet:
                PlayerPrefs.SetInt("helmetQuality", (int)newArmorQuality);
                break;
            case Armor.Type.Leggings:
                PlayerPrefs.SetInt("legsQuality", (int)newArmorQuality);
                break;
        }
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

    public float GiveAttackStats(out float _atkSpd, out float _stm, out float _atkStmCost, out bool _outOfBreath, out float _chargeAtkMult)
    {
        _atkSpd = _currentAtkSpd;
        _stm = _stamina;
        _atkStmCost = _staminaCost;
        _outOfBreath = outOfBreath;
        if (myControl.chargedAttack)
            _chargeAtkMult = 2f;
        else
            _chargeAtkMult = 1f;
        return _damage;
    }

    //void EnterShop()
    //{
    //    GameManager.Instance.Shop.GetPlayerInfo(this, ref _currentWeapon);
    //}


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
            PlayerPrefs.SetInt("money", money);
            return true;
        }
    }

    public void AddMoney(int amount)
    {
        //Debug.Log($"{money}, {amount}, {PlayerPrefs.GetInt("money")}");
        money += amount;
        PlayerPrefs.SetInt("money", money);


        GameManager.Instance.AddToRunStats("Money Earned", amount);
        
        //Debug.Log($"{money}, {amount}, {PlayerPrefs.GetInt("money")}");
    }



    protected override void Death()
    {
        print($"<color=red> Murio: {this.name}</color>");

        GameManager.Instance.AddToRunStats("Deaths", 1);

        GameManager.Instance.ChangeScenManager.CallSceneChange(-1);
    }
}
