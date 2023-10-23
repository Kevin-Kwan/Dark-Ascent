using UnityEngine;

public class RockFrogGrunt : MonoBehaviour
{
    public float noticeDistance = 10f;
    public float attackDistance = 2f;
    public float walkSpeed = 2f;
    public GameObject rockPrefab;
    public float throwForce = 10f;
    public float rotationSpeed = 2f;
    public float throwCooldown = 1f;  // Cooldown duration in seconds
    private float lastThrowTime;
    public AudioSource throwAudioSource;
    public AudioSource walkAudioSource;
    public AudioClip attackSound;
    public AudioClip walkSound;
    public Transform throwOrigin;
    private GameObject player;
    private GameObject nearestRock;
    public Animation animation;  // Reference to the Animation component
    private Rigidbody rb;
    public float projectileLifespan = 3f;
    private GameObject targetRock;
    private bool hasRock = false;
    private bool isAtRock = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animation = GetComponent<Animation>();
        rb = GetComponent<Rigidbody>();
        lastThrowTime = -throwCooldown;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (distanceToPlayer <= noticeDistance)
        {
            //walkAudioSource.clip = walkSound;
            animation.Play("Walk");
            MoveTowardsNearestRock();
        }
        else
        {
            walkAudioSource.clip = null;
        }
    }

    void MoveTowardsNearestRock()
    {
        nearestRock = FindNearestRock();
        if (nearestRock != null)
        {
            Vector3 directionToRock = (nearestRock.transform.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(directionToRock.x, 0, directionToRock.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            Vector3 direction = (nearestRock.transform.position - transform.position).normalized;
            transform.position += direction * walkSpeed * Time.deltaTime;

            //walkAudioSource.Play();
            float distanceToRock = Vector3.Distance(transform.position, nearestRock.transform.position);
            if (distanceToRock < attackDistance && Time.time >= lastThrowTime + throwCooldown)
            {
                animation.Play("Attack1");
                Destroy(nearestRock);
                ThrowRock();
                lastThrowTime = Time.time;  // Update the time of the last throw
            }
        }
        else
        {
            animation.Play("Idle");
        }
    }

    GameObject FindNearestRock()
    {
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        GameObject closestRock = null;
        float closestDistance = noticeDistance;

        foreach (GameObject rock in rocks)
        {
            float distance = Vector3.Distance(transform.position, rock.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestRock = rock;
            }
        }

        return closestRock;
    }

    void ThrowRock()
    {
        if (throwAudioSource.clip != attackSound || !throwAudioSource.isPlaying)
        {
            throwAudioSource.clip = attackSound;
            throwAudioSource.Play();
        }
        Vector3 directionToPlayer = (player.transform.position - throwOrigin.position).normalized;
        GameObject p = Instantiate(rockPrefab, throwOrigin.position, transform.rotation);
        //throwAudioSource.clip = attackSound;
       // throwAudioSource.Play();
        p.GetComponent<Rigidbody>().velocity = directionToPlayer * throwForce;
        Destroy(p, projectileLifespan);
    }
}
