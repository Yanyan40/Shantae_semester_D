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
    SerializeField hitNow;
    public AudioClip[] audioClips;
    public AudioSource audioSource;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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
            spriteRenderer.flipX = moveInput < 0;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void HandleJump()
    {
isGrounded = Physics.BoxCast(groundCheck.position, new Vector3(0.8f, 0.05f, 0.01f), Vector3.down, Quaternion.identity, 0.1f, groundLayer);

        if (isGrounded)
        {
            bool spacePressed = Input.GetKeyDown(KeyCode.Space);
            bool xPressed = Input.GetKeyDown(KeyCode.X);

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

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe cube at groundCheck.position with the same size as used in BoxCast
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(0.8f, 0.05f, 0.01f); // Adjust size according to your BoxCast
        Gizmos.DrawWireCube(groundCheck.position + Vector3.down * 0.05f, boxSize * 2);
    }

}
