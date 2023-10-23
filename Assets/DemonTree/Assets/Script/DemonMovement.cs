using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DemonMovement : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 30.0f;
    public float maxForce = 60.0f;
    public float heightTolerance = 1.0f;
    public float rotationSpeed = 2.0f;
    public float attackDistance = 3.0f;  // Distance at which the warden attacks
    public float stunDuration = 2.0f;  // Duration for which the warden is stunned
    private Rigidbody rb;
    private Animator anim;
    private float stunEndTime = 0f;  // Time at which stun ends

    private enum State { Chasing, Attacking, Stunned }
    private State currentState = State.Chasing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case State.Chasing:
                if (distanceToPlayer <= attackDistance)
                {
                    currentState = State.Attacking;
                    anim.SetBool("Attack", true);
                }
                else
                {
                    SeekAndFacePlayer();
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    currentState = State.Chasing;
                    anim.SetBool("Attack", false);
                }
                player.GetComponent<ThirdPController>().takeDamage(10);
                break;

            case State.Stunned:
                anim.SetBool("Angry", true);
                if (Time.time >= stunEndTime)
                {
                    currentState = State.Chasing;
                    anim.SetBool("Angry", false);
                }
                break;
        }
    }

    void SeekAndFacePlayer()
    {
        Vector3 steerForce = Seek(player.position);
        AdjustHeight(player.position);
        rb.AddForce(steerForce, ForceMode.Force);
        LimitSpeed();
        FaceTarget(player.position);
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity = (target - transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - rb.velocity;
        return Vector3.ClampMagnitude(steeringForce, maxForce);
    }

    void AdjustHeight(Vector3 target)
    {
        if (Mathf.Abs(transform.position.y - target.y) > heightTolerance)
        {
            float direction = Mathf.Sign(target.y - transform.position.y);
            rb.AddForce(new Vector3(0, direction, 0) * maxForce, ForceMode.Force);
        }
    }

    void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToFace = target - transform.position;
        directionToFace.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    // Assume this is called when the warden is hit by an attack that stuns
    public void GetStunned()
    {
        currentState = State.Stunned;
        stunEndTime = Time.time + stunDuration;
    }
}
