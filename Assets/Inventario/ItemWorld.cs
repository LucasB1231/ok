using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;  // Referência ao ScriptableObject do item

    // Detecta a colisão com o jogador ou aliado
    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu é o jogador ou aliado
        if (other.CompareTag("Player") || other.CompareTag("Ally"))
        {
            // Adiciona o item ao inventário do jogador
            Manager manager = FindObjectOfType<Manager>(); // Localiza o Manager na cena
            if (manager != null)
            {
                manager.AddItemToInventory(item);  // Adiciona o item ao inventário
            }

            // Destrói o item no cenário
            Destroy(gameObject);
        }
    }
}
