using UnityEngine;
using UnityEngine.AI;

public class AllyController : MonoBehaviour
{
    public Transform player; // Refer�ncia ao jogador
    public GameObject jumpTrigger; // Objeto que detecta paredes para pular
    public float moveSpeed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // For�a do pulo
    public float raycastDistance = 10f; // Dist�ncia do raycast para detectar obst�culos
    public float obstacleAvoidanceDistance = 5f; // Dist�ncia para tentar evitar obst�culos
    public float jumpDistance = 3f; // Dist�ncia para detectar se o NPC precisa pular antes de encostar na parede
    public AllyStatus allyStatus;  // Refer�ncia ao script AllyStatus
    

    //private float lastAttackTime = 0f; // Marca o tempo do �ltimo ataque
    public float attackRange = 10f;  // Dist�ncia de ataque
    public float attackCooldown = 1f;  // Delay entre ataques
    public GameObject projectilePrefab;  // O prefab do proj�til
    public float projectileSpeed = 10f;  // Velocidade do proj�til
    public Transform firePoint; // Ponto onde o proj�til ser� lan�ado (perto da m�o ou cabe�a do aliado)

    public GameObject[] nullObjects; // Lista de objetos de destino (NullObjects)
    public float stuckTimeThreshold = 3f; // Tempo ap�s o qual o NPC se considera preso
    private float stuckTimer = 0f; // Timer para o tempo preso

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    private bool isGrounded = true; // Flag para verificar se o NPC est� no ch�o

    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false; // Desabilita o NavMesh inicialmente
    }

    void Update()
    {
        if (allyStatus != null && allyStatus.health > 0 && allyStatus.currentMana > 0 ) // Defina sua pr�pria condi��o de combate
        {
            // Aliado em combate, desativar seguir o jogador
            if (agent.enabled)
            {
               
                agent.enabled = false;
            }
        }
        else
        {
            // Se n�o estiver em combate, continua seguindo o jogador

        }
        


        targetPosition = player.position;
        // Calcular dire��o do movimento
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        RotateJumpTrigger(directionToPlayer);

        // Detectar obst�culos entre o aliado e o jogador usando Raycast
        DetectRaycastObstacles();

        // Detectar obst�culos � frente e verificar a necessidade de pular
        DetectObstacles(directionToPlayer);

        // Movimentar o NPC em dire��o ao jogador
        MoveTowardsPlayer(directionToPlayer);

        // Verifica se o NPC ficou preso
        CheckIfStuck();
    }
    void RotateJumpTrigger(Vector3 directionToPlayer)
    {
        if (jumpTrigger != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // Garante que a rota��o em X e Z permane�a zerada
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            jumpTrigger.transform.rotation = Quaternion.Slerp(jumpTrigger.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }



    void DetectRaycastObstacles()
    {
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);
        if (heightDifference <= 0.5f)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = new Vector3(
    player.position.x - transform.position.x,
    0f, // Ignora o eixo Y
    player.position.z - transform.position.z
).normalized;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    ActivateNavMesh();
                }
                else
                {
                    DeactivateNavMesh();
                }
            }
            else
            {
                DeactivateNavMesh();
            }
        }
        else
        {
            DeactivateNavMesh();
        }
    }

    public void MoveTowardsPlayer(Vector3 direction)
    {
        if (!agent.enabled)
        {
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            agent.SetDestination(player.position);
        }
    }

    void DetectObstacles(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 directionFlat = new Vector3(direction.x, 0, direction.z).normalized;

        if (Physics.Raycast(transform.position, directionFlat, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                AvoidObstacle(hit);
            }
        }
    }

    void AvoidObstacle(RaycastHit hit)
    {
        Vector3 avoidDirection = Vector3.zero;

        Vector3 rightDirection = new Vector3(transform.right.x, 0, transform.right.z);
        Vector3 leftDirection = new Vector3(-transform.right.x, 0, -transform.right.z);

        if (!Physics.Raycast(transform.position, rightDirection, obstacleAvoidanceDistance))
        {
            avoidDirection = rightDirection;
        }

        if (avoidDirection == Vector3.zero && !Physics.Raycast(transform.position, leftDirection, obstacleAvoidanceDistance))
        {
            avoidDirection = leftDirection;
        }

        if (avoidDirection == Vector3.zero)
        {
            Jump();
        }
        else
        {
            rb.MovePosition(transform.position + avoidDirection * moveSpeed * Time.deltaTime);
        }
    }



    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void CheckIfStuck()
    {
        float distanceToTarget = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(targetPosition.x, 0, targetPosition.z)
        );

        if (distanceToTarget < 0.5f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer >= stuckTimeThreshold)
        {
            ActivateNavMesh();
        }
    }


    void ActivateNavMesh()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;

            // Tenta ajustar para o NavMesh
            if (!agent.isOnNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
                else
                {
                    Debug.LogWarning("N�o foi poss�vel alinhar ao NavMesh!");
                }
            }

            agent.SetDestination(player.position);
        }
    }


    void DeactivateNavMesh()
    {
        if (agent.enabled)
        {
            agent.enabled = false;
        }
    }

    GameObject FindNearestNullObject()
    {
        GameObject nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in nullObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < nearestDistance)
            {
                nearest = obj;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && isGrounded)
        {
            Jump();
        }
    }
}
