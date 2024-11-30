using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Para carregar cenas

public class CameraFadeplayer : MonoBehaviour
{
    public float speedScale = 1f; // Velocidade do fade
    public Color fadeColor = Color.black; // Cor do fade
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0)); // Curva de animação
    public bool startFadedOut = true; // Começa com a tela preta?
    public string sceneToLoad = "Mapa1"; // Nome da cena a ser carregada
    public float sceneLoadDelay = 1f; // Tempo de espera antes de carregar a cena

    private float alpha = 0f; // Transparência atual
    private Texture2D texture; // Textura usada para o fade
    private int direction = 0; // Direção do fade (1 = clareando, -1 = escurecendo)
    private float time = 0f; // Tempo usado na curva de animação
    private bool isLoadingScene = false; // Flag para evitar múltiplos carregamentos
    

    private void Start()
    {
        // Configuração inicial
        alpha = startFadedOut ? 1f : 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();

        // Inicia o fade inicial (escurecendo a tela se começar com fade in)
        if (!startFadedOut)
        {
            time = 0f; // Começa do ponto mínimo da curva
            direction = 1; // Escurecendo
        }
        else
        {
            time = 1f; // Começa do ponto máximo da curva
            direction = -1; // Clareando
        }
    }

    private void Update()
    {
        // Atualiza o fade conforme a direção
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Mathf.Clamp01(Curve.Evaluate(time)); // Garante que alpha fique entre 0 e 1
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();

            // Para o fade quando chega ao limite
            if (alpha <= 0f || alpha >= 1f)
            {
                direction = 0;

                // Apenas carrega a cena se estiver escurecendo e o fade terminou
                if (alpha >= 1f && isLoadingScene)
                {
                    StartCoroutine(DelayedLoadScene());
                }
            }
        }
    }

    private void OnGUI()
    {
        // Desenha a textura de fade
        if (alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
    }

    public void StartButton()
    {
        // Inicia o fade para escurecer a tela e prepara o carregamento da cena
        if (direction == 0)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;

            isLoadingScene = true;
            time = 0f; // Reinicia o tempo para o fade começar
            direction = 1; // Escurecendo

        }
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void TriggerFadeIn()
    {
        // Método para iniciar um fade in manualmente
        if (direction == 0 && alpha > 0f)
        {
            time = 1f; // Começa do ponto máximo
            direction = -1; // Clareando
        }
    }

    public void TriggerFadeOut()
    {
        // Método para iniciar um fade out manualmente
        if (direction == 0 && alpha < 1f)
        {
            time = 0f; // Começa do ponto mínimo
            direction = 1; // Escurecendo
        }
    }
}
