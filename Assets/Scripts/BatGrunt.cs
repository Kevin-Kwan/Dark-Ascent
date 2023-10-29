/*
 * File: BatGrunt.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the entirety of the flying BatGrunt Enemy including
 * movement, AI, and attacking the player as well as audio/visual polish
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using System.Collections;
using UnityEngine;

public class BatGrunt : MonoBehaviour
{
    public Transform player;
    public float hoverHeight = 2f;
    public float floatSpeed = 1f;
    public float floatAmplitude = 0.5f;
    public float moveSpeed = 2f;
    public float noticeDistance = 10f;
    public float shootDistance = 10f;
    public float minShootCooldownSeconds = 1.5f;
    public float maxShootCooldownSeconds = 3f;
    public GameObject projectile;
    public Transform projectileSpawn;
    public float projectileSpeed = 15f;
    public float projectileLifespan = 3f;
    public int numProjectiles = 16;
    private Animation animation;
    private float originalY;
    private bool canShoot = false;
    public AudioSource flyingAudioSource;
    public AudioSource attackAudioSource;
    public AudioClip attackClip;
    public AudioClip flyingClip;

    void Start()
    {
        animation = GetComponent<Animation>();
        originalY = transform.position.y;
        StartCoroutine(ShootCooldown());

        if (flyingClip != null)
        {
            flyingAudioSource.clip = flyingClip;
            flyingAudioSource.loop = true;  // Set the AudioSource to loop
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }

    void Update()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;

        if (distanceToPlayer > noticeDistance)
        {
            StopFlyingSound();
            return;  // Don't do anything if player is too far away
        }

        PlayFlyingSound();

        animation.Play("Idle");
        // Calculate the new y-position for the floating effect
        float newY = originalY + hoverHeight + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Remove the y-component of the direction to keep the bat at the hover height
        directionToPlayer.y = 0;

        // Calculate the new position of the bat
        Vector3 newPosition = transform.position + directionToPlayer * moveSpeed * Time.deltaTime;
        newPosition.y = newY;  // Set the y-position to the calculated floating y-position

        // Update the position of the bat
        transform.position = newPosition;

        // Optionally, make the bat face the player
        Vector3 lookDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookDirection);

        if (canShoot && distanceToPlayer < shootDistance)
        {
            canShoot = false;
            PlaySound(attackClip);
            Shoot();
            StartCoroutine(ShootCooldown());
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(Random.Range(minShootCooldownSeconds, maxShootCooldownSeconds));
        canShoot = true;
    }

    private void Shoot()
    {

        Vector3 playerCenter = player.position;
        Vector3 directionToPlayer = (player.transform.position - projectileSpawn.position).normalized;
        GameObject p = Instantiate(projectile, projectileSpawn.position, transform.rotation);
        p.GetComponent<Rigidbody>().velocity = directionToPlayer * projectileSpeed;
        Destroy(p, projectileLifespan);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            attackAudioSource.loop = false;
            attackAudioSource.clip = clip;
            attackAudioSource.Play();
        }
    }

    void PlayFlyingSound()
    {
        if (!flyingAudioSource.isPlaying)
        {
            flyingAudioSource.Play();  // Only play the sound if it's not already playing
        }
    }

    void StopFlyingSound()
    {
        if (flyingAudioSource.isPlaying)
        {
            flyingAudioSource.Stop();  // Only stop the sound if it's currently playing
        }
    }
}
