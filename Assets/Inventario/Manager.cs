using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Refer�ncias aos objetos de UI
    public GameObject inventoryPanel; // O painel de invent�rio
    public GameObject itemButtonTemplate; // O template do bot�o do item (com imagem, nome)
    public Transform itemsGrid; // O container de itens (GridLayoutGroup)

    // Gerenciadores de slots para Player, Aliado 1 e Aliado 2
    public EquipmentSlotManager playerSlotsManager;
    public EquipmentSlotManager ally1SlotsManager;
    public EquipmentSlotManager ally2SlotsManager;

    public Item[] inventoryItems; // Array de itens que o jogador possui

    void Start()
    {
        DisplayInventoryItems();
    }

    // Exibe os itens no invent�rio
    public void DisplayInventoryItems()
    {
        // Certifique-se de que o painel de invent�rio est� ativado
        inventoryPanel.SetActive(true);

        // Limpa a grid antes de adicionar os novos itens
        foreach (Transform child in itemsGrid)
        {
            Destroy(child.gameObject);
        }

        // Adiciona os itens do invent�rio na grid
        foreach (Item item in inventoryItems)
        {
            // Cria uma nova inst�ncia do bot�o a partir do template
            GameObject itemButton = Instantiate(itemButtonTemplate, itemsGrid);
            itemButton.SetActive(true); // Torna o bot�o vis�vel

            // Acessa o script ItemObject do itemButton
            ItemObject itemObject = itemButton.GetComponent<ItemObject>();

            // Verifica se o ItemObject foi encontrado e chama o m�todo para definir os dados
            if (itemObject != null)
            {
                itemObject.SetItemData(item); // Atualiza a imagem e o nome do item
            }
            else
            {
                Debug.LogWarning("ItemObject n�o encontrado no bot�o.");
            }

            // Configura o comportamento do bot�o para equipar o item
            itemButton.GetComponent<Button>().onClick.AddListener(() => SelectOwnerAndEquip(item));
        }
    }

    // Permite selecionar o dono (Player, Aliado 1 ou Aliado 2) e equipar o item
    public void SelectOwnerAndEquip(Item item)
    {
        // Exemplo simples: Equipar no Player por padr�o
        // Pode ser substitu�do por l�gica de sele��o (ex: um menu para escolher o dono)
        EquipItemToSlot(item, "Player", 0); // Slot 0 do Player
    }

    // Equipar o item no slot do dono especificado
    public void EquipItemToSlot(Item item, string owner, int slotIndex)
    {
        switch (owner)
        {
            case "Player":
                playerSlotsManager.EquipItem(item, slotIndex);
                break;
            case "Ally1":
                ally1SlotsManager.EquipItem(item, slotIndex);
                break;
            case "Ally2":
                ally2SlotsManager.EquipItem(item, slotIndex);
                break;
            default:
                Debug.LogWarning("Dono inv�lido para o equipamento.");
                break;
        }

        // Remove o item do invent�rio
        RemoveItemFromInventory(item);

        // Atualiza a UI
        DisplayInventoryItems();
    }

    // Remove um item do invent�rio
    public void RemoveItemFromInventory(Item itemToRemove)
    {
        List<Item> updatedInventory = new List<Item>(inventoryItems);
        updatedInventory.Remove(itemToRemove);
        inventoryItems = updatedInventory.ToArray();
    }

    // Adiciona um novo item ao invent�rio
    public void AddItemToInventory(Item newItem)
    {
        // Cria uma nova lista com o novo item
        Item[] newInventory = new Item[inventoryItems.Length + 1];
        inventoryItems.CopyTo(newInventory, 0);
        newInventory[inventoryItems.Length] = newItem;
        inventoryItems = newInventory;

        // Atualiza o invent�rio exibido
        DisplayInventoryItems();
    }
}
