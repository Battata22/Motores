using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCanvas : MonoBehaviour
{
    bool dentro = false;
    [SerializeField] GameObject centro;
    [SerializeField] BaseCharacter.AttackDirectionList _myAttackDir;
    Color _originalColor;

    

    public BaseCharacter myOwner;
    private float _myDamage;
    private float _myAttackSpeed;
    bool _isChargeAttack;

    
    void Update()
    {
        if (dentro && Input.GetMouseButtonDown(1))
        {
            Block();            
        } 
    }

    public void StartAttack(BaseCharacter.AttackDirectionList attackDir, BaseCharacter attacker, float damage, float attackSpeed, bool isChargeAttack)
    {
        _myAttackDir = attackDir;
        _myDamage = damage;
        myOwner = attacker;
        _myAttackSpeed = attackSpeed - 0.1f;
        _isChargeAttack = isChargeAttack;

        _originalColor = gameObject.GetComponent<RawImage>().color;

        if (isChargeAttack)
        {
            gameObject.GetComponent<RawImage>().color = Color.yellow;
            _myDamage += _myDamage * 0.25f;
        }

        StartCoroutine(DoDamage());
    }


    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(_myAttackSpeed);

        //Debug.Log($"<color=green>Attacando: {GameManager.Instance.Player}</color><color=red>{_myDamage}</color> dmg, direccion <color=cyan>{_myAttackDir}</color>");
        GameManager.Instance.Player.TakeDamage(_myDamage, _myAttackDir, _isChargeAttack);
        if (!_isChargeAttack)
            myOwner._myAttack();
        else
            myOwner._myChargeAttack();


        gameObject.GetComponent<RawImage>().color = _originalColor;
    }

    void Block()
    {
        if (GameManager.Instance.Player.outOfBreath) 
        {
            Debug.Log("<color=red> TOMATE UN RESPIRO </color>");
            return;
        } 
        if(_isChargeAttack)
        {
            Debug.Log("<color=yellow> NO SE PUEDE BLOQUEAR ATAQUE CARGADO </color>");
            return;
        }
        Debug.Log("<color=red> BLOCK </color>");

        myOwner.BuffAttackSpeed(0.9f);
        

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
