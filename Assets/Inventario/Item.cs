using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Informações básicas do item
    public string itemName; // Nome do item
    public Sprite itemIcon; // Ícone do item
    public int quantity; // Quantidade do item

    // Estatísticas do item
    public float dano; // Dano do item
    public float danoMagico; // Dano mágico
    public float velocidadeMovimento; // Velocidade de movimento
    public float velocidadeAtaque; // Velocidade de ataque
    public float vidaRegenerada; // Vida regenerada
}
