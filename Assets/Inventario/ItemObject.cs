using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public Image itemImage;  // Refer�ncia � imagem do item
    public Text itemNameText;  // Texto que exibe o nome do item
    public Item assignedItem;
    // M�todo para definir os dados do item no prefab (item de grid)
    public void SetItemData(Item item)
    {
        if (itemImage != null)
        {
            itemImage.sprite = item.itemIcon;  // Atribui a imagem do item ao bot�o
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;  // Atribui o nome do item ao texto
        }
        assignedItem = item; // Armazena o item associado
        if (itemImage != null)
        {
            itemImage.sprite = item.itemIcon;
        }
        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
        }

    }
    public void ClearItemData()
    {
        if (itemImage != null)
        {
            itemImage.sprite = null; // Remove o �cone
        }

        if (itemNameText != null)
        {
            itemNameText.text = ""; // Limpa o nome do item
        }
    }
}


