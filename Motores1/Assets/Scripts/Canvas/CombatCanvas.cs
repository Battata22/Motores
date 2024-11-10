using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCanvas : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.CombatCanvas = this;
        GameManager.Instance.OnCombatEnter += EnterCombat;

        gameObject.SetActive(false);

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
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }

}
