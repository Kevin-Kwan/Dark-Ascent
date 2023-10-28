using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownRock : MonoBehaviour
{
    public int damage = 10;  // Damage dealt by the rock

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assume the player has a method called TakeDamage
            collision.gameObject.GetComponent<ThirdPController>().takeDamage(damage);

            // Optionally, destroy the rock upon hitting the player
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Frog Grunt") && !collision.gameObject.CompareTag("Ground"))  // Assuming the frog has a tag "RockThrowingFrog"
        {
            // Optionally, destroy the rock if it hits anything else (not the player or the frog)
            Destroy(gameObject);
        }
    }
}
