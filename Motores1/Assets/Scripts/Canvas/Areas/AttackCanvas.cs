using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCanvas : MonoBehaviour
{
    bool dentro = false;
    [SerializeField] GameObject centro;
    [SerializeField] BaseCharacter.AttackDirectionList _myAttackDir;

    

    BaseCharacter _myOwner;
    private float _myDamage;
    private float _myAttackSpeed;

    
    void Update()
    {
        if (dentro && Input.GetMouseButtonDown(1))
        {
            Block();            
        } 
    }

    public void StartAttack(BaseCharacter.AttackDirectionList attackDir, BaseCharacter attacker, float damage, float attackSpeed)
    {
        _myAttackDir = attackDir;
        _myDamage = damage;
        _myOwner = attacker;
        _myAttackSpeed = attackSpeed - 0.1f;

        StartCoroutine(DoDamage());
    }


    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(_myAttackSpeed);

        //Debug.Log($"<color=green>Attacando: {GameManager.Instance.Player}</color><color=red>{_myDamage}</color> dmg, direccion <color=cyan>{_myAttackDir}</color>");
        GameManager.Instance.Player.TakeDamage(_myDamage, _myAttackDir);
    }

    void Block()
    {
        if (GameManager.Instance.Player.outOfBreath) 
        {
            Debug.Log("<color=red> TOMATE UN RESPIRO </color>");
            return;
        } 
        Debug.Log("<color=red> BLOCK </color>");

        _myOwner.BuffAttackSpeed(0.9f);

        GameManager.Instance.Player.DoBlock();

        gameObject.SetActive(false);
        centro.SetActive(false);

        print("le diste al cuadrado");
    }

    public void ParryDetected(bool youKnow)
    {
        dentro = !youKnow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dentro = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dentro = false;
    }

    private void OnDisable()
    {
        dentro = false;
    }

}
