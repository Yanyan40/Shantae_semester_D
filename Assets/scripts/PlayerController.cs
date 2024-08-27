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
            SetAnimatorBools(false, false, false, false, false, false);
        }
    }

    void HandleAttack()
    {
        if (!isDead && Input.GetKeyDown(KeyCode.Z) && !isHitting) // Ensure we are not already attacking
        {
            if (isCrouching)
            {
                CrouchEnemyRaycast();
                animator.SetTrigger("CrouchHit");
            }
            else
            {
                EnemyRaycast();
                animator.SetTrigger("isHitting");
            }
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f).normalized * moveSpeed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, 0f);

        if (moveInput != 0)
        {
            if (isCrouching)
            {
                animator.SetBool("isCrawling", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isCrawling", false);
            }

            bool facingRight = moveInput > 0;
            spriteRenderer.flipX = !facingRight;

            attackTrigger.transform.localPosition = isCrouching ?
                (facingRight ? crouchingAttackOffset : leftFacingCrouchingAttackOffset) :
                (facingRight ? standingAttackOffset : leftFacingStandingAttackOffset);

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
        bool crouchPressed = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        bool crouchReleased = Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);

        if (crouchPressed && !isCrouching)
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
            normalCollider.gameObject.SetActive(false);
            crouchCollider.gameObject.SetActive(true);
        }
        else if (crouchReleased && isCrouching)
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
            animator.SetBool("isCrawling", false);
            crouchCollider.gameObject.SetActive(false);
            normalCollider.gameObject.SetActive(true);
        }
    }

    void EnemyRaycast()
    {
        PerformRaycast("isHitting", Vector3.right);
    }

    void CrouchEnemyRaycast()
    {
        PerformRaycast("CrouchHit", Vector3.right);
    }

    void PerformRaycast(string animationTrigger, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayDirection = spriteRenderer.flipX ? -direction : direction;

        if (Physics.Raycast(transform.position, rayDirection, out hit, attackRange, attackLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit Pressed and Enemy Detected");
                isHitting = true;
                animator.SetTrigger(animationTrigger);
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

    IEnumerator ResetHittingFlag()
    {
        // Wait until the attack animation is finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isHitting = false;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
            Gizmos.DrawLine(transform.position, transform.position + rayDirection * attackRange);
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

    // Helper method to set multiple animator booleans at once
    void SetAnimatorBools(bool jumping, bool crouching, bool running, bool falling, bool hitting, bool crawling)
    {
        animator.SetBool("isJumping", jumping);
        animator.SetBool("isCrouching", crouching);
        animator.SetBool("isRunning", running);
        animator.SetBool("isFalling", falling);
        animator.SetBool("isHitting", hitting);
        animator.SetBool("isCrawling", crawling);
    }
}
