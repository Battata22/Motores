using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    //[SerializeField] int potionCost;
    //[SerializeField] int potionAmount;
    //[SerializeField] Armor.Quality armorQuality;
    //[SerializeField] int armorCost;
    //[SerializeField] int weaponCost;

    [SerializeField] ItemComprable[] shopItems;

    //Player _player;
    //int _playerMoney;
    //Weapon.WeaponType _weaponType;

    //bool _soldOutChest = false, _soldOutHelmet = false, _soldOutPants = false;

    private void Start()
    {
        GameManager.Instance.Shop = this;
        gameObject.SetActive(false);
        shopItems = GetComponentsInChildren<ItemComprable>();
        foreach(var item in shopItems)
        {
            Debug.Log(item.name);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.ExitShop();
        }
    }

    public void BuyItem(int index)
    {
        if(index < 0 || index >= shopItems.Length)
        {
            Debug.Log("<color=red> OBJETO A COMPRAR NO ENCONTRADO </color>");
            return;
        }
        shopItems[index].BuyItem();
    }

    #region old shop
    //    //public void GetPlayerInfo(Player player, ref Weapon.WeaponType weaponType)
    //    //{
    //    //    _player = player;
    //    //    //_playerMoney = money;
    //    //    _weaponType = weaponType;

    //    //}

    //    public void BuyHelmet()
    //    {
    //        #region comment
    //        //if (_soldOutHelmet)
    //        //{
    //        //    Debug.Log("<color=red>Mejora ya comprada</color>");
    //        //    return;
    //        //}
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        ////_player.SetArmor(ArmorPice.ArmorType.Helmet, armorQuality);
    //        //_player.UpgradeArmor(Armor.Type.Helmet);
    //        //_soldOutHelmet = true; 
    //        #endregion

    //        BuyItem(GetArmorIndex(Armor.Type.Helmet));
    //    }
    //    public void BuyChest()
    //    {
    //        #region comment
    //        //if (_soldOutChest)
    //        //{
    //        //    Debug.Log("<color=red>Mejora ya comprada</color>");
    //        //    return;
    //        //}
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        ////_player.SetArmor(ArmorPice.ArmorType.Chestplate, armorQuality);
    //        //_player.UpgradeArmor(Armor.Type.Chestplate);
    //        //_soldOutChest = true; 
    //        #endregion

    //        BuyItem(GetArmorIndex(Armor.Type.Chestplate));
    //    }
    //    public void BuyPants()
    //    {
    //        #region comment
    //        //if (_soldOutPants)
    //        //{
    //        //    Debug.Log("<color=red>Mejora ya comprada</color>");
    //        //    return;
    //        //}
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        ////_player.SetArmor(ArmorPice.ArmorType.Leggings, armorQuality);
    //        //_player.UpgradeArmor(Armor.Type.Leggings);
    //        //_soldOutPants = true; 
    //        #endregion

    //        BuyItem(GetArmorIndex(Armor.Type.Leggings));
    //    }
    //    public void BuySword()
    //    {
    //        if (_weaponType == Weapon.WeaponType.Sword)
    //        {
    //            Debug.Log("<color=red> Arma ya equipada </color>");
    //            return;
    //        }
    //        #region comment
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        //_player.SetWeapon(Weapon.WeaponType.Sword);

    //        //_weaponType = Weapon.WeaponType.Sword; 
    //        #endregion

    //        BuyItem(GetWeaponIdex(Weapon.WeaponType.Sword));
    //    }
    //    public void BuyGreatSword()
    //    {
    //        if (_weaponType == Weapon.WeaponType.GreatSword)
    //        {
    //            Debug.Log("<color=red> Arma ya equipada </color>");
    //            return;
    //        }
    //        #region commnet
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        //_player.SetWeapon(Weapon.WeaponType.GreatSword);

    //        //_weaponType = Weapon.WeaponType.GreatSword;
    //        //_playerMoney -= weaponCost;

    //        #endregion

    //        BuyItem(GetWeaponIdex(Weapon.WeaponType.GreatSword));

    //    }
    //    public void BuySandShield()
    //    {
    //        if (_weaponType == Weapon.WeaponType.SwordAndShield)
    //        {
    //            Debug.Log("<color=red> Arma ya equipada </color>");
    //            return;
    //        }
    //        #region comment
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        //_player.SetWeapon(Weapon.WeaponType.SwordAndShield);

    //        //_weaponType = Weapon.WeaponType.SwordAndShield; 
    //        //_playerMoney -= weaponCost;
    //        #endregion

    //        BuyItem(GetWeaponIdex(Weapon.WeaponType.SwordAndShield));
    //    }
    //    public void BuyPotion()
    //    {
    //        #region comment
    //        //if (potionAmount <= 0)
    //        //{
    //        //    Debug.Log("<color=red>Out of stock</color>");
    //        //    return;
    //        //}
    //        //if (!_player.PayAmount(potionCost))
    //        //{
    //        //    Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        //    return;
    //        //}
    //        //_player.AddPotion();
    //        //potionAmount--;
    //        //_playerMoney -= potionCost; 
    //        #endregion

    //        BuyItem(GetItemIndex<Pocion>());
    //    }

    //    int GetArmorIndex(Armor.Type searching)
    //    {
    //        var index = -1;
    //        for (int i = 0; i < shopItems.Length; i++)
    //        {
    //            if (shopItems[i].transform.TryGetComponent<ArmorPice>(out var armor) && armor.Armor.type == searching)
    //                index = i;
    //        }

    //        return index;
    //    }

    //    int GetWeaponIdex(Weapon.WeaponType searching)
    //    {
    //        var index = -1;
    //        for (int i = 0; i < shopItems.Length; i++)
    //        {
    //            if (shopItems[i].transform.TryGetComponent<Weapon>(out var weapon) && weapon.weaponType == searching)
    //                index = i;
    //        }
    //        return index;
    //    }

    //    int GetItemIndex<T>()
    //    {
    //        var index = -1;

    //        for (int i = 0; i < shopItems.Length; i++)
    //        {
    //            if (shopItems is T)
    //                i = index;
    //        }

    //        return index;
    //    } 
    #endregion

}
