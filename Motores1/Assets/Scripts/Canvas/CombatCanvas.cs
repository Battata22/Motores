using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCanvas : MonoBehaviour
{
    [SerializeField] RawImage[] _attackAreas, _centros;

    //Las partes mal hechas fueron creadas por Gianluca :)

    //TPFinal - Gianluca Battaglino.

    /// <summary>
    /// USAR COMO EVENTO, SOLO USAR += o -=
    /// </summary>

    private void Start()
    {
        GameManager.Instance.CombatCanvas = this;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        gameObject.SetActive(false);

    }

    public void ActivateDanger(BaseCharacter.AttackDirectionList attackDir, BaseCharacter attacker, float damage, float attackSpeed, bool isChargeAttack)
    {
        foreach (var img in _attackAreas)
        {
            img.gameObject.SetActive(false);
        }
        foreach (var img in _centros)
        {
            img.gameObject.SetActive(false);
        }

        _attackAreas[(int)attackDir].gameObject.SetActive(true);
        _attackAreas[(int)attackDir].gameObject.GetComponent<AttackCanvas>().StartAttack(attackDir, attacker, damage, attackSpeed, isChargeAttack);

        _centros[(int)attackDir].gameObject.SetActive(true);

        //direccion de ataque 
        //player lo saaca del GM
        //Si necesitamos comportamiento de recover necesita enemigo que llamo al atque, lo podes pedir por parametro en esta funcion
    }

    void EnterCombat()
    {
        gameObject.SetActive(true);


        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;
    }

    void ExitCombat()
    {
        gameObject.SetActive(false);

        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        //foreach(var image in _attackAreas)
        //{
        //    image.enabled = false;
        //}
        //foreach (var image in _centros)
        //{
        //    image.enabled = false;
        //}
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }

}
