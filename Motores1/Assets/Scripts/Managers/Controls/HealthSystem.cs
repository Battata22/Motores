using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    BaseCharacter _myOwner;

    private int _hp;
    //private int _incomingDmg;

    public HealthSystem(BaseCharacter newOwner)
    {
        _myOwner = newOwner;
        //_hp = newHp;
        //_incomingDmg = newDamage;
    }

    public int CalcDamage(int rawDamage)
    {
        //llamar a armadura
        int resultDmg = rawDamage;

        return resultDmg;
    }

    public int CalcHeal(int healAmount)
    {
        //Va a haber buff o debuff de healing?
        int resultHeal = healAmount;

        return resultHeal;
    }

    void Death()
    {

    }
}
