using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTControl
{
    BaseCharacter _myOwner;
    HealthSystem _healthSystem;

    public float poisonDuration, bleedDuration;

    public DOTControl(BaseCharacter myOwner, HealthSystem healthSystem)
    {
        _myOwner = myOwner;
        _healthSystem = healthSystem;
    }

    public void DoPoisonDamage(ref float ownerHp)
    {
        Debug.Log($"<color=green> Tick de veneno {GameManager.Instance.poisonDamage * Time.deltaTime} </color>");
        if (poisonDuration <= 0)
        {
            //desuscribir
            Debug.Log($"<color=green> Stop de veneno {GameManager.Instance.poisonDamage} vida restante {ownerHp}</color>");
            _myOwner.CallDOTs -= DoPoisonDamage;
            poisonDuration = 0;
            return;
        }
        poisonDuration -= Time.deltaTime;
        _healthSystem.CalcDamage(ref ownerHp, GameManager.Instance.poisonDamage);
    }

    public void DoBleedDamage(ref float ownerHp)
    {
        Debug.Log($"<color=red> Tick de sangrado {GameManager.Instance.bleedDamage * Time.deltaTime} </color>");
        if (bleedDuration <= 0)
        {
            //desuscribir
            Debug.Log($"<color=red> Stop de sangrado {GameManager.Instance.poisonDamage} vida restante {ownerHp}</color>");
            _myOwner.CallDOTs -= DoBleedDamage;
            bleedDuration = 0;
            return;
        }
        bleedDuration -= Time.deltaTime;
        _healthSystem.CalcDamage(ref ownerHp, GameManager.Instance.bleedDamage);
    }
}
