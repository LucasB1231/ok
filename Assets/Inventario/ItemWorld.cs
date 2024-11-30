using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;  // Refer�ncia ao ScriptableObject do item

    // Detecta a colis�o com o jogador ou aliado
    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu � o jogador ou aliado
        if (other.CompareTag("Player") || other.CompareTag("Ally"))
        {
            // Adiciona o item ao invent�rio do jogador
            Manager manager = FindObjectOfType<Manager>(); // Localiza o Manager na cena
            if (manager != null)
            {
                manager.AddItemToInventory(item);  // Adiciona o item ao invent�rio
            }

            // Destr�i o item no cen�rio
            Destroy(gameObject);
        }
    }
}
