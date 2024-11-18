using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] GameObject mesh;
    [SerializeField] int _currentLevel;


    private void Start()
    {
        GameManager.Instance.OncombatWin += ActivateTrigger;
    }

    void ActivateTrigger()
    {
        mesh.SetActive(true);
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            Debug.Log("<color=magenta>Player Detectado</color>");
            GameManager.Instance.ChangeScenManager.CallSceneChange(_currentLevel + 1);

        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OncombatWin -= ActivateTrigger;
    }
}
