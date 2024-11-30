using UnityEngine;

public class NPCActionController : MonoBehaviour
{
    public float detectionRange = 15f;          // Alcance de detec��o de inimigos
    public Transform player;                   // Refer�ncia ao jogador
    public AllyController movementScript;      // Script de movimenta��o
    public AllyAttack attackScript;            // Script de ataque
    public AllyStatus allyStatus;              // Refer�ncia ao script de status do aliado
    public float reviveTime = 5f;              // Tempo para reviver
    private float reviveTimer = 0f;            // Temporizador de reviver
    private bool isReviving = false;           // Flag para indicar estado de reviver
    private GameObject currentEnemy;

    void Update()
    {
        // Verifica se o aliado est� morto
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

        // Se houver um inimigo pr�ximo dentro do alcance de ataque, ativa o ataque
        if (currentEnemy != null && Vector3.Distance(transform.position, currentEnemy.transform.position) <= allyStatus.attackRange)
        {
            ActivateAttackScript();
        }
        else
        {
            // Caso contr�rio, ativa o script de movimenta��o para seguir o jogador
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

        // Reativa os scripts necess�rios
        movementScript.enabled = true;
        attackScript.enabled = false; // Come�a em movimento, sem atacar inicialmente
    }

    // Detecta inimigos dentro do alcance de detec��o
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

    // Ativa o script de movimenta��o
    void ActivateMovementScript()
    {
        attackScript.enabled = false;
        movementScript.enabled = true;

        // Movimenta em dire��o ao jogador
        movementScript.MoveTowardsPlayer(player.position);
        Debug.Log("Aliado seguindo o jogador.");
    }
}
