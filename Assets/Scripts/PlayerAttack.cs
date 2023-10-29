/*
 * File: PlayerAttack.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles player attack for the player, mainly collision detection and damage dealing
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayer;  // Set this to the enemy layer in the inspector
    public Collider attackCollider;  // Assign your attack collider in the inspector

    void Update()
    {
        // This is your existing attack input check
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Check for overlaps with enemy colliders
        Collider[] hitEnemies = Physics.OverlapSphere(attackCollider.bounds.center, attackCollider.bounds.extents.magnitude, enemyLayer);

        // Process each overlapped enemy
        foreach (Collider hitEnemy in hitEnemies)
        {
            // Assuming each enemy has a script with a TakeDamage method
            Health enemyScript = hitEnemy.GetComponent<Health>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(10);  // Assuming a damage value of 10
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
    }
}
