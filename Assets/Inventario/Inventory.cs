using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Refer�ncias de UI
    public GameObject inventoryGrid; // O painel onde os itens ser�o colocados
    public GameObject itemPrefab; // O prefab do item
    public int maxItems = 20; // N�mero m�ximo de itens na grid (ajustar conforme necess�rio)

    void Start()
    {
        // Inicializa os itens ao carregar o invent�rio
        AddItemsToInventory();
    }

    // Fun��o para adicionar itens � grid
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
            newItem.transform.SetParent(inventoryGrid.transform, false); // Adiciona o item � grid
            // Aqui voc� pode adicionar l�gica para configurar cada item (imagem, nome, etc.)
        }
    }
}
