using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carranza Gonzalo, El capo de la mafia
//Corbalan Martin, El apoyo emocional y psicologico
//Lorenzo Solari, El sobreviviente de la nieve
//Gianluca Battaglino, El verdulero que alimento al equipo

//TPFinal - Lorenzo Solari - Carranza Gonzalo - Corbalan Martin - Gianluca Battaglino.

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharacter : MonoBehaviour, IKickable
{
    //Basic
    protected string _name;
    [SerializeField] protected float _maxHp; //agregado
    [SerializeField] protected float _hp;
    [SerializeField] protected float _maxStamina; //agregado
    protected float _stamina; //cambiado a protected
    [SerializeField] protected float _staminaRegenRate;
    protected float _currentStaminaRegen;
    [SerializeField] protected float _speed;
    //Run
    [SerializeField] protected float _runningSpeed;
    [SerializeField] protected float _runningStmCost;
    public bool running { get; protected set; } //añadido running

    protected Rigidbody _rb; //agregado

    //attack
    [SerializeField] protected float _damage, _baseAttackSpeed;
    [SerializeField]protected float _currentAtkSpd;
    protected float _lastAttack = -1;
    protected bool _heavyAttack;
    //armor
    [SerializeField] protected Armor[] armorEquiped;
    //protected int _helmet, _chestPlate, _boots;

    //Weapon
    [SerializeField] protected Weapon.WeaponType _currentWeapon;
    protected float _staminaCost;

    //Damage in parts
    //protected bool _isHeadDamage, _isChestDamage, _areLegsDamage;

    protected bool _blocking;

    public bool inCombat{ get; protected set;}

    //Effects
    public bool bleading, poisoned, outOfBreath;

    protected int potions;

    protected HealthSystem _myHealthSystem;
    protected ArmorControl _myArmorControl;
    protected DOTControl _myDOTControl;
    protected StaminaControl _myStaminaControl;
    protected LifeSaver _myLifeSaver;

    public delegate void VoidDelegate();
    public VoidDelegate _myAttack, _myChargeAttack, _myBlock;

    public delegate void DotsDelegate(ref float hp);

    /// <summary>
    /// ESTO ES UN EVENTO, SOLO USAR += O -=
    /// </summary>
    public DotsDelegate CallDOTs = delegate { };


    [SerializeField] protected bool _canMove = true;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myArmorControl = new ArmorControl(this);
        _myHealthSystem = new HealthSystem(this);
        _myDOTControl = new DOTControl(this, _myHealthSystem);
        _currentStaminaRegen = _staminaRegenRate;
        _myStaminaControl = new StaminaControl(this, _currentStaminaRegen, _maxStamina);
        _myLifeSaver = new LifeSaver(this.transform);
        
        if (PlayerPrefs.HasKey("currentWeapon"))
            SetWeapon((Weapon.WeaponType)PlayerPrefs.GetInt("currentWeapon"));
        else
            SetWeapon(_currentWeapon);

        ResetAttackSpeed();
        //CallDOTs += delegate { };
      
        inCombat = false;
        outOfBreath = false;
        _hp = _maxHp;
        _stamina = _maxStamina;
    }

    protected virtual void Start()
    {

        GameManager.Instance.OnCombatEnter += EnterCombat;
    }

    //ahora toma un int
    public virtual void TakeDamage(float dmg, AttackDirectionList attackDir, bool chargeAttack) 
    {
        
        _myHealthSystem.CalcDamage(ref _hp, dmg, _myArmorControl, attackDir);
        //Debug.Log($"<color=green>{this.name} </color>| Raw Damage: <color=red>{dmg} </color>| Direccion: <color=cyan>{attackDir} " +
        //    $"</color>| Cuerrent Life <color=#ff00ff> {_hp}</color>");

        Debug.Log($"<color=magenta> {this.name} Recibio Daño </color>");
        if(_hp < 0 )
        {
            _hp = 0;
            Death();
        }
    }

    protected virtual void Death() 
    {       
        print($"<color=red> Murio: {this.name}</color>");
        GameManager.Instance.ExitCombat();

        GameManager.Instance.AddToRunStats("Enemies Killed", 1);

        Destroy(gameObject);
    }

    //Cambiado, Ahora pide 2 floats
    //protected virtual void Movement(float xAxis, float zAxis) { }

    protected virtual void Run() { }

    protected virtual void SwordNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);
    }
    protected virtual void SwordCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost * 2f);
    }
    protected virtual void SwordBlock() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);
    }

    protected virtual void SwAndShielsNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);
    }
    protected virtual void SwAndShielsCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost * 2f);
    }
    protected virtual void SwAndShielsBlock() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);

    }

    protected virtual void GreatSwordNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);
    }
    protected virtual void GreatSwordCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost * 2f);
    }
    protected virtual void GreatSwordBlock() 
    {
        _myStaminaControl.DecreseStamina(ref _stamina, _staminaCost);

    }

    public virtual void AddPotion(int amount = 1)
    {
        potions += amount;

    }

    protected abstract void EnterCombat();
    protected abstract void ExitCombat();

    public virtual void UsePotion() 
    {
        StopBleed();
        StopPoison();
    }

    public virtual void Kick()
    {
        Debug.Log($"{gameObject.name} use Kick");
        //Debug.Log("Saliendo de modo Pelea");
        GameManager.Instance.ExitCombat();
    }

    public virtual void GetKicked()
    {
        Debug.Log($"{gameObject.name} Recive a kick");
        _rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
    }

    public virtual void SetWeapon(Weapon.WeaponType newWeapon)
    {

        _currentWeapon = newWeapon;

        switch (_currentWeapon)
        {
            case Weapon.WeaponType.Sword:
                _myAttack = SwordNAttack;
                _myChargeAttack = SwordCAttack;
                _myBlock = SwordBlock;
                _damage = 20f;
                _staminaCost = 20f;
                break;
            case Weapon.WeaponType.GreatSword:
                _myAttack = SwAndShielsNAttack;
                _myChargeAttack = SwAndShielsCAttack;
                _myBlock = SwAndShielsBlock;
                _damage = 35f;
                _staminaCost = 30f;
                break;
            case Weapon.WeaponType.SwordAndShield:
                _myAttack = GreatSwordNAttack;
                _myChargeAttack = GreatSwordCAttack;
                _myBlock = GreatSwordBlock;
                _damage = 20f;
                _staminaCost = 20f;
                break;
        }

        PlayerPrefs.SetInt("currentWeapon", (int)_currentWeapon);
        Debug.Log($"Current Weapon: {_currentWeapon}");

    }

    public virtual void SetArmor(Armor.Type newArmor, Armor.Quality newArmorQuality) 
    {
        _myArmorControl.SetArmor(newArmor, newArmorQuality);
        
        
    }

    public virtual void UpgradeArmor(Armor.Type toUpgrade)
    {
        _myArmorControl.UpgradeArmor(toUpgrade);
    }

    public virtual void StartBleed(float duration) 
    {
        _myDOTControl.bleedDuration = duration;
        CallDOTs += _myDOTControl.DoBleedDamage;
    }
    public virtual void StopBleed()
    {
        _myDOTControl.bleedDuration = 0;
        CallDOTs -= _myDOTControl.DoBleedDamage;
    }

    public virtual void StartPoison(float duration) 
    {
        //set poison duration
        _myDOTControl.poisonDuration = duration;
        CallDOTs += _myDOTControl.DoPoisonDamage;
    }
    public virtual void StopPoison()
    {
        _myDOTControl.poisonDuration = 0;
        CallDOTs -= _myDOTControl.DoPoisonDamage;
    }

    /// <summary>
    /// Usar que tanto % queres que se mantenga. Porcentaje de 0 a 1
    /// </summary>
    /// <param name="num"></param>
    public void BuffAttackSpeed(float buff)
    {
        buff = Mathf.Clamp(buff, 0, 1);
        _currentAtkSpd *= buff;
        if (_currentAtkSpd < 0.15f)
            _currentAtkSpd = 0.15f;
    }

    public void ResetAttackSpeed()
    {
        _currentAtkSpd = _baseAttackSpeed;
    }

    /// <summary>
    /// Añadir X% al regen actual;
    /// </summary>
    /// <param name="buff"></param>
    public void BuffStamRegen(float buff)
    {
        buff = Mathf.Clamp(buff, 0, 1);
        _currentStaminaRegen += _currentStaminaRegen * buff;
        if (_currentStaminaRegen > _staminaRegenRate * 4)
            _currentStaminaRegen = _staminaRegenRate * 4;
        _myStaminaControl.UpdateStamRegen(_currentStaminaRegen);
    }

    public void ResetStamRegen()
    {
        _currentStaminaRegen = _staminaRegenRate;
        _myStaminaControl.UpdateStamRegen(_currentStaminaRegen);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
        if(GameManager.Instance.EnemyInCombat == this)
        {
            GameManager.Instance.ExitCombat();
            //GameManager.Instance.EnemyInCombat = null;
        }
    }

    //Identificador de golpe
    //public enum HitArea
    //{
    //    Head,
    //    Chest,
    //    Legs
    //}

    //public enum Weapon
    //{
    //    GreatSword,
    //    Sword,
    //    SwordAndShield
    //}

    public enum AttackDirectionList
    {
        UpLeft,
        UpCenter,
        UpRight,
        MidLeft,
        MidCenter,
        MidRight,
        LowLeft,
        LowCenter,
        LowRight,
        Kick,
    }
}
