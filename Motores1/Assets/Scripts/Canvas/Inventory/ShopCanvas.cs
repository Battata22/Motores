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
    int _playerMoney;
    Weapon.WeaponType _weaponType;

    bool _soldOutChest = false, _soldOutHelmet = false, _soldOutPants = false;

    private void Start()
    {
        GameManager.Instance.Shop = this;
        gameObject.SetActive(false);
    }

    public void GetPlayerInfo(ref int money, Player player, ref Weapon.WeaponType weaponType)
    {
        _player = player;
        _playerMoney = money;
        _weaponType = weaponType;

    }

    public void BuyHelmet()
    {
        if (_soldOutHelmet || !_player.PayAmount(armorCost)) return;
        _player.SetArmor(ArmorPice.ArmorType.Helmet, armorQuality);
        _soldOutHelmet = true;
    }
    public void BuyChest()
    {
        if (_soldOutChest || !_player.PayAmount(armorCost)) return;
        _player.SetArmor(ArmorPice.ArmorType.Chestplate, armorQuality);
        _soldOutChest = true;
    }
    public void BuyPants()
    {
        if (_soldOutPants || !_player.PayAmount(armorCost)) return;
        _player.SetArmor(ArmorPice.ArmorType.Leggings, armorQuality);
        _soldOutPants = true;
    }

    public void BuySword()
    {
        if (_weaponType == Weapon.WeaponType.Sword || !_player.PayAmount(weaponCost)) return;
        _player.SetWeapon(Weapon.WeaponType.Sword);

        _weaponType = Weapon.WeaponType.Sword;
        //_playerMoney -= weaponCost;
    }
    public void BuyGreatSword()
    {
        if (_weaponType == Weapon.WeaponType.GreatSword || !_player.PayAmount(weaponCost)) return;
        _player.SetWeapon(Weapon.WeaponType.GreatSword);

        _weaponType = Weapon.WeaponType.GreatSword;
        //_playerMoney -= weaponCost;
    }
    public void BuySandShield()
    {
        if (_weaponType == Weapon.WeaponType.SwordAndShield || !_player.PayAmount(weaponCost)) return;
        _player.SetWeapon(Weapon.WeaponType.SwordAndShield);

        _weaponType = Weapon.WeaponType.SwordAndShield;
        //_playerMoney -= weaponCost;
    }

    public void BuyPotion()
    {
        if(potionAmount <= 0 || !_player.PayAmount(potionCost)) return;
        _player.AddPotion();
        potionAmount--;
        //_playerMoney -= potionCost;
    }
}
