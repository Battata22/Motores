using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaControl
{
    BaseCharacter _myOwner;
    float _regenRate, _maxStamina;
    float _outOfBreathMul = 1f;

    public delegate void VoidDelegateBool(bool b);
    public event VoidDelegateBool OnOutOfBreath = delegate { };

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
        //Debug.Log("Stamina Update");
        if (ownerStamina < _maxStamina && !_myOwner.running)
            StaminaRegen(ref ownerStamina);
    }

    public void UpdateStamRegen(float newRegenRate)
    {
        _regenRate = newRegenRate;
    }


    public void DecreseStamina(ref float ownerStamina ,float staminaCost)
    {
        ownerStamina -= staminaCost;
        //Debug.Log($"Estamina reducida en {staminaCost} estamina final {ownerStamina}");
        if(ownerStamina < 0)
        {
            OutOfBreath();
            ownerStamina = 0;
        }
    }

    private void StaminaRegen(ref float ownerStamina)
    {
        Debug.Log($"<color=green> {_myOwner} Regenerado estamina | Regen rate {_regenRate} </color>");
        float inCombatMult;
        if (_myOwner.inCombat)
            inCombatMult = 0.3f;
        else
            inCombatMult = 1f;
        ownerStamina += _regenRate * inCombatMult * Time.deltaTime * _outOfBreathMul;
        //Debug.Log($"Regenerando estamina {_regenRate * inCombatMult * Time.deltaTime}; Estamina final {ownerStamina}; InCombat {_myOwner.inCombat} decrese {inCombatMult}");
        if(ownerStamina > _maxStamina)
        {
            ownerStamina = _maxStamina;
            //Debug.Log("<color=green> Estamina LLena </color>");
            _myOwner.outOfBreath = false;
            _outOfBreathMul = 1f;
        }
    }

    void OutOfBreath()
    {
        // Llamar cuando llegas a 0
        // Set out of breath
        //evita cuanquier accion que requiera estamina
        OnOutOfBreath(false);
        _myOwner.outOfBreath = true;
        _outOfBreathMul = 1.5f;

    }
}
