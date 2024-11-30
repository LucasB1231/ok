using UnityEngine;

public class PlayerController : MonoBehaviour
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

    public GameObject uiWindow; // Referência para a janela (UI) que será aberta/fechada

    private Rigidbody rb;
    private bool isGrounded;

    private bool isWindowOpen = false; // Controle do estado da janela

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (uiWindow != null)
        {
            uiWindow.SetActive(false); // Inicialmente, a janela estará fechada
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();

        // Detecta quando a tecla TAB é pressionada para alternar a janela
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUIWindow();
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

    // Função para alternar a visibilidade da janela (UI)
    private void ToggleUIWindow()
    {
        if (uiWindow != null)
        {
            Time.timeScale -= Time.timeScale;
            isWindowOpen = !isWindowOpen; // Alterna o estado da janela
            uiWindow.SetActive(isWindowOpen); // Define a visibilidade com base no estado
        }
    }
}
