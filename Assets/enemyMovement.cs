using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float detectionRadius = 5f; // Radius for CircleCast2D to detect player
    public LayerMask playerLayer; // Layer of the player for CircleCast2D detection
    public LayerMask obstacleLayer; // Layer of obstacles to jump over
    public float jumpForce = 5f; // Force applied when the enemy jumps
    public float jumpCheckDistance = 1f; // Distance ahead to check for obstacles
    public Vector2 jumpCheckSize = new Vector2(1f, 1f); // Size of the BoxCast2D for obstacle detection
    private bool isJumping = false; // Tracks if the enemy is currently jumping

    [Header("Attack Settings")]
    public float attackRange = 1f; // Range for BoxCast2D to check for attack
    public Vector2 boxCastSize = new Vector2(1f, 1f); // Size of the BoxCast2D
    public float attackCooldown = 1.5f; // Cooldown time between attacks
    private float lastAttackTime; // Time since the last attack

    private Transform player; // Reference to the player's transform
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find player by tag
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        if (!isJumping) // Only detect and move if not jumping
        {
            DetectPlayerAndMove();
        }
        HandleJump(); // Handle jump input
    }

    void DetectPlayerAndMove()
    {
        // Perform CircleCast2D to detect the player
        Collider2D playerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerDetected != null)
        {
            Debug.Log("Player detected within detection radius.");
            // Move towards the player if detected within radius
            MoveTowardsPlayer();
        }
        else
        {
            Debug.Log("Player not detected within detection radius.");
        }

        // Check if player is within attack range using BoxCast2D
        if (IsPlayerInAttackRange())
        {
            // Attack player if cooldown has passed
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time; // Reset attack cooldown
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Calculate direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Check for obstacles in the path using BoxCast2D
            RaycastHit2D obstacleHit = Physics2D.BoxCast(transform.position, jumpCheckSize, 0f, direction, jumpCheckDistance, obstacleLayer);

            if (obstacleHit.collider != null)
            {
                Debug.Log("Obstacle detected, jumping over.");
                // Trigger jump animation if obstacle detected
                animator.SetTrigger("Jump");
                isJumping = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isWalking", false); // Stop walking animation during jump
            }
            else
            {
                // Move the enemy towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                animator.SetBool("isWalking", true);
            }
        }
    }

    void HandleJump()
    {
        if (isJumping)
        {
            // Check if the enemy has landed
            if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
            {
                isJumping = false; // Reset jumping state
                animator.SetBool("isWalking", true); // Resume walking animation
            }
        }
    }

    bool IsPlayerInAttackRange()
    {
        // Perform BoxCast2D to check if the player is within attack range
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxCastSize, 0f, playerLayer);
        return hit != null && hit.CompareTag("Player");
    }

    void AttackPlayer()
    {
        // Here you would implement the enemy's attack logic, such as dealing damage to the player
        Debug.Log("Attacking the Player!");
        animator.SetTrigger("Throw"); // Trigger the attack animation
        // Add any other attack animations or effects here
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection radius for the CircleCast2D
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Visualize the BoxCast2D range for attacking
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxCastSize);

        // Visualize the BoxCast2D for obstacle detection
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.right * jumpCheckDistance, jumpCheckSize);
    }
}
