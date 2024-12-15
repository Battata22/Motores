using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carranza Gonzalo

public class HealthSystem
{
    BaseCharacter _myOwner;

    //private int _incomingDmg;

    public HealthSystem(BaseCharacter newOwner)
    {
        _myOwner = newOwner;
        //_hp = newHp;
        //_incomingDmg = newDamage;
    }

    

    public void CalcDamage(ref float ownerHp, float rawDamage, ArmorControl armorCtrl, BaseCharacter.AttackDirectionList attackDir)
    {
        //float resultHp = ownerHp;
        //llamar a armadura
        float resultDmg = armorCtrl.CalcArmor(rawDamage, attackDir);
        //Debug.Log($"DAÑO A REALIZAR: {resultDmg}");
        ownerHp -= resultDmg;
        if(_myOwner.transform.TryGetComponent<Player>(out var player))
        {
            GameManager.Instance.AddToRunStats("Hits Taken", 1);
            GameManager.Instance.AddToRunStats("Dmg Taken", resultDmg);
        }
        else
        {
            GameManager.Instance.AddToRunStats("Hits On Targets", 1);
            GameManager.Instance.AddToRunStats("Dmg Dealt", resultDmg);
        }
    }

    /// <summary>
    /// USAR SOLO PARA DAMAGE OVER TIME
    /// </summary>
    /// <param name="ownerHp"></param>
    public void CalcDamage(ref float ownerHp, float damage)
    {
        float resultDmg = damage * Time.deltaTime;

        ownerHp -= resultDmg;

        if (_myOwner.transform.TryGetComponent<Player>(out var player))
        {
            GameManager.Instance.AddToRunStats("Dmg Taken", resultDmg);
        }
        else
        {
            GameManager.Instance.AddToRunStats("Dmg Dealt", resultDmg);
        }
    }

    public void CalcHeal(ref float ownerHp ,float healAmount)
    {
        //Va a haber buff o debuff de healing?
        float resultHeal = healAmount;

        ownerHp += resultHeal;

        if (_myOwner.transform.TryGetComponent<Player>(out var player))
        {
            GameManager.Instance.AddToRunStats("Heals Received", resultHeal);
        }
    }
}
