using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickeable
{
    public WeaponType _weaponType;

    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.SetWeapon(_weaponType);
    }

    public enum WeaponType
    {
        GreatSword,
        Sword,
        SwordAndShield
    }
}
