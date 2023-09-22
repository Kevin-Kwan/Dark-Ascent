using System.Collections.Generic;
using UnityEngine;

// This script is used to control the player's game object in the third-person perspective.
public class ThirdPController : MonoBehaviour
{
    public CharacterController controller;
    // the Main Camera in the scene
    public Transform camera;
    public float speed = 6.0f;
    public float runSpeed = 12.0f;

    private float currentSpeed = 0.0f;

    // parameters for smooth turning
    public float turnSmoothingTime = 0.1f;
    float turnSmoothingVelocity;

    // parameters for jumping
    bool isGrounded;
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
    public float jumpHeightScale = 1.0f;

    // parameters for sliding
    public float maxDrift = 4f;
    public float driftDecay = 0.02f;
    public float crouchedHeight = 1.0f;
    private float standingHeight;
    private Vector3 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen.
        standingHeight = controller.height;
    }

    // Update is called once per frame
    void Update() {
        // Debug.Log(isGrounded);
        // Debug.Log(controller.isGrounded);
        // We are using GetAxisRaw in case the player is using a controller.
        // Not tested yet.
        if (controller.isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
            jumpCount = 0;
            canWallJump = false;
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // if Jumping while moving
            if (controller.isGrounded || jumpCount < maxJumps) {
                Debug.Log(jumpCount);
                if (bhopEnabled) {
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
                if (!Input.GetButton("Slide")) {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                        currentSpeed = runSpeed;
                    } else {
                        currentSpeed = speed;
                    }
                }
            }
            // current speed is preserved while in the air
        } else if (controller.isGrounded || jumpCount < maxJumps) {
            // jumping in place
            Debug.Log(jumpCount);

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
            if (!Input.GetButton("Slide"))
                currentSpeed = 0;
        }

        if (controller.isGrounded && Input.GetButtonDown("Slide") && (currentSpeed <= runSpeed))
        {
            Debug.Log("slid");
            currentSpeed += maxDrift;
        }

        if (Input.GetButton("Slide"))
        {
            controller.height = crouchedHeight;
            if (currentSpeed > 0)
            {
                currentSpeed -= driftDecay;
                Debug.Log(currentSpeed);
            }
        } else
        {
            controller.height = standingHeight;
        }

        if (canWallJump && Input.GetButtonDown("Jump")) {
            WallJump();
        }
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
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
    }
    void Jump() {
        Debug.Log("JUMPED");
        playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpAdjustment * gravity);
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
    }

}