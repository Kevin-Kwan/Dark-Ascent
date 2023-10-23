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


    private void OnCollisionEnter(Collision collision)
    {
        var hp = collision.gameObject.GetComponentInParent<ThirdPController>();
        if (hp && hp.enabled)
        {
            Debug.Log(hp);
            hp.takeDamage(damage);
            Destroy(gameObject);    //destroy this projectile after 
        }
    }
}