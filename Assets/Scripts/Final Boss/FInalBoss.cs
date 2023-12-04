/*
 * File: FinalBoss.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the AI, states, movement, and attack for the Final Boss
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
//using UnityEngine;

//public class FInalBoss : MonoBehaviour
//{
//    public Transform[] locations;  // Preset locations where the Warden can move
//    public Transform summonLocations;  // Locations where grunts will be summoned
//    public GameObject gruntPrefab;  // The grunt enemy prefab
//    public Animator animator;  // Animator to control the Warden's animations
//    private int currentLocationIndex = 0;
//    public float summonInterval = 10f;  // Interval at which grunts are summoned
//    private float lastSummonTime;
//    public int maxNumEnemies = 10;  // Maximum number of enemies that can be alive at once

//    void Start()
//    {
//        // Initialize lastSummonTime
//        lastSummonTime = Time.time - summonInterval;
//    }

//    void Update()
//    {
//        // Check if it's time to summon grunts
//        if (Time.time >= lastSummonTime + summonInterval)
//        {
//            SummonGrunts();
//            lastSummonTime = Time.time;
//        }

//        // TODO: Add other behaviors such as moving, attacking, etc.
//    }

//    public void MoveToNextLocation()
//    {
//        // Move to the next location (you can add a check to ensure locations.Length > 0)
//        currentLocationIndex = (currentLocationIndex + 1) % locations.Length;
//        transform.position = locations[currentLocationIndex].position;

//        // Trigger attack animation
//        animator.SetTrigger("Attack");
//    }

//    public void SummonGrunts()
//    {
//        int numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
//        if (numEnemies < maxNumEnemies)
//        {
//            foreach (Transform summonLocation in summonLocations)
//            {
//                Instantiate(gruntPrefab, summonLocation.position, summonLocation.rotation);
//            }
//            animator.SetTrigger("Angry");
//        }
//    }

//    // This method can be called when a weak spot is destroyed or the Warden takes damage
//    public void OnDamage()
//    {
//        // TODO: Handle damage, check if the Warden should die, etc.
//    }

//    public void Die()
//    {
//        // TODO: Handle the Warden's death
//    }
//}
//using System.Collections;
//using UnityEngine;

//public class FInalBoss : MonoBehaviour
//{
//    public Transform[] moveLocations;  // Assign the two locations here in the inspector
//    public Transform[] gruntSpawnLocations;  // Assign the grunt spawn locations here in the inspector
//    public GameObject gruntPrefab;
//    public GameObject projectilePrefab;
//    public Transform[] projectileSpawnPoints;
//    //public WardenEye[] eyes;  // Assign the eye weak spots here in the inspector
//    //public Animation anim;
//    public int health;
//    public float projectileSpeed = 10f;
//    public Transform player;
//    public int maxHealth = 2;
//    public float attackDuration = 3f;
//    public float angryDuration = 1f;
//    public float movementCooldown = 5f;
//    public float attackCooldown = 10f;
//    public float angryCooldown = 20f;

//    private int currentLocationIndex = 0;
//    private float lastAttackTime;
//    private float lastAngryTime;
//    private Animator anim;
//    private int attackCounter = 0;
//    public int attacksPerCycle = 3;  // Number of attacks before moving
//    public float sinkRiseSpeed = 5f;
//    public float sinkRiseDistance = 25f;
//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        health = maxHealth;
//        Rigidbody rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//        anim.SetTrigger("Front Switch");
//        StartCoroutine(Move());
//    }
//    private void Update()
//    {
//        LookAtPlayer();
//    }
//    private IEnumerator Move()
//    {
//        while (health > 0)
//        {
//            // Sink down
//            Vector3 sinkPosition = moveLocations[currentLocationIndex].position - new Vector3(0,25f,0);

//            while (transform.position.y > sinkPosition.y)
//            {
//                Debug.Log("Current Position: " + transform.position + ", Target Sink Position: " + sinkPosition);
//                Debug.Log("Sinking");
//                transform.position = Vector3.MoveTowards(transform.position, sinkPosition, 5f * Time.deltaTime);
//                LookAtPlayer();
//                yield return null;
//            }

//            // Switch to the other location
//            currentLocationIndex = 1 - currentLocationIndex;  // This toggles between 0 and 1
//            transform.position = new Vector3(moveLocations[currentLocationIndex].position.x, transform.position.y, moveLocations[currentLocationIndex].position.z);

//            // Rise up
//            Vector3 risePosition = moveLocations[currentLocationIndex].position;
//            while (transform.position.y < risePosition.y)
//            {
//                Debug.Log("Rising");
//                Debug.Log("Current Position: " + transform.position + ", Target Rise Position: " + risePosition);
//                transform.position = Vector3.MoveTowards(transform.position, risePosition, 5f * Time.deltaTime);
//                LookAtPlayer();
//                yield return null;
//            }

//            // Attack
//            attackCounter = 0;
//            while (attackCounter < attacksPerCycle)
//            {
//                anim.SetTrigger("Attack Trigger");  // Play attack animation
//                yield return new WaitForSeconds(attackDuration);
//                FireProjectiles();
//                attackCounter++;
//            }

//            yield return new WaitForSeconds(movementCooldown);
//        }
//    }
//    private IEnumerator SinkAndRise()
//    {
//        Vector3 startPosition = transform.position;
//        Vector3 sinkPosition = startPosition - new Vector3(0, sinkRiseDistance, 0);
//        float startTime;
//        float journeyLength;
//        float fracJourney;

//        // Sink
//        startTime = Time.time;
//        journeyLength = Vector3.Distance(startPosition, sinkPosition);
//        fracJourney = 0;
//        while (fracJourney < 1)
//        {
//            float distCovered = (Time.time - startTime) * sinkRiseSpeed;
//            fracJourney = distCovered / journeyLength;
//            transform.position = Vector3.Lerp(startPosition, sinkPosition, fracJourney);
//            yield return null;
//        }

//        // Change location
//        currentLocationIndex = 1 - currentLocationIndex;
//        Vector3 risePosition = moveLocations[currentLocationIndex].position;
//        startPosition = sinkPosition;

//        // Rise
//        startTime = Time.time;
//        journeyLength = Vector3.Distance(startPosition, risePosition);
//        fracJourney = 0;
//        while (fracJourney < 1)
//        {
//            float distCovered = (Time.time - startTime) * sinkRiseSpeed;
//            fracJourney = distCovered / journeyLength;
//            transform.position = Vector3.Lerp(startPosition, risePosition, fracJourney);
//            yield return null;
//        }
//    }
//    private void LookAtPlayer()
//    {
//        Vector3 direction = player.position - transform.position;
//        direction.y = 0;  // Keep rotation on the Y-axis only
//        transform.rotation = Quaternion.LookRotation(direction);
//    }

//    private void FireProjectiles()
//    {
//        foreach (Transform spawnPoint in projectileSpawnPoints)
//        {
//            Vector3 directionToPlayer = (player.position - spawnPoint.position).normalized;
//            GameObject projectileObject = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
//            BossProjectile projectile = projectileObject.GetComponent<BossProjectile>();
//            projectile.direction = directionToPlayer;
//            projectile.speed = projectileSpeed;
//        }
//    }

//    public void TakeDamage()
//    {
//        health--;
//        if (health <= 0)
//        {
//            Die();
//        }
//        //else if (health == 1 && Time.time > lastAngryTime + angryCooldown)
//        //{
//        //    StartCoroutine(GetAngry());
//        //}
//        anim.SetTrigger("Angry Trigger");
//        StartCoroutine(SpawnEnemiesAndMove());
//    }
//    private IEnumerator SpawnEnemiesAndMove()
//    {
//        // Stop the Move coroutine temporarily
//        StopCoroutine(Move());

//        foreach (Transform spawnLocation in gruntSpawnLocations)
//        {
//            Instantiate(gruntPrefab, spawnLocation.position, Quaternion.identity);
//        }

//        yield return new WaitForSeconds(angryDuration);  // Wait for a while before transitioning

//        //// Switch to the other location
//        //currentLocationIndex = 1 - currentLocationIndex;
//        //transform.position = moveLocations[currentLocationIndex].position;

//        // Resume the Move coroutine
//        StartCoroutine(Move());
//    }
//    private IEnumerator GetAngry()
//    {
//        lastAngryTime = Time.time;
//        //anim.SetBool("Angry", true);
//        yield return new WaitForSeconds(angryDuration);

//        foreach (Transform spawnLocation in gruntSpawnLocations)
//        {
//            Instantiate(gruntPrefab, spawnLocation.position, Quaternion.identity);
//        }
//    }

//    private void Die()
//    {
//        // Handle boss death, e.g., play death animation, destroy object, etc.
//        Debug.Log("Death to the boss");
//        //anim.Play("Death");
//        Destroy(gameObject);
//        //Destroy(gameObject, anim["Death"].length);  // Assumes the death animation is named "Death"
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FInalBoss : MonoBehaviour
{
    public Transform[] moveLocations;
    public Transform[] gruntSpawnLocations;
    public GameObject gruntPrefab;
    public GameObject projectilePrefab;
    public Transform[] projectileSpawnPoints;
    public GameObject portalPrefab;  // Assign your portal prefab here
    public int health;
    public float projectileSpeed = 10f;
    public Transform player;
    public int maxHealth = 100;
    public float attackDuration = 3f;
    public float angryDuration = 1f;
    public float movementCooldown = 5f;
    public float attackCooldown = 10f;
    public float angryCooldown = 20f;
    public float portalDuration = 2f;  // Adjust this value based on your portal effect duration
    public AudioSource attackAudioSource;
    public AudioSource spawnAudioSource;
    public AudioSource damagedAudioSource;
    public AudioSource teleportAudioSource;
    public DemonHealth bossHealthBar;

    private int currentLocationIndex = 0;
    private float lastAttackTime;
    private float lastAngryTime;
    private Animator anim;
    private int attackCounter = 0;
    public int attacksPerCycle = 3;
    public AudioSource winAudioSource;  // Assign your win audio source here
    public string nextSceneName;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StartCoroutine(Move());
        bossHealthBar.SetMaxHealth(100);
        bossHealthBar.SetHealth(100);

    }

    private void Update()
    {
        LookAtPlayer();
    }

    private IEnumerator Move()
    {
        while (health > 0)
        {
            // Teleport to the other location
            yield return StartCoroutine(Teleport());

            // Attack
            attackCounter = 0;
            while (attackCounter < attacksPerCycle)
            {
                anim.SetTrigger("Attack Trigger");
                attackAudioSource.Play();
                FireProjectiles();
                attackCounter++;
                yield return new WaitForSeconds(attackDuration);
            }

            yield return new WaitForSeconds(movementCooldown);
        }
    }

    private IEnumerator Teleport()
    {
        // Instantiate portal at current position
        GameObject entryPortal = Instantiate(portalPrefab, transform.position + new Vector3(0,4f,0), Quaternion.identity);

        // Wait for portal to fully open (assuming portalDuration is the time it takes)
        yield return new WaitForSeconds(portalDuration);

        // Update location index
        currentLocationIndex = (currentLocationIndex + 1) % moveLocations.Length;

        // Move boss to new location (hidden from view)
        transform.position = moveLocations[currentLocationIndex].position;
        teleportAudioSource.Play();
        // Instantiate portal at new position
        GameObject exitPortal = Instantiate(portalPrefab, transform.position + new Vector3(0, 4f, 0), Quaternion.identity);

        // Optionally wait for portal to fully open at the new location
        yield return new WaitForSeconds(portalDuration);

        // Clean up portal game objects after they're done
        Destroy(entryPortal);
        Destroy(exitPortal);
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void FireProjectiles()
    {
        foreach (Transform spawnPoint in projectileSpawnPoints)
        {
            Vector3 directionToPlayer = (player.position - spawnPoint.position).normalized;
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            BossProjectile projectile = projectileObject.GetComponent<BossProjectile>();
            projectile.direction = directionToPlayer;
            projectile.speed = projectileSpeed;
        }
    }

    public void TakeDamage()
    {
        health-=34;
        bossHealthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
        
        StartCoroutine(SpawnEnemiesAndMove());
    }

    private IEnumerator SpawnEnemiesAndMove()
    {
        StopCoroutine(Move());
        anim.SetTrigger("Angry Trigger");
        spawnAudioSource.Play();
        foreach (Transform spawnLocation in gruntSpawnLocations)
        {
            Instantiate(gruntPrefab, spawnLocation.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(angryDuration);
        StartCoroutine(Move());
    }

    private IEnumerator GetAngry()
    {
        lastAngryTime = Time.time;
        yield return new WaitForSeconds(angryDuration);
        foreach (Transform spawnLocation in gruntSpawnLocations)
        {
            Instantiate(gruntPrefab, spawnLocation.position, Quaternion.identity);
        }
    }

    private void Die()
    {
        Debug.Log("Death to the boss");

        winAudioSource.Play();  // Play the win sound
        StartCoroutine(TransitionToNextScene());  // Start the scene transition coroutine
    }

    private IEnumerator TransitionToNextScene()
    {
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds or for however long you want
        // clear PlayerPrefs for CurrentLevelIndex to allow for restart progress
        PlayerPrefs.DeleteKey("CurrentLevelIndex");
        SceneManager.LoadScene(nextSceneName);  // Load the next scene
    }
}
