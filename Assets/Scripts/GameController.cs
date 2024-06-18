using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
        
    public GameObject[] woodPrefabs;
    public TMP_Text miniWoodNumText;
    public Button upgradeAttackButton;
    public Button upgradeCritButton;
    public Button upgradeSpeedButton;
    public Slider healthSlider;
    public GameObject startPopup;
        
    private int miniWoodCount = 0;
    private Wood currentWood;
    private Player player;
    private float treeHealthMultiplier = 1.0f;
    private int treesChopped = 0;
        
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        SetupUI();
        SpawnNewWood();
        UpdateMiniWoodCount();
        ShowStartPopup();
    }
    
    void Update()
    {
        if (IsPopupActive())
            return;

        if (Input.GetMouseButtonDown(0))
            HandleClick();
    }
        
    private void SetupUI()
    {
        upgradeAttackButton.onClick.AddListener(player.UpgradeAttack);
        upgradeCritButton.onClick.AddListener(player.UpgradeCrit);
        upgradeSpeedButton.onClick.AddListener(player.UpgradeSpeed);
    }
        
    private void ShowStartPopup()
    {
        startPopup.SetActive(true);
    }

    private bool IsPopupActive()
    {
        return startPopup.activeSelf;
    }
        
    private void HandleClick()
    {
        if (currentWood != null)
        {
            player.ChopWood(currentWood);
        }
    }
        
    public void AddMiniWood(int amount)
    {
        miniWoodCount += amount;
        UpdateMiniWoodCount();
    }

    public bool SpendMiniWood(int amount)
    {
        if (miniWoodCount >= amount)
        {
            miniWoodCount -= amount;
            UpdateMiniWoodCount();
            return true;
        }
        return false;
    }

    private void UpdateMiniWoodCount()
    {
        miniWoodNumText.text = miniWoodCount.ToString();
    }
        
    public Wood GetCurrentWood()
    {
        return currentWood;
    }

    private void SpawnNewWood()
    {
        if (currentWood != null)
        {
            Destroy(currentWood.gameObject);
        }

        int woodIndex = (treesChopped / 10) % woodPrefabs.Length; 
        GameObject woodObject = Instantiate(woodPrefabs[woodIndex]);
        currentWood = woodObject.GetComponent<Wood>();
        currentWood.SetHealthMultiplier(treeHealthMultiplier);
    }

    public void UpdateHealthSlider(float value)
    {
        if (healthSlider != null)
        {
            healthSlider.value = value;
        }
    }

    public void IncreaseTreeMultiplier()
    {
        treeHealthMultiplier += 0.1f;
        treesChopped++;
        SpawnNewWood();
    }
       
    public void SaveGame()
    {
        GameData data = new GameData
        {
            clickDamage = player.clickDamage,
            critChance = player.critChance,
            autoAttackInterval = player.autoAttackInterval,
            upgradeAttackLevel = player.upgradeAttackLevel,
            upgradeAttackCost = player.upgradeAttackCost,
            upgradeCritLevel = player.upgradeCritLevel,
            upgradeCritCost = player.upgradeCritCost,
            upgradeSpeedLevel = player.upgradeSpeedLevel,
            upgradeSpeedCost = player.upgradeSpeedCost,
            miniWoodCount = miniWoodCount,
            treesChopped = treesChopped,
            treeHealthMultiplier = treeHealthMultiplier
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            player.clickDamage = data.clickDamage;
            player.critChance = data.critChance;
            player.autoAttackInterval = data.autoAttackInterval;
            player.upgradeAttackLevel = data.upgradeAttackLevel;
            player.upgradeAttackCost = data.upgradeAttackCost;
            player.upgradeCritLevel = data.upgradeCritLevel;
            player.upgradeCritCost = data.upgradeCritCost;
            player.upgradeSpeedLevel = data.upgradeSpeedLevel;
            player.upgradeSpeedCost = data.upgradeSpeedCost;
            miniWoodCount = data.miniWoodCount;
            treesChopped = data.treesChopped;
            treeHealthMultiplier = data.treeHealthMultiplier;

            player.UpdateUpgradeButtonTexts();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

    public void StartNewGame()
    {
        startPopup.SetActive(false); 
        SpawnNewWood(); 
    }

    public void LoadSavedGame()
    {
        LoadGame();
        startPopup.SetActive(false); 
        SpawnNewWood(); 
    }
}
