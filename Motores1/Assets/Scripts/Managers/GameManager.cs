using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carranza Gonzalo

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
    public ChangeSceneManager ChangeScenManager;

    [SerializeField] public float potionHealAmount;
    [SerializeField] public float poisonDamage;
    [SerializeField] public float bleedDamage;

    public delegate void VoidDelegate();
    public event VoidDelegate OnCombatEnter = delegate { }, OnCombatExit = delegate { }, OncombatWin = delegate { };
    public event VoidDelegate OnShopActive = delegate { }, OnShopDisable = delegate { };

    public Dictionary<string, float> runStats = new Dictionary<string, float>();

    #region Update temporal para debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ShowRunStats();
        }
    }
    #endregion
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

    public void WinCombat()
    {
        OncombatWin();
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

    public void AddToRunStats(string key, float value)
    {
        if(!runStats.ContainsKey(key))
            runStats.Add(key, value);
        else
            runStats[key] += value;
    }

    public void ShowRunStats()
    {
        foreach (var stat in runStats)
        {
            Debug.Log($"<color=yellow> {stat.Key}: {stat.Value} </color>");
        }
    }
    
}
