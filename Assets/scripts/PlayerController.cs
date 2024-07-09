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
    public Transform[] attachedObjects; 
    public bool isDead = false; 
    public BoxCollider normalCollider; 
    public BoxCollider crouchCollider; 
    [Header("BoxCast Parameters")]
    public Vector3 boxCastSize = new Vector3(0.5f, 0.05f, 0.01f);
    public float boxCastDistance = 0.1f;
    public Vector3 boxCastOffset = Vector3.zero;

    [SerializeField] private bool checkingGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        crouchCollider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDead) // Check if the player is dead
        {
            checkingGround = CheckGround();
            HandleMovement();
            HandleJump();
            HandleHit();
            HandleCrouch(); 
            HandleFall();
        }
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

    void HandleCrouch()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("isCrouching", true);
            normalCollider.gameObject.SetActive(false);
            crouchCollider.gameObject.SetActive(true);
        }
        else if (isGrounded && Input.GetKeyUp(KeyCode.V))
        {
            animator.SetBool("isCrouching", false);
            crouchCollider.gameObject.SetActive(false);
            normalCollider.gameObject.SetActive(true);
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
