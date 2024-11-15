using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorControl
{
    BaseCharacter _myOwner;

    //int _armor;
    public float _headArmor, _chestArmor, _legsArmor;

    public ArmorControl(BaseCharacter myOwner)
    {
        _myOwner = myOwner;
        //_armor = armor;
        //_incomingDmg = incomingDmg;
    }

    public float CalcArmor(float rawDamage, BaseCharacter.AttackDirectionList attackDir)
    {
        //Debug.Log("Llamado calculo de armadura");
        float result = 0;

        switch (attackDir)
        {
            case BaseCharacter.AttackDirectionList.UpLeft:
            case BaseCharacter.AttackDirectionList.UpCenter:
            case BaseCharacter.AttackDirectionList.UpRight:
                result = CalcHelmetArmor(rawDamage);
                break;
            case BaseCharacter.AttackDirectionList.MidLeft:
            case BaseCharacter.AttackDirectionList.MidCenter:
            case BaseCharacter.AttackDirectionList.MidRight:
                result = CalcChestArmor(rawDamage); 
                break;
            case BaseCharacter.AttackDirectionList.LowLeft:
            case BaseCharacter.AttackDirectionList.LowCenter:
            case BaseCharacter.AttackDirectionList.LowRight:
                result = CalcLegsArmor(rawDamage);
                break;
        }

        return result;
    }

    private float CalcHelmetArmor(float rawDamage)
    {
        float result = rawDamage;
        result -= rawDamage * _chestArmor * 0.1f;
        //Debug.Log($"Armadura de Cabeza calculada: Daño crudo {rawDamage} Daño final {result}");
        return result;
    }

    private float CalcChestArmor(float rawDamage)
    {
        float result = rawDamage;
        result -= rawDamage * _chestArmor * 0.1f;
        //Debug.Log($"Armadura de Pecho calculada: Daño crudo {rawDamage} Daño final {result}");
        return result;
    }

    private float CalcLegsArmor(float rawDamage)
    {
        float result = rawDamage;
        result -= rawDamage * _chestArmor * 0.1f;
        //Debug.Log($"Armadura de Piernas calculada: Daño crudo {rawDamage} Daño final {result}");
        return result;
    }

    public void SetArmor(ArmorPice.ArmorType armorType, ArmorPice.ArmorQuality armorQuality)
    {
        switch(armorType)
        {
            case ArmorPice.ArmorType.Helmet:
                _headArmor = (int)armorQuality;
                Debug.Log($"Casco equipado {_headArmor}");
                break;
            case ArmorPice.ArmorType.Chestplate:
                _chestArmor = (int)armorQuality;
                Debug.Log($"Pechera equipado {_chestArmor}");
                break;
            case ArmorPice.ArmorType.Leggings:
                _legsArmor = (int)armorQuality;
                Debug.Log($"Pantalones equipado {_legsArmor}");
                break;
        }
    }
}
