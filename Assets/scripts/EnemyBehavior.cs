using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float shootInterval = 2f;
    public Transform player;
    public float detectionRange = 10f;
    public float stoppingDistance = 1.5f;
    public Animator animator;
    public LayerMask playerLayer;

    private float timer = 0f;
    private NavMeshAgent agent;
    private bool isShooting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from the enemy.");
        }
        else
        {
            agent.updatePosition = true;
            agent.updateRotation = false;
        }

        if (animator == null)
        {
            Debug.LogWarning("Animator component is not assigned.");
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        // Check if the player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerInRange = Physics.CheckSphere(transform.position, detectionRange, playerLayer);

        if (playerInRange)
        {
            // Handle shooting behavior
            if (!isShooting)
            {
                isShooting = true;
                timer = shootInterval; // Ensure shooting starts immediately
            }

            // Shoot continuously while the player is within range
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                timer = 0f;
                Shoot();
            }

            // Stop the agent from moving
            agent.isStopped = true;
            SetWalkingAnimation(false);
        }
        else
        {
            // Handle movement towards the player
            if (distanceToPlayer <= detectionRange)
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                agent.SetDestination(targetPosition);

                // Stop moving when within stopping distance
                if (distanceToPlayer <= stoppingDistance)
                {
                    agent.isStopped = true;
                    SetWalkingAnimation(false);
                }
                else
                {
                    agent.isStopped = false;
                    SetWalkingAnimation(true);
                }

                // Flip the enemy's render direction towards the player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                if (directionToPlayer.x > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

                isShooting = false; // Reset shooting flag when moving
            }
            else
            {
                agent.isStopped = true;
                SetWalkingAnimation(false);
            }
        }
    }

    void Shoot()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned! Cannot shoot.");
            return;
        }

        if (projectilePrefab == null || shootingPoint == null)
        {
            Debug.LogError("Projectile prefab or shooting point is not assigned.");
            return;
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            // Calculate the direction and force for the throw
            Vector3 direction = (player.position - shootingPoint.position).normalized;
            float throwStrength = 10f; // Adjust this value to control the throw speed
            Vector3 force = direction * throwStrength;

            // Apply the force to the projectile
            projectileRb.AddForce(force, ForceMode.Impulse);
            animator.SetTrigger("Throw");

            Debug.Log("Projectile shot towards player.");
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component!");
        }
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw stopping distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
