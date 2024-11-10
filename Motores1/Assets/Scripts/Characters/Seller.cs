using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : BaseCharacter
{
    public int money;

    //Enum en vez de List
    private enum SideQuestList
    {

    }
    private SideQuestList _sideQuestList;

    private bool _hasActiveQuest;

    public bool blueTeam;

    //Intems in shop
    public List<GameObject> sellingItems;
    public List<GameObject> selledItems;

    public void AssingQuest(Player newPlayer)
    {

    }

    public void OpenStorage()
    {

    }

    protected override void EnterCombat()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit += ExitCombat;
    }

    protected override void ExitCombat()
    {
        GameManager.Instance.OnCombatExit -= ExitCombat;
        GameManager.Instance.OnCombatEnter += EnterCombat;
    }
}
