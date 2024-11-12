using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPice : MonoBehaviour, IPickeable
{
    public enum ArmorType
    {
        Helmet,
        Chestplate,
        Leggings
    }

    public enum ArmorQuality
    {
        None,
        Low,
        Mid,
        High
    }

    [SerializeField] ArmorType armorType;
    [SerializeField] ArmorQuality armorQuality;

    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.SetArmor(armorType, armorQuality);
    }
}
