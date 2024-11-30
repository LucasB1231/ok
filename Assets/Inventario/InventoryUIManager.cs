using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    // Refer�ncias aos objetos de UI
    public GameObject mainWindow; // A janela inicial que aparece ao pressionar TAB
    public GameObject playerButton; // Bot�o com a foto do Player
    public GameObject allyButton1; // Bot�o com a foto do Aliado1
    public GameObject allyButton2; // Bot�o com a foto do Aliado2
    public GameObject inventoryPanel; // O painel de invent�rio � direita
    public GameObject playerImage; // Imagem do player na janela
    public GameObject ally1Image; // Imagem do aliado 1
    public GameObject ally2Image; // Imagem do aliado 2
    public GameObject characterImageDisplay; // Imagem do personagem � esquerda no invent�rio
    public GameObject[] equipmentSlots; // Array de GameObjects para slots de equipamento


    // Refer�ncia ao painel do invent�rio (esse painel � comum para todos os personagens)
    public GameObject inventoryItems; // Painel de itens no invent�rio

    private bool isWindowOpen = false; // Para controlar a abertura/fechamento da janela
    private GameObject currentCharacter; // Personagem que est� com o invent�rio aberto

    void Start()
    {
        mainWindow.SetActive(false); // A janela come�a fechada
        inventoryPanel.SetActive(false); // O painel de invent�rio come�a fechado

        // Inicialmente, o mouse est� oculto
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

            // Exibe os bot�es para escolher o personagem
            playerButton.SetActive(true);
            allyButton1.SetActive(true);
            allyButton2.SetActive(true);
            inventoryPanel.SetActive(false);

            // Tamb�m esconde as imagens dos personagens no invent�rio
            playerImage.SetActive(false);
            ally1Image.SetActive(false);
            ally2Image.SetActive(false);
        }
        else
        {
            // Desativa o mouse quando a janela for fechada
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Fecha o invent�rio e restaura os bot�es de sele��o
            CloseInventoryWindow();
        }
    }

    // Abre o invent�rio do Player
    public void OpenPlayerInventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de invent�rio
        playerImage.SetActive(true); // Exibe a imagem do Player

        // Remover os bot�es de sele��o de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);

        // Aqui voc� pode adicionar a l�gica para carregar os itens do invent�rio
        // Exemplo: inventoryItems.GetComponent<Inventory>().DisplayItems(character);
    }

    // Abre o invent�rio do Aliado 1
    public void OpenAlly1Inventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de invent�rio
        ally1Image.SetActive(true); // Exibe a imagem do Aliado 1

        // Remover os bot�es de sele��o de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);
    }

    // Abre o invent�rio do Aliado 2
    public void OpenAlly2Inventory(GameObject character)
    {
        currentCharacter = character;
        inventoryPanel.SetActive(true); // Exibe o painel de invent�rio
        ally2Image.SetActive(true); // Exibe a imagem do Aliado 2

        // Remover os bot�es de sele��o de personagem
        playerButton.SetActive(false);
        allyButton1.SetActive(false);
        allyButton2.SetActive(false);
    }

    // Fecha o invent�rio e a janela
    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);

        // Restaurar os bot�es para selecionar o personagem novamente
        playerButton.SetActive(true);
        allyButton1.SetActive(true);
        allyButton2.SetActive(true);

        // Esconde as imagens dos personagens
        playerImage.SetActive(false);
        ally1Image.SetActive(false);
        ally2Image.SetActive(false);
    }

    // Fun��o para abrir o invent�rio do Player
    public void OnPlayerButtonClicked()
    {
        OpenPlayerInventory(playerButton);
    }

    // Fun��o para abrir o invent�rio do Aliado 1
    public void OnAlly1ButtonClicked()
    {
        OpenAlly1Inventory(allyButton1);
    }

    // Fun��o para abrir o invent�rio do Aliado 2
    public void OnAlly2ButtonClicked()
    {
        OpenAlly2Inventory(allyButton2);
    }
    public void OnEquipmentSlotClicked(GameObject equipmentSlot)
    {
        // Aqui voc� pode implementar l�gica para remover o item do slot ou trocar com outro item do invent�rio.
        Debug.Log("Slot de equipamento clicado: " + equipmentSlot.name);
    }
    
}
