using System.Collections;
using UnityEngine;

public class BatGrunt: MonoBehaviour
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

    void Start()
    {
        animation = GetComponent<Animation>();
        originalY = transform.position.y;
        StartCoroutine(ShootCooldown());
    }

    void Update()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;

        if (distanceToPlayer > noticeDistance) return;  // Don't do anything if player is too far away

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
        //float theta = 360f / numProjectiles;
        //Quaternion spawnRotation = transform.rotation;
        //for (int i = 0; i < numProjectiles; i++)
        //{
        //    GameObject p = Instantiate(projectile, projectileSpawn.position, spawnRotation);
        //    p.GetComponent<Rigidbody>().velocity = p.transform.forward * projectileSpeed;
        //    Destroy(p, projectileLifespan);  // Destroy the projectile after a set duration
        //    spawnRotation *= Quaternion.Euler(0, theta, 0);
        //}
        // Calculate the direction to the player
        Vector3 playerCenter = player.position;
        //Vector3 playerCenter = player.position - new Vector3(0, player.GetComponentInChildren<CapsuleCollider>().height / 2, 0);
        Vector3 directionToPlayer = (playerCenter - projectileSpawn.position).normalized;
        // Create a projectile
        GameObject p = Instantiate(
        projectile,
        projectileSpawn.position,
        transform.rotation  // Flip the rotation by 180 degrees
    );

        // Set the projectile's velocity to move towards the player
        p.GetComponent<Rigidbody>().velocity = directionToPlayer * projectileSpeed;

        // Optionally, destroy the projectile after a set duration
        Destroy(p, projectileLifespan);

    }
}
