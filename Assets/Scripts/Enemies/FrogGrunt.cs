/*
 * File: FrogGrunt.cs
 * Authors: Mehar Johal
 * Created: 09/18/2023
 * Modified: 10/28/2023
 * Description: This script handles AI, movement, and attacking for the base jumping Frog Grunt
 * Contributions:
 *  Mehar Johal - 
 *  (Sole contributor)
 */
using UnityEngine;

public class FrogGrunt : MonoBehaviour
{
    public float noticeDistance = 10f;
    public float attackDistance = 2f;
    public float jumpForce = 10f;
    public float walkSpeed = 2f;
    public int attackDamage = 10;
    public float hopHeight = 2f;
    public float rotationSpeed = 2f;
    public float hopForce = 5f;
    public float forwardHopForce = 3f;
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public Animation animation;  // Reference to the Animation component
    private Rigidbody rb;
    private GameObject player;
    private bool isGrounded = false;
    public float attackCooldown = 1f;  // Set a cooldown duration of 1 second (adjust as needed)
    private float lastAttackTime;  // Variable to store the timestamp of the last attack


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        animation = GetComponent<Animation>();  // Get the Animation component
                                                // Optionally add a check if player or animation is null, and log an error
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if (player == null) return;  // Exit if no player found

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= noticeDistance)
        {
            // Rotate to face the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            if (distanceToPlayer > attackDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                }
            }
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        animation.Play("Idle");
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (isGrounded)
        {
            //Vector3 force = new Vector3();
            rb.AddForce(Vector3.up * hopForce + direction * forwardHopForce, ForceMode.Impulse);
            PlaySound(jumpSound);
            isGrounded = false;
            // chase player here
            //Vector3 chaseDirection = player.transform.position - this.transform.position;
            //chaseDirection.Normalize();



            //Debug.Log("HEKLO");
            ////float forceX = Random.Range(-jumpHorizontalMaxForce, jumpHorizontalMaxForce);
            ////float forceY = jumpVerticalForce;
            ////float forceZ = Random.Range(-jumpHorizontalMaxForce, jumpHorizontalMaxForce);
            //float forceX = 10f;
            //float forceY = 10f;
            //float forceZ = 10f;

            //force.x = forceX * chaseDirection.x;
            //force.y = forceY;
            //force.z = forceZ * chaseDirection.z;
            //rb.AddForce(force);
            //// Hop towards player if far away
            ////transform.position = new Vector3(transform.position.x, transform.position.y + hopHeight, transform.position.z);
            ////transform.position += direction * walkSpeed * Time.deltaTime;
            //animation.Play("Jump");
            ////PlaySound(hopSound);
            //isGrounded = false;  // Set isGrounded to false assuming the hop will take the frog off the ground
        }
        else
        {
            // Walk towards player if close or if not grounded
            transform.position += direction * walkSpeed * Time.deltaTime;
            animation.Play("Walk");
        }
    }

    void AttackPlayer()
    {
        lastAttackTime = Time.time;
        player.GetComponent<ThirdPController>().takeDamage(attackDamage);
        animation.Play("Attack1");  // Updated to use Animation.Play with animation name
        PlaySound(attackSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Assumes you have a way to check if the frog is grounded, similar to your Chaser script
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Hey im grounded");
            isGrounded = true;
        }
    }
}
