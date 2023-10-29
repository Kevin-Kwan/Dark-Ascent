using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private ParticleSystem onDeathParticles;


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
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