using UnityEngine;

public class FInalBoss : MonoBehaviour
{
    public Transform[] locations;  // Preset locations where the Warden can move
    public Transform summonLocations;  // Locations where grunts will be summoned
    public GameObject gruntPrefab;  // The grunt enemy prefab
    public Animator animator;  // Animator to control the Warden's animations
    private int currentLocationIndex = 0;
    public float summonInterval = 10f;  // Interval at which grunts are summoned
    private float lastSummonTime;
    public int maxNumEnemies = 10;  // Maximum number of enemies that can be alive at once

    void Start()
    {
        // Initialize lastSummonTime
        lastSummonTime = Time.time - summonInterval;
    }

    void Update()
    {
        // Check if it's time to summon grunts
        if (Time.time >= lastSummonTime + summonInterval)
        {
            SummonGrunts();
            lastSummonTime = Time.time;
        }

        // TODO: Add other behaviors such as moving, attacking, etc.
    }

    public void MoveToNextLocation()
    {
        // Move to the next location (you can add a check to ensure locations.Length > 0)
        currentLocationIndex = (currentLocationIndex + 1) % locations.Length;
        transform.position = locations[currentLocationIndex].position;

        // Trigger attack animation
        animator.SetTrigger("Attack");
    }

    public void SummonGrunts()
    {
        int numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (numEnemies < maxNumEnemies)
        {
            foreach (Transform summonLocation in summonLocations)
            {
                Instantiate(gruntPrefab, summonLocation.position, summonLocation.rotation);
            }
            animator.SetTrigger("Angry");
        }
    }

    // This method can be called when a weak spot is destroyed or the Warden takes damage
    public void OnDamage()
    {
        // TODO: Handle damage, check if the Warden should die, etc.
    }

    public void Die()
    {
        // TODO: Handle the Warden's death
    }
}

