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
    [SerializeField] private bool hitNow;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public Transform[] attachedObjects; // Objects that should flip with the player

    [Header("BoxCast Parameters")]
    public Vector3 boxCastSize = new Vector3(0.5f, 0.05f, 0.01f);
    public float boxCastDistance = 0.1f;
    public Vector3 boxCastOffset = Vector3.zero;


    [SerializeField] private bool checkingGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        checkingGround = CheckGround();
        HandleMovement();
        HandleJump();
        HandleHit();
        HandleFall();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f) * moveSpeed;
        Vector3 newVelocity = new Vector3(moveDirection.x, rb.velocity.y, 0f);
        rb.velocity = newVelocity;

        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
            bool facingRight = moveInput > 0;
            spriteRenderer.flipX = !facingRight;

            // Flip attached objects
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
        isGrounded = checkingGround;

        if (isGrounded)
        {
            bool spacePressed = Input.GetKeyDown(KeyCode.Space);
            bool xPressed = Input.GetKeyDown(KeyCode.X);
            animator.SetBool("isJumping", false);


            if (spacePressed || xPressed)
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
    }

    void HandleFall()
    {
        // Add fall handling logic if needed
    }

    void HandleHit()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isHitting", true);
            audioSource.PlayOneShot(audioClips[1]);
        }
        else
        {
            animator.SetBool("isHitting", false);
        }
    }

    bool CheckGround()
    {
        return Physics.BoxCast(groundCheck.position + boxCastOffset, boxCastSize, Vector3.down, Quaternion.identity, boxCastDistance, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe cube at groundCheck.position with the same size as used in BoxCast
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position + boxCastOffset + Vector3.down * boxCastDistance, boxCastSize * 2);
    }
}
