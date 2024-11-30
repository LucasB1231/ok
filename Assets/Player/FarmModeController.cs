using UnityEngine;

public class FarmModeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float airControlMultiplier = 0.5f;
    public float jumpForce = 300f;
    public float dashForce = 10f;
    public float backDashForceMultiplier = 0.7f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public GameObject uiWindow; // Refer�ncia para a janela (UI) que ser� aberta/fechada
    public GameObject blueprintPrefab; // O prefab do blueprint
    public GameObject plantPrefab; // O prefab da planta final

    private Rigidbody rb;
    private bool isGrounded;
    private GameObject currentBlueprint; // Inst�ncia do blueprint
    private bool isBlueprintActive = false; // Controle se o blueprint est� ativo

    private bool isWindowOpen = false; // Controle do estado da janela

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (uiWindow != null)
        {
            uiWindow.SetActive(false); // Inicialmente, a janela estar� fechada
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();

        // Detecta quando a tecla TAB � pressionada para alternar a janela
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUIWindow();
        }

        // Detecta quando a tecla Q � pressionada para ativar/desativar o blueprint
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleBlueprint();
        }

        // Atualiza a posi��o do blueprint enquanto a c�mera se move
        if (isBlueprintActive && currentBlueprint != null)
        {
            UpdateBlueprintPosition();
        }

        // Coloca a planta final ao clicar com o mouse
        if (isBlueprintActive && Input.GetMouseButtonDown(0) && currentBlueprint != null)
        {
            PlacePlant();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        float currentMoveSpeed = isGrounded ? moveSpeed : moveSpeed * airControlMultiplier;

        rb.AddForce(moveDirection * currentMoveSpeed, ForceMode.Force);

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 moveDirection = cameraTransform.forward * Input.GetAxis("Vertical") +
                                    cameraTransform.right * Input.GetAxis("Horizontal");

            if (moveDirection.magnitude > 0)
            {
                rb.AddForce(moveDirection.normalized * dashForce, ForceMode.Impulse);
            }
            else
            {
                Vector3 backDash = -cameraTransform.forward;
                backDash.y = 0f;
                rb.AddForce(backDash.normalized * dashForce * backDashForceMultiplier, ForceMode.Impulse);
            }
        }
    }

    // Fun��o para alternar a visibilidade da janela (UI)
    private void ToggleUIWindow()
    {
        if (uiWindow != null)
        {
            Time.timeScale -= Time.timeScale;
            isWindowOpen = !isWindowOpen; // Alterna o estado da janela
            uiWindow.SetActive(isWindowOpen); // Define a visibilidade com base no estado
        }
    }

    // Fun��o para ativar/desativar o blueprint
    private void ToggleBlueprint()
    {
        isBlueprintActive = !isBlueprintActive;

        if (isBlueprintActive)
        {
            // Ativa o blueprint
            if (blueprintPrefab != null && currentBlueprint == null)
            {
                currentBlueprint = Instantiate(blueprintPrefab); // Cria o blueprint
            }
        }
        else
        {
            // Desativa o blueprint
            if (currentBlueprint != null)
            {
                Destroy(currentBlueprint); // Remove o blueprint
            }
        }
    }

    // Atualiza a posi��o do blueprint com base no raycast
    private void UpdateBlueprintPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Vector3 position = hit.point;
                position.y += 0.01f; // Ajuste para evitar sobreposi��o com o solo
                currentBlueprint.transform.position = position; // Atualiza a posi��o do blueprint
            }
        }
    }

    // Instancia a planta final no local do blueprint
    private void PlacePlant()
    {
        if (plantPrefab != null && currentBlueprint != null)
        {
            Instantiate(plantPrefab, currentBlueprint.transform.position, Quaternion.identity);
            Destroy(currentBlueprint); // Remove o blueprint ap�s a coloca��o da planta
            isBlueprintActive = false; // Desativa o modo de coloca��o
        }
    }
}
