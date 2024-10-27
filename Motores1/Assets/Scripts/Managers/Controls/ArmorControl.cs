using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorControl
{
    BaseCharacter _myOwner;

    //int _armor;
    private int _incomingDmg;

    public ArmorControl(BaseCharacter myOwner)
    {
        _myOwner = myOwner;
        //_armor = armor;
        //_incomingDmg = incomingDmg;
    }

    public int ReduceDamag()
    {
        int result = 0;
        return result;
    }
}
