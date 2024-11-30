using UnityEngine;

public class PlayerModeController : MonoBehaviour
{
    public enum PlayerMode { Normal, Combat, Farmer } // Define os modos do jogador
    public PlayerMode currentMode = PlayerMode.Normal; // Modo inicial como Normal

    // Referências para os componentes do jogador
    public GameObject combatUI; // UI ou elementos relacionados ao combate
    public GameObject farmerUI; // UI ou elementos relacionados ao modo fazendeiro
    public GameObject normalUI; // UI ou elementos gerais do modo normal

    // Referências aos scripts de comportamento
    public PlayerController normalScript; // Script do modo Normal
    public AtackModeController combatScript; // Script do modo de Combate
    public FarmModeController farmerScript; // Script do modo Fazendeiro

    void Start()
    {
        // Inicializa no modo Normal
        SetMode(PlayerMode.Normal);
    }

    void Update()
    {
        // Alternar modos com base na tecla pressionada
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMode(PlayerMode.Combat);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMode(PlayerMode.Farmer);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) // Alterna de volta para o modo normal
        {
            SetMode(PlayerMode.Normal);
        }
    }

    void SetMode(PlayerMode newMode)
    {
        // Atualiza o modo atual
        currentMode = newMode;

        // Atualiza a UI
        UpdateModeUI();

        // Habilita o script correspondente ao modo atual
        EnableCurrentScript();
    }

    void UpdateModeUI()
    {
        // Ativar/desativar elementos de UI ou características do jogador
        if (combatUI != null) combatUI.SetActive(currentMode == PlayerMode.Combat);
        if (farmerUI != null) farmerUI.SetActive(currentMode == PlayerMode.Farmer);
        if (normalUI != null) normalUI.SetActive(currentMode == PlayerMode.Normal);
    }

    void EnableCurrentScript()
    {
        // Desativa todos os scripts de comportamento
        if (normalScript != null) normalScript.enabled = false;
        if (combatScript != null) combatScript.enabled = false;
        if (farmerScript != null) farmerScript.enabled = false;

        // Ativa apenas o script correspondente ao modo atual
        switch (currentMode)
        {
            case PlayerMode.Normal:
                if (normalScript != null) normalScript.enabled = true;
                Debug.Log("Modo Normal Ativado");
                break;

            case PlayerMode.Combat:
                if (combatScript != null) combatScript.enabled = true;
                Debug.Log("Modo de Combate Ativado");
                break;

            case PlayerMode.Farmer:
                if (farmerScript != null) farmerScript.enabled = true;
                Debug.Log("Modo Fazendeiro Ativado");
                break;
        }
    }
}
