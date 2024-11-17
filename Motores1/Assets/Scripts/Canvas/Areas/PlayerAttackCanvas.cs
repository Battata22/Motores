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
        if (dentro && Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLICK DETECTADO");
            StartAttack();
        }
    }

    private void StartAttack()
    {
        float stamina;
        float _attackSpeed;
        float atkStmCost;
        bool outOfBreath;
        //_myAttackDir = attackDir;
        _myDamage = _myOwner.GiveAttackStats(out _attackSpeed, out stamina, out atkStmCost,out outOfBreath);
        //Debug.Log($"Dmg {_myDamage} AtkSpd {_attackSpeed}");

        if (!outOfBreath && stamina >= atkStmCost)
            Attack(_attackSpeed);
        else
        {
            Debug.Log("<color=yellow> DEFICIENCIA DE ESTAMINA </color>");
        }
    }

    void Attack(float _attackSpeed)
    {

        //Debug.Log($"<color=green>Attacando: {GameManager.Instance.Player}</color><color=red>{_myDamage}</color> dmg, direccion <color=cyan>{_myAttackDir}</color>");
        GameManager.Instance.EnemyInCombat.TakeDamage(_myDamage, _myAttackDir);
        _myOwner.BuffAttackSpeed(0.8f);

        _myActivator.DeactivateImages(_attackSpeed);
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
