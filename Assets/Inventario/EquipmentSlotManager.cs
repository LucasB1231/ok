using UnityEngine;
using System.Collections.Generic;

public class EquipmentSlotManager : MonoBehaviour
{
    public GameObject[] equipmentSlots; // Slots para o equipamento
    public string owner; // Nome ou tipo (Player, Aliado1, Aliado2)

    // Atualiza um slot com um item
    public void EquipItem(Item item, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equipmentSlots.Length)
        {
            Debug.LogWarning("Índice de slot inválido.");
            return;
        }

        GameObject slot = equipmentSlots[slotIndex];
        ItemObject itemObject = slot.GetComponent<ItemObject>();

        if (itemObject != null)
        {
            itemObject.SetItemData(item);
        }
    }

    // Remove o item de um slot
    public void UnequipItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equipmentSlots.Length)
        {
            Debug.LogWarning("Índice de slot inválido.");
            return;
        }

        GameObject slot = equipmentSlots[slotIndex];
        ItemObject itemObject = slot.GetComponent<ItemObject>();

        if (itemObject != null)
        {
            itemObject.ClearItemData(); // Função para limpar o slot
        }
    }
}
