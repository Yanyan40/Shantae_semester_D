using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float stoppingDistance = 1.5f;
    public Animator animator;

    private NavMeshAgent agent;
    private bool playerInTrigger = false;

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

        if (!playerInTrigger)
        {
            // Calculate the distance between the player and the enemy
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                agent.SetDestination(targetPosition);

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
            }
            else
            {
                agent.isStopped = true;
                SetWalkingAnimation(false);
            }
        }
        else
        {
            agent.isStopped = true;
            SetWalkingAnimation(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            agent.isStopped = true;
            SetWalkingAnimation(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
