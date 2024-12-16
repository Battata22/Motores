using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemComprable : MonoBehaviour
{
    public int price;
    protected bool _soldOut = false;

    public abstract void BuyItem();
}
