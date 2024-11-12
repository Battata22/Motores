using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaControl
{
    BaseCharacter _myOwner;
    float _regenRate, _maxStamina;

    //float _stamina;
    bool exausted;

    public StaminaControl(BaseCharacter myOwner, float newRegenRate, float newMaxstamina)
    {
        _myOwner = myOwner;   
        _regenRate = newRegenRate;
        _maxStamina = newMaxstamina;
    }

    public void FakeUpdate(ref float ownerStamina)
    {
        if(Input.GetKeyUp(KeyCode.Space)) 
        {
            DecreseStamina(ref ownerStamina, 40);
        }
        Debug.Log("Stamina Update");
        if (ownerStamina < _maxStamina && !_myOwner.running)
            StaminaRegen(ref ownerStamina);
    }


    public void DecreseStamina(ref float ownerStamina ,float staminaCost)
    {
        ownerStamina -= staminaCost;
        Debug.Log($"Estamina reducida en {staminaCost} estamina final {ownerStamina}");
    }

    private void StaminaRegen(ref float ownerStamina)
    {
        //Debug.Log("estamina regen llamado");
        float inCombatMult;
        if (_myOwner.inCombat)
            inCombatMult = 0.3f;
        else
            inCombatMult = 1f;
        ownerStamina += _regenRate * inCombatMult * Time.deltaTime;
        //Debug.Log($"Regenerando estamina {_regenRate * inCombatMult * Time.deltaTime}; Estamina final {ownerStamina}; InCombat {_myOwner.inCombat} decrese {inCombatMult}");
        if(ownerStamina > _maxStamina)
        {
            ownerStamina = _maxStamina;
            Debug.Log("<color=green> Estamina LLena </color>");
        }
    }

    void OutOfBreath()
    {

    }
}
