using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] int potionCost;
    [SerializeField] int potionAmount;
    [SerializeField] ArmorPice.ArmorQuality armorQuality;
    [SerializeField] int armorCost;
    [SerializeField] int weaponCost;

    Player _player;
    //int _playerMoney;
    Weapon.WeaponType _weaponType;

    bool _soldOutChest = false, _soldOutHelmet = false, _soldOutPants = false;

    private void Start()
    {
        GameManager.Instance.Shop = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.ExitShop();
        }
    }

    public void GetPlayerInfo(Player player, ref Weapon.WeaponType weaponType)
    {
        _player = player;
        //_playerMoney = money;
        _weaponType = weaponType;

    }

    public void BuyHelmet()
    {
        if (_soldOutHelmet)
        {
            Debug.Log("<color=red>Mejora ya comprada</color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        //_player.SetArmor(ArmorPice.ArmorType.Helmet, armorQuality);
        _player.UpgradeArmor(ArmorPice.ArmorType.Helmet);
        _soldOutHelmet = true;
    }
    public void BuyChest()
    {
        if (_soldOutChest)
        {
            Debug.Log("<color=red>Mejora ya comprada</color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        //_player.SetArmor(ArmorPice.ArmorType.Chestplate, armorQuality);
        _player.UpgradeArmor(ArmorPice.ArmorType.Chestplate);
        _soldOutChest = true;
    }
    public void BuyPants()
    {
        if (_soldOutPants)
        {
            Debug.Log("<color=red>Mejora ya comprada</color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        //_player.SetArmor(ArmorPice.ArmorType.Leggings, armorQuality);
        _player.UpgradeArmor(ArmorPice.ArmorType.Leggings);
        _soldOutPants = true;
    }

    public void BuySword()
    {
        if (_weaponType == Weapon.WeaponType.Sword)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        _player.SetWeapon(Weapon.WeaponType.Sword);

        _weaponType = Weapon.WeaponType.Sword;
        //_playerMoney -= weaponCost;
    }
    public void BuyGreatSword()
    {
        if (_weaponType == Weapon.WeaponType.GreatSword)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        _player.SetWeapon(Weapon.WeaponType.GreatSword);

        _weaponType = Weapon.WeaponType.GreatSword;
        //_playerMoney -= weaponCost;
    }
    public void BuySandShield()
    {
        if (_weaponType == Weapon.WeaponType.SwordAndShield)
        {
            Debug.Log("<color=red> Arma ya equipada </color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        _player.SetWeapon(Weapon.WeaponType.SwordAndShield);

        _weaponType = Weapon.WeaponType.SwordAndShield;
        //_playerMoney -= weaponCost;
    }

    public void BuyPotion()
    {
        if (potionAmount <= 0)
        {
            Debug.Log("<color=red>Out of stock</color>");
            return;
        }
        if (!_player.PayAmount(potionCost))
        {
            Debug.Log("<color=yellow>Dinero insuficiente</color>");
            return;
        }
        _player.AddPotion();
        potionAmount--;
        //_playerMoney -= potionCost;
    }
}
