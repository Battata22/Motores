using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickeable
{
    public WeaponType _weaponType;

    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.SetWeapon(_weaponType);
        //Destroy(gameObject);

        StartCoroutine(ActiveCosa(5f));
    }


    //momentaneo para testeo
    private IEnumerator ActiveCosa(float num)
    {
        var colider = gameObject.GetComponent<Collider>();
        var mesh = gameObject.GetComponentsInChildren<MeshRenderer>();

        colider.enabled = false;
        foreach ( var c in mesh)
        {
            c.enabled=false;
        }

        yield return new WaitForSeconds(num);

        colider.enabled = true;
        foreach (var c in mesh)
        {
            c.enabled = true;
        }

    }

    public enum WeaponType
    {
        GreatSword,
        Sword,
        SwordAndShield
    }
}
