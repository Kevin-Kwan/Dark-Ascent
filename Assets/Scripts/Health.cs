/*
 * File: Health.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the health for the enemies in the game, and how they die as well.
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public GameObject damageEffect; // Prefab for a visual effect (e.g., particle system) to show when taking damage
    public GameObject deathEffect;  // Prefab for a visual effect (e.g., particle system) to show on death

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (damageEffect != null)
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
