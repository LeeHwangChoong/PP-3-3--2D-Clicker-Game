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

    public int upgradeAttackLevel = 1;
    public int upgradeAttackCost = 1;
    public int upgradeCritLevel = 1;
    public int upgradeCritCost = 10;
    public int upgradeSpeedLevel = 1;
    public int upgradeSpeedCost = 1;

    public Button upgradeAttackButton;
    public Button upgradeCritButton;
    public Button upgradeSpeedButton;

    private TMP_Text upgradeAttackButtonText;
    private TMP_Text upgradeCritButtonText;
    private TMP_Text upgradeSpeedButtonText;

    //public GameObject damageTextPrefab;

    private void Start()
    {
        anim = transform.Find("MainSprite").GetComponent<Animator>();

        upgradeAttackButtonText = upgradeAttackButton.GetComponentInChildren<TMP_Text>();
        upgradeCritButtonText = upgradeCritButton.GetComponentInChildren<TMP_Text>();
        upgradeSpeedButtonText = upgradeSpeedButton.GetComponentInChildren<TMP_Text>();
               
        upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        upgradeCritButton.onClick.AddListener(UpgradeCrit);
        upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);
                
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
        //ShowDamageText(damage, wood.transform.position);
    }

    private void AutoAttack()
    {
        if (GameController.Instance.startPopup.activeSelf) return;

        Wood wood = GameController.Instance.GetCurrentWood();
        if (wood != null)
        {
            anim.SetTrigger("Attack1");
            wood.TakeDamage(autoDamage);
            //ShowDamageText(autoDamage, wood.transform.position);
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

    public void UpdateUpgradeButtonTexts()
    {
        upgradeAttackButtonText.text = $"LV. {upgradeAttackLevel} ���ݷ� ���� (����: {upgradeAttackCost})";
        upgradeCritButtonText.text = $"LV. {upgradeCritLevel} ġ��Ÿ Ȯ�� ���� (����: {upgradeCritCost})";
        upgradeSpeedButtonText.text = $"LV. {upgradeSpeedLevel} �ڵ� ���� �ӵ� ���� (����: {upgradeSpeedCost})";
    }

    //private void ShowDamageText(float damage, Vector3 position)
    //{
    //    // ������ �ؽ�Ʈ ����
    //    GameObject damageText = Instantiate(damageTextPrefab);

    //    // ������ �ؽ�Ʈ ���� ����
    //    damageText.GetComponent<TextMeshPro>().text = damage.ToString();

    //    // �ؽ�Ʈ�� �ణ ���� �̵���Ű�� ���� ��ġ ����
    //    Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
    //    damageText.transform.position = screenPosition + new Vector3(0, 1, 0);

    //    // ���� �ð� �� �ؽ�Ʈ ����
    //    Destroy(damageText, 1.0f);
    //}
}