/*
 * File: ThirdPController.cs
 * Authors: Kevin Kwan, Akhilesh Sivaganesan, Mehar Johal, Connor Sugasawa, Amal Chaudry
 * Created: 09/18/2022
 * Modified: 10/20/2023
 * Description: This script handles the movement of the player's game object in the third-person perspective.
 * Camera movement is also handled here as well as player animations.
 * Contributions:
 *   Kevin Kwan:
 *     - Created basic character controller featuring walking, running, and jumping allowing for smooth, responsible input
 *     - Implemented 3rd Person Camera control similar to Roblox's 
 *     - Using Scripting, Mechanim, and Animator Layers, Blend Trees, States, and Transitions:
 *       - Implemented animations for walking, running, sliding, jumping, and falling
 *       - Implemented animations for attacking, taking damage, and death
 *   Akhilesh Sivaganesan:
 *     - Implemented wall jumping
 *     - Character switches direction when wall jumping
 *   Mehar Johal:
 *     - Implemented double jumping
 *     - Player can jump multiple times in the air based on maxJumps
 *     - Player jumps reset when they land on the ground
 *   Connor Sugasawa:
 *     - Implemented sliding
 *     - Sliding speed decays over time
 *     - Sliding makes hitbox smaller
 *   Amal Chaudry:
 *    - Implemented elevator interaction with the player controller
 *    - Player can now stand on moving platforms and move with them
 *    - Player can jump off of moving platforms
 */ 

using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// This script is used to control the player's game object in the third-person perspective.
public class ThirdPController : MonoBehaviour
{
    public CharacterController controller;

    // for moving platforms
    private Transform currentPlatform = null;

    // the Main Camera in the scene
    public Transform camera;
    public CinemachineFreeLook freeLookCamera;
    public float speed = 6.0f;
    public float runSpeed = 12.0f;

    private float currentSpeed = 0.0f;

    // parameters for smooth turning
    public float turnSmoothingTime = 0.1f;
    float turnSmoothingVelocity;

    // parameters for jumping
    public float jumpHeight = 1.0f;
    public float jumpAdjustment = -2.0f;
    public float gravity = -9.81f;
    private int jumpCount = 0;
    public int maxJumps = 2;
    public Vector3 playerVelocity;

    public bool bhopEnabled = false;

    // parameters for wall jumping
    private bool canWallJump;
    private Vector3 wallNormal;
    public float jumpHeightScale = 4.5f;

    // parameters for sliding
    public float maxWalkDrift = 4f;
    public float maxSprintDrift = 1f;
    public float driftDecay = 0.02f;
    public float crouchedHeight = 1.0f;
    private float standingHeight;

    // animator
    public Animator animator;
    private CharacterInputController cinput;
    float _inputForward = 0f;
    float _inputTurn = 0f;
    public float speedChangeRate = 0.01f;
    private float currentVelY = 0f;
    private float targetVelY = 0f;

    // death animation (temporary)
    public GameObject ghostBody;
    public float floatingSpeed = 0.25f;

    // player stats
    public float health = 100f;
    public float previousHealth = 100f;
    public float maxHealth = 100f;
    public float pushPower = 3.0f;

    // grounded animation
    private bool grounded = false;
    // attack animation
    private bool inAttack = false;

    // not used atm
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float attackDamage = 10f;
    public GameObject weapon;
    public float invincibilityTime = 1f;
    public float previousDamageTime = 0f;
    public bool tookDamage = false;

    // audio
    public AudioSource walkAudio;
    public AudioSource sprintAudio;
    public AudioSource slideAudio;
    public float slideAudioFactor = 12f;
    public AudioSource jumpAudio;
    public AudioSource wallJumpAudio;
    public AudioSource swingAudio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen.
        standingHeight = controller.height;
        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
        weapon.SetActive(false);
        animator.SetBool("isDead", false);

    }

    // Update is called once per frame
    void Update() {
        // death animation
        if (health <=0) {
            Debug.Log("Player is dead");
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetBool("isDead", true);
            // floating animation
            ghostBody.transform.position = new Vector3(ghostBody.transform.position.x, ghostBody.transform.position.y + floatingSpeed * Time.deltaTime, ghostBody.transform.position.z);
            controller.enabled = false;
            // handle case if player clicks a menu item or hits restart before this return
            // we prevent the player from moving after death
            // unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            // disable this script
            // this.enabled = false;
            return;
        } else {
            animator.SetLayerWeight(1, 1);
            animator.SetBool("isDead", false);
        }
        // attacking animation
        if (Input.GetButtonDown("Fire1") && !inAttack) 
        {
            inAttack = true;
            // enable the weapon
            weapon.SetActive(true);
            // weapon.GetComponent<Weapon>().Attack();
            animator.SetBool("isAttacking", true);
            swingAudio.Play();
        }
        if (inAttack)
        {
            AnimatorStateInfo attackStateInfo = animator.GetCurrentAnimatorStateInfo(1);
            // Check if the attack animation is done playing
            if (attackStateInfo.IsName("Attack") && attackStateInfo.normalizedTime >= 1 && !animator.IsInTransition(1)) {
                animator.SetBool("isAttacking", false);
                weapon.SetActive(false);
                inAttack = false;
            }
        }
        // taking damage animation
        if (tookDamage) {
            animator.SetBool("tookDamage", true);
            animator.SetLayerWeight(2, 1);
        } else {
            AnimatorStateInfo damageStateInfo = animator.GetCurrentAnimatorStateInfo(2); 
            // Check if the damage animation is done playing
            if (damageStateInfo.IsName("TakeDamage") && damageStateInfo.normalizedTime >= 1 && !animator.IsInTransition(2)) {
                animator.SetBool("tookDamage", false);
                animator.SetLayerWeight(2, 0);
            }
        }
        // Debug.Log(isGrounded);
        // Debug.Log(controller.isGrounded);
        // We are using GetAxisRaw in case the player is using a controller.
        // Not tested yet.
        // if holding right click
        if (Input.GetButton("Fire2")) {
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        } else {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
        if (controller.isGrounded && playerVelocity.y < 0) {
            grounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            playerVelocity.y = 0f;
            jumpCount = 0;
            canWallJump = false;
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;
        } else {
            grounded = false;
            animator.SetBool("isGrounded", false);
            if (playerVelocity.y < 0) {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            } else if (playerVelocity.y > 0) {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
        }
        if (cinput.enabled) {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        var animState = animator.GetCurrentAnimatorStateInfo(0);
        // animator.SetFloat("velX", _inputTurn);

        // absolute value of _inputForward and _inputTurn
        if (_inputForward < 0f) {
            _inputForward = - _inputForward;
        }
        if (_inputTurn < 0f) {
            _inputTurn = - _inputTurn;
        }

        targetVelY = Mathf.Max(_inputForward, _inputTurn);

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f) // if no input, stop applying movement
        {
            // Movement and also handles jumping while moving
            // This if section might need to be moved to a new function and changed if bhop or other movement mechanics are added.


            // Calculate the angle between the player's input direction and the positive x-axis.
            // This angle is then used to rotate the player's game object so that it faces the direction of movement.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            // Smoothly rotate the player's game object to face the direction of movement.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothingTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // if Jumping while moving
            if (controller.isGrounded || jumpCount < maxJumps) {
                //Debug.Log(jumpCount);
                if (bhopEnabled) {
                    if (Input.GetButton("Jump") && controller.transform.parent != null) {
                        controller.transform.SetParent(null);
                    }
                    if (Input.GetButton("Jump") && controller.isGrounded) {
                        Jump();
                        jumpCount++;
                    }
                    else if (Input.GetButtonDown("Jump") && jumpCount < maxJumps) {
                        Jump();
                        jumpCount++;
                    }
                } else {
                    if (Input.GetButtonDown("Jump")) {
                        Jump();
                        jumpCount++;
                    }
                }
                // player cannot "run" while in the air
                if (!Input.GetButton("Slide"))
                {
                    // for running, added slight speedup and slowdown
                    if (Input.GetButton("Sprint"))
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, speedChangeRate);
                        targetVelY = Mathf.Max(_inputForward, _inputTurn) * 2f;
                    }
                    else
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, speed, speedChangeRate);
                    }
                    // Debug.Log(currentSpeed);
                }
            }
            // current speed is preserved while in the air
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        } else if (controller.isGrounded || jumpCount < maxJumps) {
            // jumping in place
            //Debug.Log(jumpCount);

            if (bhopEnabled) {
                    if (Input.GetButton("Jump") && controller.isGrounded) {
                        Jump();
                        jumpCount++;
                    }
                    else if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
                    {
                        Jump();
                        jumpCount++;
                    }
            } else {
                    if (Input.GetButtonDown("Jump")) {
                        Jump();
                        jumpCount++;

                    }
                }
        }

        if (Input.GetButton("Slide"))
        {
            controller.height = crouchedHeight;
            if (currentSpeed > 0)
            {
                currentSpeed -= driftDecay;
            }
            animator.SetBool("isSliding", true);
            // force the player to stand back up once they stop moving
            // else {
            //     currentSpeed = 0;
            //     controller.height = standingHeight;
            // }

            if (animState.IsName("sliding"))
            {
                slideAudio.volume = Mathf.Clamp(currentSpeed * currentSpeed / slideAudioFactor / slideAudioFactor, 0.0f, 1.0f);
            }
            else
            {
                slideAudio.volume = 0;
            }
        }
        else
        {
            animator.SetBool("isSliding", false);
            controller.height = standingHeight;
            slideAudio.volume = 0;
        }

        if (!Input.GetButton("Slide") && grounded && direction.magnitude >= 0.1f)
        {
            
            if (Input.GetButton("Sprint"))
            {
                sprintAudio.mute = false;
                walkAudio.mute = true;
            }
            else
            {
                walkAudio.mute = false;
                sprintAudio.mute = true;
            }
        }
        else
        {
            walkAudio.mute = true;
            sprintAudio.mute = true;
        }

        if (canWallJump && Input.GetButtonDown("Jump")) {
            WallJump();
        }
        // set velY to forward to max of _inputForward and _inputTurn
        // because fly animation is used in all directions
        // this plays the movement animations
        currentVelY = Mathf.Lerp(currentVelY, targetVelY, speedChangeRate);
        animator.SetFloat("velY", currentVelY);

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Character Controller handles collision detection different. Make sure to read docs.
    // I don't believe it can detect triggers, so an alternative method would be creating an empty GameObject child
    // and then giving that child a collider (make sure it doesn't collide with Player) to detect triggers.

    // private void OnCollisionStay(Collision collision) {
    //     if (collision.gameObject.tag == "Ground") {
    //         isGrounded = true;
    //     }
    // }
    // private void OnCollisionExit(Collision collision) {
    //     if (collision.gameObject.tag == "Ground") {
    //         isGrounded = false;
    //     }
    // }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // Debug.Log("Controller collision detected");
        if (!controller.isGrounded && hit.collider.CompareTag("Wall")) {
            wallNormal = hit.normal;
            canWallJump = true;
            Debug.Log("Can Wall Jump!");
        }
        if (hit.gameObject.tag == "Hazard") {
            // can be modified to take in an enemy object and get the damage from that
            takeDamage(10f);
        }
        // handle pushable objects
        if (hit.gameObject.tag == "Pushable") 
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                float effectivePushPower = pushPower / rb.mass;
                // moving faster = more force
                effectivePushPower = currentSpeed/speed * effectivePushPower;
                rb.velocity = pushDirection * effectivePushPower;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Check if the character enters the trigger zone of the platform
        if (other.CompareTag("Elevator"))
        {
            // Set the platform as the parent of the character
            Debug.Log("trigger entered");
            currentPlatform = other.transform;
            controller.transform.SetParent(currentPlatform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the character exits the trigger zone of the platform
        if (other.transform == currentPlatform)
        {
            // Reset the parent of the character
            controller.transform.SetParent(null);
            currentPlatform = null;
        }
    }


    void Jump() {
        Debug.Log("JUMPED");
        if (playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpAdjustment * gravity);
        animator.SetBool("isJumping", true);
        jumpAudio.Play();
    }
    void WallJump() {
        // Calculate the jump direction based on the wall normal and desired jump characteristics.
        Vector3 jumpDirection = (Vector3.up + wallNormal).normalized;

        // Scale only the vertical component of the jump (adjust jumpHeightScale as needed).
        float wallJumpHeight = jumpHeight * jumpHeightScale;
        Vector3 scaledJump = jumpDirection * wallJumpHeight;
        
        // Keep the horizontal component the same.
        scaledJump += transform.forward * 3; //* playerVelocity.magnitude;

        // Apply the scaled jump force.
        playerVelocity = scaledJump;

        // Optionally, add a forward force to make the player move away from the wall.
        playerVelocity += transform.forward * jumpAdjustment;

        // Calculate the new forward direction after the wall jump.
        Vector3 newForward = Vector3.ProjectOnPlane(-wallNormal, Vector3.up).normalized;

        // Rotate the player's character to face the new forward direction.
        transform.forward = newForward;
        
        // Disable wall-jumping until the player lands on the ground or another wall.
        canWallJump = false;

        wallJumpAudio.Play();
    }

    void takeDamage(float damage) {
        if (Time.time - previousDamageTime > invincibilityTime) {
            health -= damage;
            previousDamageTime = Time.time;
            Debug.Log(health);
            tookDamage = true;
        } else {
            tookDamage = false;
        }
    }
}
