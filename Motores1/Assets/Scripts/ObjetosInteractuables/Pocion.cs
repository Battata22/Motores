using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : MonoBehaviour, IPickeable
{
    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.AddPotion();
        Destroy(gameObject);
    }
}
