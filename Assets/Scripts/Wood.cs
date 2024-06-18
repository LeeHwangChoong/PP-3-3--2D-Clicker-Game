using UnityEngine;
using System.Collections;

public class Wood : MonoBehaviour
{
    public float baseHealth = 100f;
    private float currentHealth;
    private float healthMultiplier = 1.0f;
    private SpriteRenderer spriteRenderer;
    private bool isChopped = false;
    private AudioSource audioSource;
    public GameObject hitEffectPrefab;

    void Start()
    {
        spriteRenderer = transform.Find("WoodSprite").GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        ResetHealth();
    }

    public void SetHealthMultiplier(float multiplier)
    {
        healthMultiplier = multiplier;
        ResetHealth();
    }

    public void TakeDamage(float amount)
    {
        if (isChopped) return;

        currentHealth -= amount;
        GameController.Instance.UpdateHealthSlider(currentHealth / (baseHealth * healthMultiplier));

        if (audioSource != null)
        {
            audioSource.Play(); // 공격 받을 때 효과음 재생
        }

        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);            
        }

        if (currentHealth <= 0)
        {
            OnWoodChoppedDown();
        }
    }

    private void ResetHealth()
    {
        currentHealth = baseHealth * healthMultiplier;
        isChopped = false;
        GameController.Instance.UpdateHealthSlider(currentHealth / (baseHealth * healthMultiplier));
    }

    private void OnWoodChoppedDown()
    {
        if (isChopped) return;

        isChopped = true;
        int miniWoodCount = Random.Range(1, 3);
        GameController.Instance.AddMiniWood(miniWoodCount);
        StartCoroutine(FadeOutAndDestroy());        
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float duration = 1.0f;
        Color originalColor = spriteRenderer.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0, t / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // 최종 알파값 0 설정
        Destroy(gameObject);
        GameController.Instance.IncreaseTreeMultiplier();
    }
}
