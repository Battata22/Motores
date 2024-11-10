using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [SerializeField] public Vector3 point;
    [SerializeField] Transform _camHolder, _camCenter;
    Transform _followThis;

    public bool usePoint = false;
    bool inCombat = false;

    private void Start()
    {
        _followThis = _camHolder;
        GameManager.Instance.Camera = this;
        GameManager.Instance.OnCombatEnter += EnterCombat;

    }

    private void LateUpdate()
    {
        //el punto se lo da el CamDistance
        
        if (usePoint && !inCombat)
        {
            transform.position = point;
        }
        else
        {
            transform.position = _followThis.position;
        }

        transform.forward = _followThis.forward;
    }

    void EnterCombat()
    {
        _followThis = _camCenter;
        inCombat = true;

        GameManager.Instance.OnCombatExit += ExitCombat;
        GameManager.Instance.OnCombatEnter -= EnterCombat;
    }

    void ExitCombat()
    {
        _followThis = _camHolder;
        inCombat = false;

        GameManager.Instance.OnCombatEnter += EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatEnter -= EnterCombat;
        GameManager.Instance.OnCombatExit -= ExitCombat;
    }


}
