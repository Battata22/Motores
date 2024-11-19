using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carranza Gonzalo

public class Seller : BaseCharacter, IInteractable
{
    public int money;

    //Enum en vez de List
    private enum SideQuestList
    {

    }
    private SideQuestList _currentQuest;

    private bool _hasActiveQuest;

    public bool blueTeam;

    //Intems in shop
    public List<GameObject> sellingItems;
    public List<GameObject> selledItems;

    [SerializeField] protected BaseCharacter _target;
    [SerializeField] protected float _attackDist;
    [SerializeField, Range(0, 100)] protected int _chanceOfBlock;
    [SerializeField, Range(0, 100)] protected int _chanceOfChargeAttak;
    [SerializeField, Range(0, 1)] protected float _nerfChancePorcentage;
    [SerializeField, Range(0, 100)] float _currentBlockChance;
    [SerializeField] bool _angry = false;

    //Combat CD
    [SerializeField] protected float _enterCombatCD;
    float _lastCombatTime;

    delegate void WhileAngry();
    WhileAngry DoAttack = delegate { }, DoCheckEnterCombat = delegate { }, DoChaseCheck = delegate { }, OpenStore = delegate { };
    protected override void Awake()
    {
        base.Awake();
        _currentBlockChance = _chanceOfBlock;
        OpenStore += OpenStorage;
    }

    private void Update()
    {
        _myLifeSaver.FakeUpdate();

        #region WhileAngry
        DoAttack(); 
        #endregion
    }

    void Attack()
    {
        if (inCombat && Time.time - _lastAttack > _currentAtkSpd)
        {
            _lastAttack = Time.time;

            var dir = ChooseAttackDirection();
            //Debug.Log($"<color=#00ffff>Direccion de Ataque: {dir}</color>");

            ChooseChargeAttack();

            GameManager.Instance.CombatCanvas.ActivateDanger(dir, this, _damage, _currentAtkSpd, _heavyAttack);
        }
    }

    private void FixedUpdate()
    {
        #region WhileAngry
        DoCheckEnterCombat();

        DoChaseCheck(); 
        #endregion
    }

    void CheckEnterCombat()
    {
        if (outOfBreath) return;

        if (!inCombat && (Time.time - _lastCombatTime > _enterCombatCD) && Vector3.SqrMagnitude(_target.transform.position - transform.position) <= (_attackDist * _attackDist))
        {
            GameManager.Instance.EnemyInCombat = this;
            GameManager.Instance.EnterCombat();
            GameManager.Instance.CamRotation.LookAtMe(transform);
        }
    }

    void ChaseCheck()
    {
        if (outOfBreath) return;

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

        //Debug.Log($"{gameObject.name} Enter combat mode");
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

    public override void TakeDamage(float dmg, AttackDirectionList attackDir, bool chargeAttack)
    {
        var num = Random.Range(0, _chanceOfBlock);
        //Debug.Log($"<color=magenta>Numero Random {num} Chance en int {(int)_currentBlockChance}</color>");
        if (!outOfBreath && !chargeAttack && num < (int)_currentBlockChance)
            DoBlock();
        else
            base.TakeDamage(dmg, attackDir, chargeAttack);
    }

    protected void ChooseChargeAttack()
    {
        if (Random.Range(1, 101) <= _chanceOfChargeAttak)
        {
            _heavyAttack = true;
            //Debug.Log($"<color=#ff00ff>Tipo de ataque: Liviano</color>");
        }
        else
        {
            _heavyAttack = false;
            //Debug.Log($"<color=#ff00ff>Tipo de ataque: Pesado</color>");
        }
    }

    protected override void ExitCombat()
    {
        //GameManager.Instance.EnemyInCombat = null;

        //Debug.Log($"{gameObject.name} Exit combat mode");
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

    public override void GetKicked()
    {
        GetAngry();
        base.GetKicked();
    }

    private void GetAngry()
    {
        if (_angry) return;
        _angry = true;
        _lastCombatTime = Time.time;
        DoAttack += Attack;
        DoCheckEnterCombat += CheckEnterCombat;
        DoChaseCheck += ChaseCheck;
        OpenStore -= OpenStorage;
    }

    private void StopAnger()
    {
        _angry = false;
        DoAttack -= Attack;
        DoCheckEnterCombat -= CheckEnterCombat;
        DoChaseCheck -= ChaseCheck;
    }

    public void AssingQuest(Player newPlayer)
    {

    }

    public void OpenStorage()
    {
        Debug.Log($"<color=green> Abriendo Tienda </color>");
        GameManager.Instance.EnterShop();

    }

    public void Interact()
    {
        //throw new System.NotImplementedException();
        OpenStore();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter -= EnterCombat;
    }
}
