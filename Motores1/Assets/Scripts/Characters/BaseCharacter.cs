using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharacter : MonoBehaviour, IKickable
{
    //Basic
    protected string _name;
    [SerializeField] protected float _maxHp; //agregado
    protected float _hp;
    [SerializeField] protected float _maxStamina; //agregado
    protected float stamina; //cambiado a protected
    [SerializeField] protected float _speed;
    [SerializeField] protected float _staminaRegenRate;
    public bool running { get; protected set; } //añadido running
    protected Rigidbody _rb; //agregado

    //attack
    [SerializeField] protected float _damage, _attackSpeed;
    protected float _lastAttack = -1;
    protected bool _heavyAttack;
    //armor
    protected int _helmet, _chestPlate, _boots;

    //Weapon
    [SerializeField] protected Weapon.WeaponType _currentWeapon;
    protected float _staminaCost;

    //Damage in parts
    protected bool _isHeadDamage, _isChestDamage, _areLegsDamage;

    protected bool _blocking;

    public bool inCombat{ get; protected set;}

    //Effects
    public bool bleading, poisoned;

    protected int _potions;

    protected HealthSystem _myHealthSystem;
    protected ArmorControl _myArmorControl;
    protected DOTControl _myDOTControl;
    protected StaminaControl _myStaminaControl;

    public delegate void VoidDelegate();
    public VoidDelegate _myAttack, _myChargeAttack, _myBlock;

    [SerializeField] protected bool _canMove = true;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myArmorControl = new ArmorControl(this);
        _myDOTControl = new DOTControl(this);
        _myHealthSystem = new HealthSystem(this);
        _myStaminaControl = new StaminaControl(this, _staminaRegenRate, _maxStamina);
        SetWeapon(_currentWeapon);

        inCombat = false;
        _hp = _maxHp;
        stamina = _maxStamina;
    }

    protected virtual void Start()
    {
        GameManager.Instance.OnCombatEnter += EnterCombat;
    }

    //ahora toma un int
    protected virtual void TakeDamage(int dmg) 
    {
        //print($"<color=red> Dmg entrante {dmg} </color>");
        //print($"<color=green> Vida Inicial {_hp}</color>");
        _hp -= _myHealthSystem.CalcDamage(dmg);
        //print($"<color=green> Vida restante {_hp}</color>");

        if(_hp < 0 )
        {
            _hp = 0;
            Death();
        }
    }

    protected virtual void Death() 
    {       
        print("<color=red> AHHHHHHHH, ME MUUEEROOOOOOOooooooohhhhhh......</color>");
    }

    //Cambiado, Ahora pide 2 floats
    //protected virtual void Movement(float xAxis, float zAxis) { }

    protected virtual void Run() { }

    protected virtual void SwordNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost);
    }
    protected virtual void SwordCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost * 2f);
    }
    protected virtual void SwordBlock() { }

    protected virtual void SwAndShielsNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost);
    }
    protected virtual void SwAndShielsCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost * 2f);
    }
    protected virtual void SwAndShielsBlock() { }

    protected virtual void GreatSwordNAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost);
    }
    protected virtual void GreatSwordCAttack() 
    {
        _myStaminaControl.DecreseStamina(ref stamina, _staminaCost * 2f);
    }
    protected virtual void GreatSwordBlock() { }


    protected abstract void EnterCombat();
    protected abstract void ExitCombat();


    public virtual void Kick()
    {
        Debug.Log($"{gameObject.name} use Kick");
        //Debug.Log("Saliendo de modo Pelea");
        GameManager.Instance.ExitCombat();
    }

    public void GetKicked()
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
                _staminaCost = 20f;
                break;
            case Weapon.WeaponType.GreatSword:
                _myAttack = SwAndShielsNAttack;
                _myChargeAttack = SwAndShielsCAttack;
                _myBlock = SwAndShielsBlock;
                _staminaCost = 30f;
                break;
            case Weapon.WeaponType.SwordAndShield:
                _myAttack = GreatSwordNAttack;
                _myChargeAttack = GreatSwordCAttack;
                _myBlock = GreatSwordBlock;
                _staminaCost = 20f;
                break;
        }
        Debug.Log($"Current Weapon: {_currentWeapon}");

    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }

    //Identificador de golpe
    public enum HitArea
    {
        Head,
        Chest,
        Legs
    }

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
