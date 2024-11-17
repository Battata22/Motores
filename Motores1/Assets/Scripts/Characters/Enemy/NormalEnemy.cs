using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemy : Enemy
{
    protected override void Update()
    {
        if(inCombat && outOfBreath)
        {
            GameManager.Instance.ExitCombat();
            Debug.Log($"<color=magenta>{this.name} pateo a {_target.name}</color>");
            _target.GetKicked();
        }

        base.Update();
    }
}
