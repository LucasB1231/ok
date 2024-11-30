using UnityEngine;
using UnityEngine.AI;

public class AllyAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // O prefab do projétil
    public float attackRange = 5f; // Distância de ataque
    public Transform firePoint; // Ponto de onde o projétil será disparado (por exemplo, na mão ou arma do aliado)
    public float projectileSpeed = 10f; // Velocidade do projétil

    public float cooldownTime = 2f; // Tempo de recarga entre os ataques
    private float currentCooldown = 0f; // Contador do tempo de recarga

    private NavMeshAgent navMeshAgent; // Referência ao NavMeshAgent do aliado

    void Start()
    {
        // Obtém a referência ao NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Atualiza o temporizador do cooldown
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime; // Subtrai o tempo a cada quadro
        }

        AttackEnemy();
    }

    void AttackEnemy()
    {
        // Verifica se há inimigos dentro do alcance de ataque e se o cooldown terminou
        if (currentCooldown <= 0)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);

            foreach (Collider enemy in enemiesInRange)
            {
                // Se o inimigo tiver a tag "Inimigo", o aliado ataca
                if (enemy.CompareTag("Inimigo"))
                {
                    // Move o aliado em direção ao inimigo
                    navMeshAgent.SetDestination(enemy.transform.position);

                    // Se o aliado estiver perto o suficiente do inimigo, ataca
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
                    {
                        LaunchProjectile(enemy.transform);
                        currentCooldown = cooldownTime; // Reseta o temporizador de cooldown
                    }

                    break; // Apenas ataca um inimigo por vez
                }
            }
        }
    }

    void LaunchProjectile(Transform enemyTransform)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instancia o projétil na posição de disparo
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Calcula a direção para o inimigo
            Vector3 direction = (enemyTransform.position - firePoint.position).normalized;

            // Aplica a força no projétil
            Rigidbody rbProjectile = projectile.GetComponent<Rigidbody>();
            if (rbProjectile != null)
            {
                rbProjectile.AddForce(direction * projectileSpeed, ForceMode.VelocityChange);
            }

            // Log para debug
            Debug.Log("Atacando inimigo: " + enemyTransform.name);
        }
    }
}
