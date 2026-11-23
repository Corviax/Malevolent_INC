using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float health = 100f;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //Aqui se carga el HUD de muerte, no destruir al jugador
        Destroy(gameObject);
    }
}
