using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    // Variáveis de Vida
    public float maxHealth = 100f;
    public float currentHealth;

    // Variáveis de Mana
    public float maxMana = 100f;
    public float currentMana;
    public float manaRegenRate = 5f; // Quantidade regenerada por segundo

    // Variáveis de Status
    public float attack = 10f;
    public float magicAttack = 15f;
    public float armor = 5f;
    public float moveSpeed = 5f;
    public float attackSpeed = 1f;
    public float attackRange = 1f;

    // Variáveis de Nível e XP
    public int level = 1;
    public float currentXP = 0f;
    public float xpToNextLevel = 100f;

    // Referências de UI
    public Slider healthSlider;
    public Slider manaSlider;
    public Image characterImage;
    public Sprite normalSprite;
    public Sprite damageSprite;
    public float damageSpriteDuration = 0.5f;

    // Tecla para teste
    public KeyCode testDamageKey = KeyCode.Space;
    public float testDamageAmount = 10f;

    private void Start()
    {
        // Inicialização de valores
        currentHealth = maxHealth;
        currentMana = maxMana;
        UpdateUI();
    }

    private void Update()
    {
        // Teste de dano
        if (Input.GetKeyDown(testDamageKey))
        {
            TakeDamage(testDamageAmount);
        }

        // Regeneração de vida e mana
        RegenerateStats();
    }

    private void RegenerateStats()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += maxHealth * 0.01f * Time.deltaTime; // Regenera 1% da vida máxima por segundo
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }

        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        float effectiveDamage = Mathf.Max(0, damage - armor); // Reduz dano pela armadura
        currentHealth -= effectiveDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (characterImage != null)
        {
            ShowDamageSprite();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        if (manaSlider != null)
        {
            manaSlider.value = currentMana / maxMana;
        }
    }

    private void ShowDamageSprite()
    {
        if (characterImage != null)
        {
            characterImage.sprite = damageSprite;
            CancelInvoke(nameof(ResetToNormalSprite));
            Invoke(nameof(ResetToNormalSprite), damageSpriteDuration);
        }
    }

    private void ResetToNormalSprite()
    {
        if (characterImage != null)
        {
            characterImage.sprite = normalSprite;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} morreu.");
        Destroy(gameObject);
    }

    public void GainXP(float amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel *= 1.5f; // Incremento exponencial no XP necessário

        // Melhorias de status ao subir de nível
        maxHealth += 10f;
        maxMana += 5f;
        attack += 2f;
        magicAttack += 3f;
        armor += 1f;
        moveSpeed += 0.5f;
        attackSpeed += 0.1f;

        currentHealth = maxHealth; // Recupera a vida total ao subir de nível
        currentMana = maxMana; // Recupera a mana total

        Debug.Log($"Subiu para o nível {level}! Status melhorados.");
        UpdateUI();
    }
}
