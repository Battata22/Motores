using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordComprable : ItemComprable
{
    public override void BuyItem()
    {
        if (GameManager.Instance.Player.playerWeapon == Weapon.WeaponType.GreatSword)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!GameManager.Instance.Player.PayAmount(price))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        GameManager.Instance.Player.SetWeapon(Weapon.WeaponType.GreatSword);
    }
}
