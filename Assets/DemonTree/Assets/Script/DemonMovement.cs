using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DemonMovement : MonoBehaviour {
    //	private Animator anim;
    //	int hIdles;
    //	int hAngry;
    //	int hAttack;
    //	int hGrabs;
    //	int hThumbsUp;

    //	// Use this for initialization
    //	void Start () {
    //		anim = GetComponent<Animator> ();
    //		hIdles = Animator.StringToHash("Idles");
    //		hAngry = Animator.StringToHash("Angry");
    //		hAttack = Animator.StringToHash("Attack");
    //		hGrabs = Animator.StringToHash("Grabs");
    //		hThumbsUp = Animator.StringToHash("ThumbsUp");
    //	}

    //	// Update is called once per frame
    //	void Update () {
    //	        if (Input.GetKeyDown(KeyCode.W)) {
    //			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idles")) {
    //				anim.SetBool(hIdles, false);
    //				anim.SetBool(hAngry, true);
    //	             }
    //	        } else if (Input.GetKeyDown(KeyCode.S)) {
    //			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idles")) {
    //				anim.SetBool(hIdles, false);
    //				anim.SetBool(hAttack, true);
    //			    }
    //	        } else if (Input.GetKeyDown(KeyCode.A)) {
    //			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idles")) {
    //				anim.SetBool(hIdles, false);
    //				anim.SetBool(hGrabs, true);
    //			    }
    //			} else if (Input.GetKeyDown(KeyCode.D)) {
    //			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idles")) {
    //				anim.SetBool(hIdles, false);
    //				anim.SetBool(hThumbsUp, true);
    //			    }
    //		    } else {
    //			if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Idles")) {
    //				anim.SetBool(hIdles, true);
    //				anim.SetBool(hAngry, false);
    //				anim.SetBool(hAttack, false);
    //				anim.SetBool(hGrabs, false);
    //				anim.SetBool(hThumbsUp, false);
    //			}
    //		}
    //	}
    //public Transform player;  // Reference to the player's transform
    //public float chaseSpeed = 3.5f;  // Speed at which the warden chases player
    //public float verticalSpeed = 2.0f;  // Speed at which the warden adjusts height
    //public float heightThreshold = 2.0f;  // Minimum difference in height to trigger vertical movement
    //private NavMeshAgent navMeshAgent;
    //private float lerpTime = 0;

    //private void Start()
    //{
    //    navMeshAgent = GetComponent<NavMeshAgent>();
    //}

    //private void Update()
    //{
    //    // Check for null references to avoid runtime errors
    //    if (navMeshAgent == null || player == null) return;

    //    // Horizontal movement handled by NavMeshAgent
    //    navMeshAgent.speed = chaseSpeed;


    //    // Manual vertical adjustment
    //    float heightDifference = player.position.y - transform.position.y;
    //    Debug.Log(heightDifference);
    //    if (Mathf.Abs(heightDifference) > heightThreshold)
    //    {
    //        Debug.Log("Adjusting Height");
    //        navMeshAgent.enabled = false;
    //        lerpTime += Time.deltaTime * verticalSpeed;
    //        float newY = Mathf.Lerp(transform.position.y, player.position.y, lerpTime);
    //        Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
    //        transform.position = newPosition;
    //    }
    //    else
    //    {
    //        lerpTime = 0;  // Reset lerpTime when not adjusting height
    //        navMeshAgent.enabled = true;
    //        navMeshAgent.SetDestination(player.position);
    //    }
    //}
    public Transform player;
    public float maxSpeed = 30.0f;  // Increased speed
    public float maxForce = 60.0f;  // Increased force
    public float heightTolerance = 1.0f;
    public float rotationSpeed = 2.0f; // Speed of rotation towards the player
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 steerForce = Seek(player.position);
        AdjustHeight(player.position);

        rb.AddForce(steerForce, ForceMode.Force);
        LimitSpeed();

        FaceTarget(player.position); // Face towards the player
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
        directionToFace.y = 0;  // Maintain current elevation level
        Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Ensure the warden stays upright.
        if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}