using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCanvas : MonoBehaviour
{
    [SerializeField] RawImage[] _attackAreas; 

    private void Start()
    {
        GameManager.Instance.CombatCanvas = this;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        gameObject.SetActive(false);

    }

    public void ActivateDanger(BaseCharacter.AttackDirectionList attackDir)
    {
        foreach(var img in _attackAreas)
        {
            img.gameObject.SetActive(false);
        }
        _attackAreas[(int)attackDir].gameObject.SetActive(true);
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

        foreach(var image in _attackAreas)
        {
            image.enabled = false;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }

}
