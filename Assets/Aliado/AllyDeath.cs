using UnityEngine;

public class AllyDeath : MonoBehaviour
{
    //public GameObject deathEffect;  // Efeito visual ao morrer
    public float deathDelay = 2f;  // Tempo antes de desaparecer

    void start()
    {
        Destroy(gameObject, deathDelay);
    }
}
