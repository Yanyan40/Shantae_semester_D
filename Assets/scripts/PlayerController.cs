using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private Rigidbody rb;
    private Collider playerCollider;
    [SerializeField] private bool hitNow;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public Transform[] attachedObjects;
    public bool isDead = false;
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
            HandleHit();
            HandleCrouch();
            HandleFall();
            UpdateAttackTriggerPosition();
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isFalling", false);
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Create the movement vector and normalize it if needed
        Vector3 moveDirection = new Vector3(moveInput, 0f, verticalInput);
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Apply the move speed to the normalized direction
        moveDirection *= moveSpeed;

        // Update the Rigidbody's velocity
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        if (moveInput != 0 || verticalInput != 0)
        {
            animator.SetBool("isRunning", true);

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

            foreach (Transform obj in attachedObjects)
            {
                Vector3 objPosition = obj.localPosition;
                objPosition.x = Mathf.Abs(objPosition.x) * (facingRight ? 1 : -1);
                obj.localPosition = objPosition;
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
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
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }

    void HandleHit()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isHitting", true);
            audioSource.PlayOneShot(audioClips[1]);
            audioSource.PlayOneShot(audioClips[2]);
        }
        else
        {
            animator.SetBool("isHitting", false);
        }
    }

    void HandleCrouch()
    {
        bool vPressed = Input.GetKey(KeyCode.V);

        if (vPressed)
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
            normalCollider.gameObject.SetActive(false);
            crouchCollider.gameObject.SetActive(true);
        }
        else if (!vPressed && isCrouching)
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
            crouchCollider.gameObject.SetActive(false);
            normalCollider.gameObject.SetActive(true);
        }
    }

    void UpdateAttackTriggerPosition()
    {
        // Determine facing direction
        bool facingRight = !spriteRenderer.flipX;

        // Set attack trigger offset based on facing direction and crouch state
        if (facingRight)
        {
            attackTrigger.transform.localPosition = isCrouching ? crouchingAttackOffset : standingAttackOffset;
        }
        else // Facing left
        {
            attackTrigger.transform.localPosition = isCrouching ? leftFacingCrouchingAttackOffset : leftFacingStandingAttackOffset;
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
