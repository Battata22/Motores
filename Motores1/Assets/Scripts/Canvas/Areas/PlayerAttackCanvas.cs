using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCanvas : MonoBehaviour
{
    bool dentro = false;
    [SerializeField] BaseCharacter.AttackDirectionList _myAttackDir;

    [SerializeField] Player _myOwner;
    private float _myDamage;

    public delegate void AttackCanvasDelegate();
    public event AttackCanvasDelegate AttackCanvas;

    PlayerCanvasAttackActivate _myActivator;

    void Update()
    {
        if (dentro && Input.GetMouseButtonUp(0))
        {
            //Debug.Log("CLICK DETECTADO");
            StartAttack();
        }
    }

    private void StartAttack()
    {
        float stamina;
        float _attackSpeed;
        float atkStmCost;
        bool outOfBreath;
        float chargeAtkMult;
        //_myAttackDir = attackDir;
        _myDamage = _myOwner.GiveAttackStats(out _attackSpeed, out stamina, out atkStmCost,out outOfBreath, out chargeAtkMult);
        //Debug.Log($"Dmg {_myDamage} AtkSpd {_attackSpeed}");

        if (!outOfBreath && stamina >= atkStmCost)
            Attack(_attackSpeed, chargeAtkMult);
        else
        {
            Debug.Log("<color=yellow> DEFICIENCIA DE ESTAMINA </color>");
        }
    }

    void Attack(float _attackSpeed,float _chargeMult)
    {
        bool chargeAttack = false;
        if (_chargeMult != 1f)chargeAttack = true;
        //Debug.Log($"<color=green>Attacando: {GameManager.Instance.Player}</color><color=red>{_myDamage}</color> dmg, direccion <color=cyan>{_myAttackDir}</color>");
        _myActivator.DeactivateImages(_attackSpeed);
        GameManager.Instance.EnemyInCombat.TakeDamage(_myDamage * _chargeMult, _myAttackDir, chargeAttack);
        if (chargeAttack)
            _myOwner._myChargeAttack();
        else
            _myOwner._myAttack();

        _myOwner.BuffAttackSpeed(0.8f);
        _myOwner.BuffStamRegen(0.5f);

    }

    public void GetActivator(PlayerCanvasAttackActivate newActivator)
    {
        _myActivator = newActivator;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dentro = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dentro = false;
    }

    private void OnDisable()
    {
        dentro = false;
    }
}
