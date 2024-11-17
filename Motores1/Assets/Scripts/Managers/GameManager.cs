using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        //_npc = null;
    }
    #endregion

    //Variables
    public CamMovement Camera;

    public Player Player;
    public Enemy Enemy;
    public CombatCanvas CombatCanvas;
    public CamRotation CamRotation;
    public GameObject mouseCenterGO;
    public BaseCharacter EnemyInCombat;
    public ShopCanvas Shop;

    [SerializeField] public float potionHealAmount;
    [SerializeField] public float poisonDamage;
    [SerializeField] public float bleedDamage;

    public delegate void VoidDelegate();
    public event VoidDelegate OnCombatEnter = delegate { }, OnCombatExit = delegate { };
    public event VoidDelegate OnShopActive = delegate { }, OnShopDisable = delegate { };
    public void EnterCombat()
    {
        //CombatCanvas.gameObject.SetActive(true);
        OnCombatEnter();
        
        //mover camara a zoom combate
        //lockear todos los enemigos
        //lockear player
        //activar canvas de combate
    }

    public void ExitCombat()
    {
        CombatCanvas.gameObject.SetActive(false);
        EnemyInCombat = null;
        OnCombatExit();
        //desactivar canvas de combate
        //liberar player
        //liberar enemigos
        //mover cam a zoom normal
    }

    public void EnterShop()
    {
        Shop.gameObject.SetActive(true);
        OnShopActive();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }   

    public void ExitShop()
    {
        Shop.gameObject.SetActive(false);
        OnShopDisable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
}
