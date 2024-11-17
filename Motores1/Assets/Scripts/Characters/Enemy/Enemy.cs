using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : BaseCharacter
{
    public bool blueTeam;

    [SerializeField] protected BaseCharacter _target;
    [SerializeField] protected float _attackDist;
    [SerializeField, Range(0,100)] protected int _chanceOfBlock;
    [SerializeField, Range(0,1)] protected float _nerfChancePorcentage;
    [SerializeField, Range(0, 100 )] float _currentBlockChance;

    //Combat CD
    [SerializeField] protected float _enterCombatCD;
    float _lastCombatTime = -1;

    bool noAI = false;

    protected override void Awake()
    {
        base.Awake();
        _currentBlockChance = _chanceOfBlock;
        GameManager.Instance.OnShopActive += DisableAI;
        GameManager.Instance.OnShopDisable += EnableAI;
    }

    private void Update()
    {
        if (noAI) return;
        _myLifeSaver.FakeUpdate();

        if (inCombat && Time.time - _lastAttack > _currentAtkSpd)
        {
            _lastAttack = Time.time;

            var dir = ChooseAttackDirection();
            //Debug.Log($"<color=#00ffff>Direccion de Ataque: {dir}</color>");
            
            if(Random.Range(0,2) == 0)
            {
                _heavyAttack = false;
                //Debug.Log($"<color=#ff00ff>Tipo de ataque: Liviano</color>");
            }
            else
            {
                _heavyAttack = true;
                //Debug.Log($"<color=#ff00ff>Tipo de ataque: Pesado</color>");
            }

            GameManager.Instance.CombatCanvas.ActivateDanger(dir, this, _damage, _currentAtkSpd);
        }
    }

    private void FixedUpdate()
    {
        if (noAI) return;

        if (!inCombat && (Time.time - _lastCombatTime > _enterCombatCD) && Vector3.SqrMagnitude(_target.transform.position - transform.position) <= (_attackDist * _attackDist))
        {
            GameManager.Instance.EnemyInCombat = this;
            GameManager.Instance.EnterCombat();
            GameManager.Instance.CamRotation.LookAtMe(transform);
        }
        if (!(Vector3.SqrMagnitude(_target.transform.position - transform.position) <= (_attackDist * _attackDist)))
            ChaseTarget();
    }

    protected AttackDirectionList ChooseAttackDirection()
    {
        //var attacks = new Attacks();
        var attacks = (AttackDirectionList)Random.Range(0, (int)AttackDirectionList.Kick);
        return attacks;

        #region Test
        //switch(attacks) 
        //{
        //    case AttackList.UpLeft:
        //        Debug.Log($"<color=#8a8a8a>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.UpCenter:
        //        Debug.Log($"<color=#0000ff>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.UpRight:
        //        Debug.Log($"<color=#00ff00>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.MidLeft:
        //        Debug.Log($"<color=#00ffff>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.MidCenter:
        //        Debug.Log($"<color=#ff0000>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.MidRight:
        //        Debug.Log($"<color=#ff00ff>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.LowLeft:
        //        Debug.Log($"<color=#ffff00>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.LowCenter:
        //        Debug.Log($"<color=#ffffff>Ataque realizado {attacks}</color>");
        //        break;
        //    case AttackList.LowRight:
        //        Debug.Log($"<color=#000000>Ataque realizado {attacks}</color>");
        //        break;
        //} 
        #endregion
    }

    protected override void EnterCombat()
    {
        //GameManager.Instance.EnemyInCombat = this;

        Debug.Log($"{gameObject.name} Enter combat mode");
        _canMove = false;
        inCombat = true;

        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;
        //GameManager.Instance.CamRotation.LookAtMe(this.transform);

        transform.LookAt(_target.transform);

        //GameManager.Instance.mouseCenterGO.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; ;
    }

    public override void TakeDamage(float dmg, AttackDirectionList attackDir)
    {
        var num = Random.Range(0, _chanceOfBlock);
        //Debug.Log($"<color=magenta>Numero Random {num} Chance en int {(int)_currentBlockChance}</color>");
        if (num < (int)_currentBlockChance)
            DoBlock();
        else
            base.TakeDamage(dmg, attackDir);
    }

    protected override void ExitCombat()
    {
        //GameManager.Instance.EnemyInCombat = null;

        Debug.Log($"{gameObject.name} Exit combat mode");
        _canMove = true;
        inCombat = false;

        ResetAttackSpeed();
        ResetBlockChance();

        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        _lastCombatTime = Time.time;

        //GameManager.Instance.mouseCenterGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void ChaseTarget()
    {
        if (!_canMove) return;
        var dir = _target.transform.position - transform.position;
        _rb.position += dir.normalized * _speed * Time.fixedDeltaTime;

        transform.LookAt(_target.transform);
    }

    void DoBlock()
    {
        Debug.Log($"<color=red> ATAQUE BLOQUEADO </color>");
        NerfBlockChance();
    }

    protected void NerfBlockChance()
    {
        _currentBlockChance *= _nerfChancePorcentage;
    }

    void ResetBlockChance()
    {
        _currentBlockChance = _chanceOfBlock;
    }   

    void DisableAI()
    {
        noAI = true;
    }

    void EnableAI()
    {
        noAI = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnShopActive -= DisableAI;
        GameManager.Instance.OnShopDisable -= EnableAI;
        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter -= EnterCombat;
    }
}
