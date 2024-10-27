using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharacter : MonoBehaviour
{
    //Basic
    protected string _name; 
    [SerializeField] protected float _maxHp; //agregado
    protected float _hp;
    [SerializeField] protected float _maxStamina; //agregado
    protected float stamina; //cambiado a protected
    [SerializeField] protected float _speed;
    public bool running; //añadido running
    protected Rigidbody _rb; //agregado

    //attack
    [SerializeField] protected float _damage, _attackSpeed;

    //armor
    protected int _helmet, _chestPlate, _boots;

    //Damage in parts
    private bool _isHeadDamage, _isChestDamage, _areLegsDamage;

    protected bool _blocking;

    //Effects
    public bool bleading, poisoned;

    protected int _potions;

    HealthSystem _myHealthSystem;
    ArmorControl _myArmorControl;
    DOTControl _myDOTControl;
    StaminaControl _myStaminaControl;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myArmorControl = new ArmorControl(this);
        _myDOTControl = new DOTControl(this);
        _myHealthSystem = new HealthSystem(this);
        _myStaminaControl = new StaminaControl(this);

        _hp = _maxHp;
        stamina = _maxStamina;
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
    public virtual void Movement(float xAxis, float zAxis) { }

    protected virtual void Run() { }

    protected virtual void Attack() { }

    protected virtual void Block() { }

    //Identificador de golpe
    public enum HitArea
    {
        Head,
        Chest,
        Legs
    }
}
