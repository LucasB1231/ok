using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    // Referências aos objetos de UI
    public GameObject mainWindow; // A janela inicial que aparece ao pressionar TAB
    public GameObject playerButton; // Botão com a foto do Player
    public GameObject allyButton1; // Botão com a foto do Aliado1
    public GameObject allyButton2; // Botão com a foto do Aliado2
    public GameObject inventoryPanel; // O painel de inventário à direita
    public GameObject playerImage; // Imagem do player na janela
    public GameObject ally1Image; // Imagem do aliado 1
    public GameObject ally2Image; // Imagem do aliado 2
    public GameObject characterImageDisplay; // Imagem do personagem à esquerda no inventário
    public GameObject[] equipmentSlots; // Array de GameObjects para slots de equipamento


    // Referência ao painel do inventário (esse painel é comum para todos os personagens)
    public GameObject inventoryItems; // Painel de itens no inventário

    private bool isWindowOpen = false; // Para controlar a abertura/fechamento da janela
    private GameObject currentCharacter; // Personagem que está com o inventário aberto

    void Start()
    {
        mainWindow.SetActive(false); // A janela começa fechada
        inventoryPanel.SetActive(false); // O painel de inventário começa fechado

        // Inicialmente, o mouse está oculto
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Ao pressionar TAB, abrir ou fechar a janela
        {
            ToggleMainWindow();
        }
    }

    // Alterna a janela inicial (abrir ou fechar)
    private void ToggleMainWindow()
    {
        isWindowOpen = !isWindowOpen;
        mainWindow.SetActive(isWindowOpen);

        if (isWindowOpen)
        {
            // Ativa o mouse quando a janela for aberta
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Exibe os botões para escolher o personagem
            playerButton.SetActive(true);
            allyButton1.SetActive(true);
            allyButton2.SetActive(true);
            inventoryPanel.SetActive(false);

            // Também esconde as imagens dos personagens no inventário
            playerImage.SetActive(false);
            ally1Image.SetActive(false);
            ally2Image.SetActive(false);
        }
        else
        {
            // Desativa o mouse quando a janela for fechada
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Fecha o inventário e restaura os botões de seleção
            CloseInventoryWindow();
        }
    }

    // Abre o inventário do Player
    public void OpenPlayerInventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de inventário
        playerImage.SetActive(true); // Exibe a imagem do Player

        // Remover os botões de seleção de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);

        // Aqui você pode adicionar a lógica para carregar os itens do inventário
        // Exemplo: inventoryItems.GetComponent<Inventory>().DisplayItems(character);
    }

    // Abre o inventário do Aliado 1
    public void OpenAlly1Inventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de inventário
        ally1Image.SetActive(true); // Exibe a imagem do Aliado 1

        // Remover os botões de seleção de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);
    }

    // Abre o inventário do Aliado 2
    public void OpenAlly2Inventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de inventário
        ally2Image.SetActive(true); // Exibe a imagem do Aliado 2

        // Remover os botões de seleção de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);
    }

    // Fecha o inventário e a janela
    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);

        // Restaurar os botões para selecionar o personagem novamente
        playerButton.SetActive(true);
        allyButton1.SetActive(true);
        allyButton2.SetActive(true);

        // Esconde as imagens dos personagens
        playerImage.SetActive(false);
        ally1Image.SetActive(false);
        ally2Image.SetActive(false);
    }

    // Função para abrir o inventário do Player
    public void OnPlayerButtonClicked()
    {
        OpenPlayerInventory(playerButton);
    }

    // Função para abrir o inventário do Aliado 1
    public void OnAlly1ButtonClicked()
    {
        OpenAlly1Inventory(allyButton1);
    }

    // Função para abrir o inventário do Aliado 2
    public void OnAlly2ButtonClicked()
    {
        OpenAlly2Inventory(allyButton2);
    }
    public void OnEquipmentSlotClicked(GameObject equipmentSlot)
    {
        // Aqui você pode implementar lógica para remover o item do slot ou trocar com outro item do inventário.
        Debug.Log("Slot de equipamento clicado: " + equipmentSlot.name);
    }
    
}
