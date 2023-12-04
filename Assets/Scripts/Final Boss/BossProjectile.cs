/*
 * File: BossProjectile.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the projectile for the final Warden Boss, from movement to collision detection. It is placed on the Final Boss Projectile prefab
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 direction;
    public bool isReflected = false;
    public AudioSource reflect;
    public GameObject reflectParticlePrefab;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit boss projectile");
        Debug.Log(collision.gameObject.name);
        if (!isReflected && collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Debug.Log(collision.gameObject.name);
            // Reflect the projectile when hit by the player's weapon
            Instantiate(reflectParticlePrefab, transform.position, Quaternion.identity);
            direction = -direction;
            isReflected = true;
        }
        else if (isReflected && collision.gameObject.CompareTag("FinalBoss"))
        {
            // If the reflected projectile hits the Warden's eye, inform the eye
            collision.gameObject.GetComponent<FInalBoss>().TakeDamage();
            Destroy(gameObject);  // Destroy the projectile
        }
        else if (!isReflected && collision.gameObject.CompareTag("Player"))
        {
            // If the reflected projectile hits the Warden's eye, inform the eye
            collision.gameObject.GetComponent<ThirdPController>().takeDamage(10);
            Destroy(gameObject);  // Destroy the projectile
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isReflected && other.CompareTag("PlayerWeapon"))
        {
            // Reflect the projectile when hit by the player's weapon
            direction = -direction;
            Instantiate(reflectParticlePrefab, transform.position, Quaternion.identity);
            isReflected = true;
            reflect.Play();
        }
        else if (isReflected && other.CompareTag("FinalBoss"))
        {
            // If the reflected projectile hits the Warden's eye, inform the eye
            other.GetComponent<FInalBoss>().TakeDamage();
            Destroy(gameObject);  // Destroy the projectile
        }
        else if (!isReflected && other.CompareTag("Player"))
        {
            // If the reflected projectile hits the Warden's eye, inform the eye
            other.GetComponent<ThirdPController>().takeDamage(10f);
            Destroy(gameObject);  // Destroy the projectile
        }
    }
    //void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("Hit boss projectile");
    //    if (!isReflected && other.gameObject.CompareTag("PlayerWeapon"))
    //    {
    //        // Reflect the projectile when hit by the player's weapon
    //        direction = -direction;
    //        isReflected = true;
    //    }
    //    else if (isReflected && other.gameObject.CompareTag("FinalBoss"))
    //    {
    //        // If the reflected projectile hits the Warden's eye, inform the eye
    //        other.gameObject.GetComponent<FInalBoss>().TakeDamage();
    //        Destroy(gameObject);  // Destroy the projectile
    //    }
    //}
}
