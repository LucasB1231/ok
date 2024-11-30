using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Informa��es b�sicas do item
    public string itemName; // Nome do item
    public Sprite itemIcon; // �cone do item
    public int quantity; // Quantidade do item

    // Estat�sticas do item
    public float dano; // Dano do item
    public float danoMagico; // Dano m�gico
    public float velocidadeMovimento; // Velocidade de movimento
    public float velocidadeAtaque; // Velocidade de ataque
    public float vidaRegenerada; // Vida regenerada
}
