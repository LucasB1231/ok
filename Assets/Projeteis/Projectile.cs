using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;  // How long before the projectile is destroyed
    public int damage = 10;  // Damage dealt by the projectile

    private void Start()
    {
        Destroy(gameObject, lifetime);  // Destroy the projectile after a certain time
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Inimigo"))
        {
            // Apply damage to the enemy (you should have a method to handle damage in the enemy script)
            //collision.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);  // Destroy the projectile after hitting the enemy
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);  // Destroy the projectile when it hits the ground
        }
    }
}


