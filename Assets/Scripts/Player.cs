using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float clickDamage = 1f;
    public float critChance = 0.1f;
    public float autoDamage = 1f;
    public float autoAttackInterval = 1f;

    private float nextAutoAttackTime = 0f;
    private Animator anim;

    private int upgradeAttackLevel = 1;
    private int upgradeAttackCost = 1;
    private int upgradeCritLevel = 1;
    private int upgradeCritCost = 10;
    private int upgradeSpeedLevel = 1;
    private int upgradeSpeedCost = 1;

    public Button upgradeAttackButton;
    public Button upgradeCritButton;
    public Button upgradeSpeedButton;

    private TMP_Text upgradeAttackButtonText;
    private TMP_Text upgradeCritButtonText;
    private TMP_Text upgradeSpeedButtonText;

    private void Start()
    {
        anim = transform.Find("MainSprite").GetComponent<Animator>();

        upgradeAttackButtonText = upgradeAttackButton.GetComponentInChildren<TMP_Text>();
        upgradeCritButtonText = upgradeCritButton.GetComponentInChildren<TMP_Text>();
        upgradeSpeedButtonText = upgradeSpeedButton.GetComponentInChildren<TMP_Text>();

        // 버튼 클릭 리스너 설정
        upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        upgradeCritButton.onClick.AddListener(UpgradeCrit);
        upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);

        // 초기 텍스트 설정
        UpdateUpgradeButtonTexts();
    }

    void Update()
    {
        if (Time.time >= nextAutoAttackTime)
        {
            AutoAttack();
            nextAutoAttackTime = Time.time + autoAttackInterval;
        }
    }

    public void ChopWood(Wood wood)
    {
        float damage = clickDamage;
        anim.SetTrigger("Attack1");
        if (Random.value < critChance)
        {
            damage *= 1.5f;            
        }
        wood.TakeDamage(damage);
    }

    private void AutoAttack()
    {
        Wood wood = GameController.Instance.GetCurrentWood();
        if (wood != null)
        {
            anim.SetTrigger("Attack1");
            wood.TakeDamage(autoDamage);
        }
    }

    public void UpgradeAttack()
    {
        if (GameController.Instance.SpendMiniWood(upgradeAttackCost))
        {
            clickDamage += 1;        
            upgradeAttackCost += 1;
            upgradeAttackLevel += 1;
            UpdateUpgradeButtonTexts();
        }
    }

    public void UpgradeCrit()
    {
        if (GameController.Instance.SpendMiniWood(upgradeCritCost))
        {
            critChance += 0.05f;
            upgradeCritCost += 10;
            upgradeCritLevel += 1;
            UpdateUpgradeButtonTexts();
        }
    }

    public void UpgradeSpeed()
    {
        if (GameController.Instance.SpendMiniWood(upgradeSpeedCost))
        {            
            autoAttackInterval *= 0.9f;
            upgradeSpeedCost *= 2;
            upgradeSpeedLevel += 1;
            UpdateUpgradeButtonTexts();
        }
    }

    private void UpdateUpgradeButtonTexts()
    {
        upgradeAttackButtonText.text = $"LV. {upgradeAttackLevel} 공격력 증가 (나무: {upgradeAttackCost})";
        upgradeCritButtonText.text = $"LV. {upgradeCritLevel} 치명타 확률 증가 (나무: {upgradeCritCost})";
        upgradeSpeedButtonText.text = $"LV. {upgradeSpeedLevel} 자동 공격 속도 증가 (나무: {upgradeSpeedCost})";
    }
}
