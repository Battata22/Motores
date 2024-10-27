using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaControl : MonoBehaviour
{
    BaseCharacter _myOwner;

    //float _stamina;
    bool exausted;

    public StaminaControl(BaseCharacter myOwner)
    {
        _myOwner = myOwner;
    }

    public void Running()
    {

    }

    void OutOfBreath()
    {

    }
}
