using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameObject[] woodPrefabs;
    public TMP_Text miniWoodNumText;
    public Button upgradeAttackButton;
    public Button upgradeCritButton;
    public Button upgradeSpeedButton;
    public Slider healthSlider;

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
        SpawnNewWood();
        UpdateMiniWoodCount();

        upgradeAttackButton.onClick.AddListener(player.UpgradeAttack);
        upgradeCritButton.onClick.AddListener(player.UpgradeCrit);
        upgradeSpeedButton.onClick.AddListener(player.UpgradeSpeed);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentWood == null)
            {                
                return;
            }
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

        int woodIndex = (treesChopped / 10) % woodPrefabs.Length; // 벤 나무 개수에 따라 프리팹 선택
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
}