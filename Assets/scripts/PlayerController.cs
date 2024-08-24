using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int EnemyValue = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private Rigidbody rb;
    private Collider playerCollider;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public Transform[] attachedObjects;
    public bool isDead = false;
    public bool isHitting = false;
    public BoxCollider normalCollider;
    public BoxCollider crouchCollider;
    public GameObject attackTrigger; // Reference to the attack trigger GameObject
    public Vector3 standingAttackOffset; // Offset for attack trigger while standing
    public Vector3 crouchingAttackOffset; // Offset for attack trigger while crouching
    public Vector3 leftFacingStandingAttackOffset; // Offset for attack trigger while standing and facing left
    public Vector3 leftFacingCrouchingAttackOffset; // Offset for attack trigger while crouching and facing left
    [Header("BoxCast Parameters")]
    public Vector3 boxCastSize = new Vector3(0.5f, 0.05f, 0.01f);
    public float boxCastDistance = 0.1f;
    public Vector3 boxCastOffset = Vector3.zero;

    private bool isCrouching = false;
    private bool isCrawling = false; // New variable to track crawling state
    public float attackRange = 1f; // Distance for Raycast attack
    public LayerMask attackLayer; // Define which layers should be hit by the Raycast

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        crouchCollider.gameObject.SetActive(false);
        rb.freezeRotation = true; // Prevent rotation if needed
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isDead) // Check if the player is dead
        {
            isGrounded = CheckGround();
            HandleMovement();
            HandleJump();
            HandleCrouch();
            HandleFall();
            HandleAttack();
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isHitting", false);
            animator.SetBool("isCrawling", false); // Reset crawling state
        }
    }

    void HandleAttack()
    {
        if (!isDead && Input.GetKeyDown(KeyCode.Z) && !isHitting) // Ensure we are not already attacking
        {
            // Perform attack raycast
            EnemyRaycast();
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Create the movement vector, focusing only on the X-axis
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f);

        // Normalize the direction if needed
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Apply the move speed to the normalized direction
        moveDirection *= moveSpeed;

        // Update the Rigidbody's velocity, only affecting the X and Y axes
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, 0f);

        if (moveInput != 0)
        {
            if (isCrouching)
            {
                isCrawling = true;
                animator.SetBool("isCrawling", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                isCrawling = false;
                animator.SetBool("isRunning", true);
                animator.SetBool("isCrawling", false);
            }

            // Determine facing direction
            bool facingRight = moveInput > 0;
            spriteRenderer.flipX = !facingRight;

            // Set attack trigger offset based on facing direction and crouch state
            if (facingRight)
            {
                attackTrigger.transform.localPosition = isCrouching ? crouchingAttackOffset : standingAttackOffset;
            }
            else // Facing left
            {
                attackTrigger.transform.localPosition = isCrouching ? leftFacingCrouchingAttackOffset : leftFacingStandingAttackOffset;
            }

            // Adjust the position of attached objects (like weapons, etc.) based on facing direction
            foreach (Transform obj in attachedObjects)
            {
                Vector3 objPosition = obj.localPosition;
                objPosition.x = Mathf.Abs(objPosition.x) * (facingRight ? 1 : -1);
                obj.localPosition = objPosition;
            }
        }
        else
        {
            isCrawling = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrawling", false);
        }
    }

    void HandleJump()
    {
        bool spacePressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X);

        if (isGrounded && spacePressed)
        {
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            audioSource.PlayOneShot(audioClips[0]);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    void HandleFall()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
        else if (isGrounded)
        {
            animator.SetBool("isFalling", false);
        }
    }

    void HandleCrouch()
    {
        bool vPressed = Input.GetKey(KeyCode.DownArrow); // Use KeyCode.DownArrow for crouching

        if (vPressed)
        {
            // Enter crouch state
            if (!isCrouching) // Check if the player is not already crouching
            {
                isCrouching = true;
                animator.SetBool("isCrouching", true);
                normalCollider.gameObject.SetActive(false);
                crouchCollider.gameObject.SetActive(true);
            }

            // Check for attack while crouching
            if (Input.GetKeyDown(KeyCode.Z) && !isHitting) // Ensure we are not already attacking
            {
                // Perform attack raycast
                CrouchEnemyRaycast();
            }
        }
        else if (!vPressed && isCrouching)
        {
            // Exit crouch state
            isCrouching = false;
            isCrawling = false; // Reset crawling state
            animator.SetBool("isCrouching", false);
            animator.SetBool("isCrawling", false);
            crouchCollider.gameObject.SetActive(false);
            normalCollider.gameObject.SetActive(true);
        }
    }

    void EnemyRaycast()
    {
        // Perform Raycast attack
        RaycastHit hit;
        Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;

        if (Physics.Raycast(transform.position, rayDirection, out hit, attackRange, attackLayer))
        {
            // Check if the raycast hit an enemy
            if (hit.collider.CompareTag("Enemy")) // Replace "Enemy" with your enemy tag
            {
                Debug.Log("Hit Pressed and Enemy Detected");
                isHitting = true;
                animator.SetTrigger("isHitting");
                audioSource.PlayOneShot(audioClips[1]); // Play sound at start of attack

                ScoreManager.Instance.AddEnemiesDestroyed(EnemyValue);
                Destroy(hit.collider.gameObject);

                // Reset isHitting after attack is processed
                StartCoroutine(ResetHittingFlag());
            }
            else
            {
                Debug.Log("Raycast hit something, but not an enemy");
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }

    void CrouchEnemyRaycast()
    {
        // Perform Raycast attack while crouching
        RaycastHit hit;
        Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;

        if (Physics.Raycast(transform.position, rayDirection, out hit, attackRange, attackLayer))
        {
            // Check if the raycast hit an enemy
            if (hit.collider.CompareTag("Enemy")) // Replace "Enemy" with your enemy tag
            {
                Debug.Log("Crouch Hit Pressed and Enemy Detected");
                isHitting = true;
                animator.SetTrigger("CrouchHit");
                audioSource.PlayOneShot(audioClips[1]); // Play sound at start of attack

                // Destroy the enemy game object
                Destroy(hit.collider.gameObject);

                // Reset isHitting after attack is processed
                StartCoroutine(ResetHittingFlag());
            }
            else
            {
                Debug.Log("Raycast hit something, but not an enemy");
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }

    IEnumerator ResetHittingFlag()
    {
        // Wait until the attack animation is finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isHitting = false;
    }

    void OnDrawGizmos()
    {
        // Draw the raycast direction in the scene view
        if (Application.isPlaying) // Draw during play mode
        {
            Gizmos.color = Color.red; // Raycast color
            Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
            Vector3 rayStart = transform.position;

            // Draw raycast line
            Gizmos.DrawLine(rayStart, rayStart + rayDirection * attackRange);
        }
    }

    bool CheckGround()
    {
        return Physics.BoxCast(groundCheck.position + boxCastOffset, boxCastSize, Vector3.down, Quaternion.identity, boxCastDistance, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position + boxCastOffset + Vector3.down * boxCastDistance, boxCastSize * 2);
    }
}
