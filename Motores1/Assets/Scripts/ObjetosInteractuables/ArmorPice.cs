using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TPFinal - Lorenzo Solari.
public class ArmorPice : MonoBehaviour, IPickeable

{
    #region Reemplazado por struct de armor
    //public enum ArmorType
    //{
    //    Helmet,
    //    Chestplate,
    //    Leggings
    //}

    //public enum ArmorQuality
    //{
    //    None,
    //    Low,
    //    Mid,
    //    High
    //}

    //[SerializeField] ArmorType armorType;
    //[SerializeField] ArmorQuality armorQuality; 
    #endregion

    [SerializeField] Armor thisArmor;
    public Armor Armor { get { return thisArmor; } private set { } }

    public void PickUp(BaseCharacter baseChar)
    {
        baseChar.SetArmor(thisArmor.type, thisArmor.quality);

        StartCoroutine(ActiveCosa(5f));
    }
    //


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

    //public override void BuyItem()
    //{
    //    switch(thisArmor.type)
    //    {
    //        case Armor.Type.Helmet:
    //            BuyHelmet();
    //            break;
    //        case Armor.Type.Chestplate:
    //            BuyChest();
    //            break;
    //        case Armor.Type.Leggings:
    //            BuyPants();
    //            break;
    //        default:
    //            Debug.Log("<color=red> TIPO DE ARMADURA COMPRADA INVALIDO </color>");
    //            break;
    //    }
    //}

    //public void BuyHelmet()
    //{
    //    if (_soldOut)
    //    {
    //        Debug.Log("<color=red>Mejora ya comprada</color>");
    //        return;
    //    }
    //    if (!GameManager.Instance.Player.PayAmount(price))
    //    {
    //        Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        return;
    //    }
    //    GameManager.Instance.Player.UpgradeArmor(Armor.Type.Helmet);
    //    _soldOut = true;
    //}
    //public void BuyChest()
    //{
    //    if (_soldOut)
    //    {
    //        Debug.Log("<color=red>Mejora ya comprada</color>");
    //        return;
    //    }
    //    if (!GameManager.Instance.Player.PayAmount(price))
    //    {
    //        Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        return;
    //    }
    //    GameManager.Instance.Player.UpgradeArmor(Armor.Type.Chestplate);
    //    _soldOut = true;
    //}
    //public void BuyPants()
    //{
    //    if (_soldOut)
    //    {
    //        Debug.Log("<color=red>Mejora ya comprada</color>");
    //        return;
    //    }
    //    if (!GameManager.Instance.Player.PayAmount(price))
    //    {
    //        Debug.Log("<color=yellow>Dinero insuficiente</color>");
    //        return;
    //    }
    //    GameManager.Instance.Player.UpgradeArmor(Armor.Type.Leggings);
    //    _soldOut = true;
    //}
}
