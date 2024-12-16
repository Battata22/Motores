using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestComprable : ItemComprable
{
    public override void BuyItem()
    {
        if (_soldOut)
        {
            Debug.Log("<color=red>Mejora ya comprada</color>");
            return;
        }
        if (!GameManager.Instance.Player.PayAmount(price))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        GameManager.Instance.Player.UpgradeArmor(Armor.Type.Chestplate);
        _soldOut = true;
    }
}
