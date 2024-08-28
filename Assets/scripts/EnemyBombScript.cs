using UnityEngine;
using UnityEngine.AI;

public class EnemyBombScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float shootInterval = 2f;
    public Transform player;

    public float detectionRange = 10f;  // Range within which the enemy detects the player
    public float stoppingDistance = 1.5f;  // Distance at which the enemy stops following
    public Animator animator;

    private float timer = 0f;
    private bool playerInTrigger = false; // Flag to check if player is in the trigger
    private NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component from the parent object
        agent = GetComponentInParent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned in the Inspector!");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is not found in the parent object.");
        }
        else
        {
            // Constrain the agent to move only on the X-axis
            agent.updatePosition = true;
            agent.updateRotation = false;
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is not set.");
            return;
        }

        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent is not assigned.");
            return;
        }

        // Stop movement and shooting if the player is in the trigger
        if (playerInTrigger)
        {
            // Shoot continuously while the player is inside the trigger
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                timer = 0f;
                Shoot();
            }

            // Stop the agent from moving
            agent.isStopped = true;
        }
        else
        {
            // Calculate the distance between the player and the enemy
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                // If the player is within detection range, move towards the player
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                agent.SetDestination(targetPosition);

                // Stop moving when within stopping distance
                if (distanceToPlayer <= stoppingDistance)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                // If the player is out of range, stop the agent
                agent.isStopped = true;
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
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (agent != null)
            {
                agent.isStopped = true; // Ensure the agent stops moving when the player enters the trigger
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}