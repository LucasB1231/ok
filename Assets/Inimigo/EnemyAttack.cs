using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 0.5f; // Dist�ncia m�xima para atacar
    public float moveSpeed = 3f; // Velocidade de movimento
    public float attackDamage = 10f; // Dano causado pelo ataque
    public float attackCooldown = 1.5f; // Tempo entre ataques

    private float attackTimer = 0f; // Temporizador para controlar o cooldown
    public LayerMask targetLayers; // Camadas dos alvos (Player e Ally)

    private Transform target; // Alvo atual
    private Rigidbody rb; // Refer�ncia ao Rigidbody do inimigo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("O Rigidbody n�o est� anexado ao inimigo!");
        }
    }

    void Update()
    {
        // Atualiza o temporizador do ataque
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // Busca o alvo mais pr�ximo
        FindClosestTarget();

        // Move-se em dire��o ao alvo, se houver um
        if (target != null)
        {
            MoveTowardsTarget();

            // Realiza o ataque se poss�vel
            if (attackTimer <= 0 && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                AttackTarget();
            }
        }
    }

    void FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange * 5, targetLayers);

        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (Collider col in colliders)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = col.transform;
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Impede que o inimigo tente se mover verticalmente

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange)
        {
            Vector3 newVelocity = direction * moveSpeed;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z); // Mant�m o movimento apenas no eixo horizontal
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Para o inimigo quando estiver na dist�ncia certa
        }
    }

    void AttackTarget()
    {
        Debug.Log($"Atacando {target.name} e causando {attackDamage} de dano!");

        // Exemplo: Adicione l�gica para aplicar dano aqui

        attackTimer = attackCooldown; // Reinicia o cooldown
    }

    void OnDrawGizmosSelected()
    {
        // Desenha a �rea de ataque no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
