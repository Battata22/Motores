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

        StartCoroutine(ActiveCosa(5f));
    }

    private IEnumerator ActiveCosa(float num)
    {
        var colider = gameObject.GetComponent<Collider>();
        var mesh = gameObject.GetComponentsInChildren<MeshRenderer>();

        colider.enabled = false;
        foreach (var c in mesh)
        {
            c.enabled = false;
        }

        yield return new WaitForSeconds(num);

        colider.enabled = true;
        foreach (var c in mesh)
        {
            c.enabled = true;
        }

    }
}
