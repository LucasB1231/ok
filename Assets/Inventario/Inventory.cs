using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Referências de UI
    public GameObject inventoryGrid; // O painel onde os itens serão colocados
    public GameObject itemPrefab; // O prefab do item
    public int maxItems = 20; // Número máximo de itens na grid (ajustar conforme necessário)

    void Start()
    {
        // Inicializa os itens ao carregar o inventário
        AddItemsToInventory();
    }

    // Função para adicionar itens à grid
    public void AddItemsToInventory()
    {
        // Limpa a grid antes de adicionar novos itens
        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Adiciona os itens ao painel
        for (int i = 0; i < maxItems; i++)
        {
            GameObject newItem = Instantiate(itemPrefab); // Cria um novo item usando o prefab
            newItem.transform.SetParent(inventoryGrid.transform, false); // Adiciona o item à grid
            // Aqui você pode adicionar lógica para configurar cada item (imagem, nome, etc.)
        }
    }
}
