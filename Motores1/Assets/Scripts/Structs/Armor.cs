using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Armor
{
    public enum Type
    {
        Helmet,
        Chestplate,
        Leggings
    }

    public enum Quality
    {
        None,
        Low,
        Mid,
        High
    }

    public Type type;
    public Quality quality;
}
