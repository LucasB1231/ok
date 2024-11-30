using UnityEngine;
using UnityEngine.UI;

public class AllyStatus : MonoBehaviour
{
    // Atributos de status do aliado
    public float attack;
    public float magicAttack;
    public float magicPower;       // Poder mágico
    public float health;
    public float maxHealth = 100f;
    public float armor;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;

    // Variáveis de Mana
    public float maxMana = 100f;
    public float currentMana;

    // Variáveis de Nível e XP
    public int level = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;

    // Referências aos sliders de HP e Mana para a UI
    public Slider healthSlider;
    public Slider manaSlider;

    // Referências às imagens do aliado
    public Image allyImage;
    public Sprite defaultSprite;
    public Sprite damageSprite;

    // Flag para status de combate
    public bool isInCombat = false;  // Controle de combate

    // Propriedade para verificar se o aliado está morto
    private bool _isDead = false;
    public bool isDead
    {
        get => _isDead;
        private set => _isDead = value;
    }

    void Start()
    {
        // Configurações iniciais
        health = maxHealth;
        currentMana = maxMana;
        allyImage.sprite = defaultSprite;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Atualiza os sliders de HP e Mana
        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }

        if (manaSlider != null)
        {
            manaSlider.value = currentMana / maxMana;
        }
    }
    public void revive()
    {

        isDead = false;
    }

    // Método para aplicar dano físico
    public void TakeDamage(float damage)
    {
        float finalDamage = damage - armor;
        finalDamage = Mathf.Max(0, finalDamage); // Garante que o dano mínimo seja 0
        health -= finalDamage;

        // Checa se o aliado está morto
        CheckIfDead();

        Debug.Log($"Dano recebido: {finalDamage}. HP restante: {health}");
        ChangeImageOnDamage();
        UpdateUI();
    }

    // Método para curar o aliado
    public void Heal(float healAmount)
    {
        health += healAmount;
        health = Mathf.Min(health, maxHealth); // Garante que não ultrapasse o máximo

        // Checa se o aliado está vivo novamente
        CheckIfDead();

        Debug.Log($"Aliado curado. HP atual: {health}");
        UpdateUI();
    }

    // Método para aplicar dano mágico
    public void TakeMagicDamage(float damage)
    {
        health -= damage;

        // Checa se o aliado está morto
        CheckIfDead();

        Debug.Log($"Dano mágico recebido: {damage}. HP restante: {health}");
        UpdateUI();
    }

    // Método para verificar se o aliado está morto
    private void CheckIfDead()
    {
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    // Método para mudar a imagem do aliado quando ele sofre dano
    private void ChangeImageOnDamage()
    {
        if (allyImage != null)
        {
            Debug.Log("Mudando imagem para dano...");
            allyImage.sprite = damageSprite;

            // Restaurar a imagem original após 0.5 segundos
            Invoke(nameof(RestoreOriginalImage), 0.5f);
        }
        else
        {
            Debug.LogError("AllyImage não está atribuído corretamente!");
        }
    }

    private void RestoreOriginalImage()
    {
        if (allyImage != null)
        {
            Debug.Log("Restaurando imagem original...");
            allyImage.sprite = defaultSprite;
        }
        else
        {
            Debug.LogError("AllyImage não está atribuído corretamente!");
        }
    }

    // Método para subir de nível e aumentar os atributos
    public void LevelUp()
    {
        level++;
        currentXP = 0;
        xpToNextLevel *= 1.2f;

        maxHealth += 10f;
        attack += 5f;
        magicAttack += 5f;
        magicPower += 5f;
        armor += 2f;
        moveSpeed += 0.2f;
        attackSpeed += 0.1f;
        attackRange += 0.1f;

        Debug.Log($"Aliado subiu para o nível {level}!");
        UpdateUI();
    }

    void Update()
    {
        // Regeneração de HP fora de combate
        if (!isInCombat && !isDead)
        {
            health += 0.1f * Time.deltaTime;
            health = Mathf.Min(health, maxHealth);
        }

        // Regeneração de Mana
        currentMana += 0.1f * Time.deltaTime;
        currentMana = Mathf.Min(currentMana, maxMana);

        // Teste manual para dano
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(20f);
        }

        UpdateUI();
    }
}
