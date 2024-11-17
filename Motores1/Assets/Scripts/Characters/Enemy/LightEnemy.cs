using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : Enemy
{

    [Header("<color=green>Light Enemy Setings</color>")]
    [SerializeField, Range(0, 100)] int _poisonChance;
    [SerializeField] float _poisonDuration;
    [SerializeField, Range(0, 100)] int _bleedChance;
    [SerializeField] float _bleedDuration;

    protected override void Awake()
    {
        base.Awake();
        _myAttack += ChanceOfPoison;
        _myAttack += ChanceOfBleed;
    }

    void ChanceOfPoison()
    {
        if (Random.Range(1, 101) > _poisonChance) return;
        _target.StartPoison(_poisonDuration);
    }

    void ChanceOfBleed()
    {
        if (Random.Range(1, 101) > _bleedChance) return;
        _target.StartPoison(_bleedDuration);
    }
}
