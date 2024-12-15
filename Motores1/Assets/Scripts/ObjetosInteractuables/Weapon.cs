using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static Weapon;

public class Weapon : ItemComprable, IPickeable
{
    public WeaponType weaponType; 

    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.SetWeapon(weaponType);
        //Destroy(gameObject);

        StartCoroutine(ActiveCosa(5f));
    }

    public override void BuyItem()
    {
        switch (weaponType)
        {
            case WeaponType.Sword:
                BuySword();
                break;
            case WeaponType.GreatSword:
                BuyGreatSword();
                break;
            case WeaponType.SwordAndShield:
                BuySandShield();
                break;
        }
    }

    public void BuySword()
    {
        if (GameManager.Instance.Player.playerWeapon == Weapon.WeaponType.Sword)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!GameManager.Instance.Player.PayAmount(price))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        GameManager.Instance.Player.SetWeapon(Weapon.WeaponType.Sword);

        //_weaponType = Weapon.WeaponType.Sword;

    }
    public void BuyGreatSword()
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

        //_weaponType = Weapon.WeaponType.GreatSword;
        //_playerMoney -= weaponCost;


    }
    public void BuySandShield()
    {
        if (GameManager.Instance.Player.playerWeapon == Weapon.WeaponType.SwordAndShield)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!GameManager.Instance.Player.PayAmount(price))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        GameManager.Instance.Player.SetWeapon(Weapon.WeaponType.SwordAndShield);

        //_weaponType = Weapon.WeaponType.SwordAndShield;
        //_playerMoney -= weaponCost;

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
