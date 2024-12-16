using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionComprable : ItemComprable
{
    [SerializeField] int buyableAmount;

    public override void BuyItem()
    {
        if (buyableAmount <= 0)
        {
            Debug.Log("<color=red>Out of stock</color>");
            return;
        }
        if (!GameManager.Instance.Player.PayAmount(price))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        GameManager.Instance.Player.AddPotion(1);
        buyableAmount--;
    }
}
