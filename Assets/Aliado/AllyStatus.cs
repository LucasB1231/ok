using UnityEngine;
using UnityEngine.UI;

public class AllyStatus : MonoBehaviour
{
    // Atributos de status do aliado
    public float attack;
    public float magicAttack;
    public float magicPower;       // Poder m�gico
    public float health;
    public float maxHealth = 100f;
    public float armor;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;

    // Vari�veis de Mana
    public float maxMana = 100f;
    public float currentMana;

    // Vari�veis de N�vel e XP
    public int level = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;

    // Refer�ncias aos sliders de HP e Mana para a UI
    public Slider healthSlider;
    public Slider manaSlider;

    // Refer�ncias �s imagens do aliado
    public Image allyImage;
    public Sprite defaultSprite;
    public Sprite damageSprite;

    // Flag para status de combate
    public bool isInCombat = false;  // Controle de combate

    // Propriedade para verificar se o aliado est� morto
    private bool _isDead = false;
    public bool isDead
    {
        get => _isDead;
        private set => _isDead = value;
    }

    void Start()
    {
        // Configura��es iniciais
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

    // M�todo para aplicar dano f�sico
    public void TakeDamage(float damage)
    {
        float finalDamage = damage - armor;
        finalDamage = Mathf.Max(0, finalDamage); // Garante que o dano m�nimo seja 0
        health -= finalDamage;

        // Checa se o aliado est� morto
        CheckIfDead();

        Debug.Log($"Dano recebido: {finalDamage}. HP restante: {health}");
        ChangeImageOnDamage();
        UpdateUI();
    }

    // M�todo para curar o aliado
    public void Heal(float healAmount)
    {
        health += healAmount;
        health = Mathf.Min(health, maxHealth); // Garante que n�o ultrapasse o m�ximo

        // Checa se o aliado est� vivo novamente
        CheckIfDead();

        Debug.Log($"Aliado curado. HP atual: {health}");
        UpdateUI();
    }

    // M�todo para aplicar dano m�gico
    public void TakeMagicDamage(float damage)
    {
        health -= damage;

        // Checa se o aliado est� morto
        CheckIfDead();

        Debug.Log($"Dano m�gico recebido: {damage}. HP restante: {health}");
        UpdateUI();
    }

    // M�todo para verificar se o aliado est� morto
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

    // M�todo para mudar a imagem do aliado quando ele sofre dano
    private void ChangeImageOnDamage()
    {
        if (allyImage != null)
        {
            Debug.Log("Mudando imagem para dano...");
            allyImage.sprite = damageSprite;

            // Restaurar a imagem original ap�s 0.5 segundos
            Invoke(nameof(RestoreOriginalImage), 0.5f);
        }
        else
        {
            Debug.LogError("AllyImage n�o est� atribu�do corretamente!");
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
            Debug.LogError("AllyImage n�o est� atribu�do corretamente!");
        }
    }

    // M�todo para subir de n�vel e aumentar os atributos
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

        Debug.Log($"Aliado subiu para o n�vel {level}!");
        UpdateUI();
    }

    void Update()
    {
        // Regenera��o de HP fora de combate
        if (!isInCombat && !isDead)
        {
            health += 0.1f * Time.deltaTime;
            health = Mathf.Min(health, maxHealth);
        }

        // Regenera��o de Mana
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
