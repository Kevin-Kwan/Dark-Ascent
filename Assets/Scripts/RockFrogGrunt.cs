/*
 * File: RockFrogGrunt.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles the AI, movement, and attack for the Rock throwing Frog Grunt
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using UnityEngine;

public class RockFrogGrunt : MonoBehaviour
{
    public enum State { Idle, MovingToRock, MovingToPlayer, Attacking }
    public State currentState = State.Idle;

    public float noticeDistance = 10f;
    public float attackDistance = 2f;
    public float walkSpeed = 2f;
    public GameObject rockPrefab;
    public float throwForce = 10f;
    public float rotationSpeed = 2f;
    public float throwCooldown = 1f;
    private float lastThrowTime;
    public AudioSource throwAudioSource;
    public AudioSource walkAudioSource;
    public AudioClip attackSound;
    public AudioClip walkSound;
    public AudioClip meleeSound;
    public Transform throwOrigin;
    private GameObject player;
    private GameObject nearestRock;
    public Animation animation;
    private Rigidbody rb;
    public float attackDamage = 10f;
    public float projectileLifespan = 3f;
    private bool isAttacking = false;

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
        Debug.Log(currentState);
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.MovingToRock:
                MoveTowardsNearestRock();
                break;
            case State.MovingToPlayer:
                MoveTowardsPlayer();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
        }
    }

    void Idle()
    {
        animation.Play("Idle");
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= noticeDistance)
        {
            nearestRock = FindNearestRock();
            if (nearestRock != null)
            {
                currentState = State.MovingToRock;
            }
            else
            {
                currentState = State.MovingToPlayer;
            }
        }
    }

    void MoveTowardsNearestRock()
    {
        animation.Play("Walk");
        nearestRock = FindNearestRock();
        if (nearestRock != null)
        {
            Vector3 directionToRock = (nearestRock.transform.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(directionToRock.x, 0, directionToRock.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            Vector3 direction = (nearestRock.transform.position - transform.position).normalized;
            transform.position += direction * walkSpeed * Time.deltaTime;

            float distanceToRock = Vector3.Distance(transform.position, nearestRock.transform.position);
            if (distanceToRock < attackDistance && Time.time >= lastThrowTime + throwCooldown)
            {
                currentState = State.Attacking;
            }
        }
        else
        {
            currentState = State.MovingToPlayer;
        }
    }

    void MoveTowardsPlayer()
    {
        if (isAttacking) return;
        animation.Play("Walk");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * walkSpeed * Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        if (distanceToPlayer < attackDistance && Time.time >= lastThrowTime + throwCooldown)
        {
            currentState = State.Attacking;
        }
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animation.Play("Attack1");

            if (nearestRock != null)
            {
                if (throwAudioSource.clip != attackSound || !throwAudioSource.isPlaying)
                {
                    throwAudioSource.clip = attackSound;
                    throwAudioSource.Play();
                }
                Destroy(nearestRock);
                Vector3 directionToPlayer = (player.transform.position - throwOrigin.position).normalized;
                GameObject p = Instantiate(rockPrefab, throwOrigin.position, transform.rotation);
                p.GetComponent<Rigidbody>().velocity = directionToPlayer * throwForce;
                Destroy(p, projectileLifespan);
            }
            else
            {
                if (throwAudioSource.clip != meleeSound || !throwAudioSource.isPlaying)
                {
                    throwAudioSource.clip = meleeSound;
                    throwAudioSource.Play();
                }
                player.GetComponent<ThirdPController>().takeDamage(attackDamage);  // Assuming attackDamage is defined
            }
            lastThrowTime = Time.time;
            Invoke("ResetAttack", animation["Attack1"].length);
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
    void ResetAttack()
    {
        isAttacking = false;
        currentState = State.Idle;
    }
}
