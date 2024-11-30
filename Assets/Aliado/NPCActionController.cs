using UnityEngine;

public class NPCActionController : MonoBehaviour
{
    public float detectionRange = 15f;          // Alcance de detecção de inimigos
    public Transform player;                   // Referência ao jogador
    public AllyController movementScript;      // Script de movimentação
    public AllyAttack attackScript;            // Script de ataque
    public AllyStatus allyStatus;              // Referência ao script de status do aliado
    public float reviveTime = 5f;              // Tempo para reviver
    private float reviveTimer = 0f;            // Temporizador de reviver
    private bool isReviving = false;           // Flag para indicar estado de reviver
    private GameObject currentEnemy;

    void Update()
    {
        // Verifica se o aliado está morto
        if (allyStatus != null && allyStatus.isDead)
        {
            if (!isReviving)
            {
                StartReviveProcess();
            }
            return; // Sai do Update enquanto estiver no estado de reviver
        }

        // Detecta inimigos dentro do alcance
        DetectEnemy();

        // Se houver um inimigo próximo dentro do alcance de ataque, ativa o ataque
        if (currentEnemy != null && Vector3.Distance(transform.position, currentEnemy.transform.position) <= allyStatus.attackRange)
        {
            ActivateAttackScript();
        }
        else
        {
            // Caso contrário, ativa o script de movimentação para seguir o jogador
            ActivateMovementScript();
        }
    }

    void FixedUpdate()
    {
        if (isReviving)
        {
            reviveTimer += Time.deltaTime;

            if (reviveTimer >= reviveTime)
            {
                Revive();
            }
        }
    }

    void StartReviveProcess()
    {
        Debug.Log("Aliado abatido, iniciando temporizador para reviver...");
        isReviving = true;
        reviveTimer = 0f;

        // Desativa os scripts de movimento e ataque
        movementScript.enabled = false;
        attackScript.enabled = false;
    }

    public void Revive()
    {
        Debug.Log("Aliado reviveu!");

        // Restaura vida e atualiza o status
        allyStatus.health = allyStatus.maxHealth / 4; // Revive com 25% da vida
        allyStatus.UpdateUI();

        // Marca como revivido e encerra o processo de reviver
        isReviving = false;
        allyStatus.revive();

        // Reativa os scripts necessários
        movementScript.enabled = true;
        attackScript.enabled = false; // Começa em movimento, sem atacar inicialmente
    }

    // Detecta inimigos dentro do alcance de detecção
    void DetectEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange);

        currentEnemy = null;
        foreach (Collider col in enemies)
        {
            if (col.CompareTag("Inimigo"))
            {
                currentEnemy = col.gameObject;
                break; // Encontra o primeiro inimigo e sai do loop
            }
        }
    }

    // Ativa o script de ataque
    void ActivateAttackScript()
    {
        movementScript.enabled = false;
        attackScript.enabled = true;
        Debug.Log("Aliado atacando o inimigo!");
    }

    // Ativa o script de movimentação
    void ActivateMovementScript()
    {
        attackScript.enabled = false;
        movementScript.enabled = true;

        // Movimenta em direção ao jogador
        movementScript.MoveTowardsPlayer(player.position);
        Debug.Log("Aliado seguindo o jogador.");
    }
}
