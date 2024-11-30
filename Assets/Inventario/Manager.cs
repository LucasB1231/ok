using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Referências aos objetos de UI
    public GameObject inventoryPanel; // O painel de inventário
    public GameObject itemButtonTemplate; // O template do botão do item (com imagem, nome)
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

    // Exibe os itens no inventário
    public void DisplayInventoryItems()
    {
        // Certifique-se de que o painel de inventário está ativado
        inventoryPanel.SetActive(true);

        // Limpa a grid antes de adicionar os novos itens
        foreach (Transform child in itemsGrid)
        {
            Destroy(child.gameObject);
        }

        // Adiciona os itens do inventário na grid
        foreach (Item item in inventoryItems)
        {
            // Cria uma nova instância do botão a partir do template
            GameObject itemButton = Instantiate(itemButtonTemplate, itemsGrid);
            itemButton.SetActive(true); // Torna o botão visível

            // Acessa o script ItemObject do itemButton
            ItemObject itemObject = itemButton.GetComponent<ItemObject>();

            // Verifica se o ItemObject foi encontrado e chama o método para definir os dados
            if (itemObject != null)
            {
                itemObject.SetItemData(item); // Atualiza a imagem e o nome do item
            }
            else
            {
                Debug.LogWarning("ItemObject não encontrado no botão.");
            }

            // Configura o comportamento do botão para equipar o item
            itemButton.GetComponent<Button>().onClick.AddListener(() => SelectOwnerAndEquip(item));
        }
    }

    // Permite selecionar o dono (Player, Aliado 1 ou Aliado 2) e equipar o item
    public void SelectOwnerAndEquip(Item item)
    {
        // Exemplo simples: Equipar no Player por padrão
        // Pode ser substituído por lógica de seleção (ex: um menu para escolher o dono)
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
                Debug.LogWarning("Dono inválido para o equipamento.");
                break;
        }

        // Remove o item do inventário
        RemoveItemFromInventory(item);

        // Atualiza a UI
        DisplayInventoryItems();
    }

    // Remove um item do inventário
    public void RemoveItemFromInventory(Item itemToRemove)
    {
        List<Item> updatedInventory = new List<Item>(inventoryItems);
        updatedInventory.Remove(itemToRemove);
        inventoryItems = updatedInventory.ToArray();
    }

    // Adiciona um novo item ao inventário
    public void AddItemToInventory(Item newItem)
    {
        // Cria uma nova lista com o novo item
        Item[] newInventory = new Item[inventoryItems.Length + 1];
        inventoryItems.CopyTo(newInventory, 0);
        newInventory[inventoryItems.Length] = newItem;
        inventoryItems = newInventory;

        // Atualiza o inventário exibido
        DisplayInventoryItems();
    }
}
