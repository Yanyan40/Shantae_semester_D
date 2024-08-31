using UnityEngine;
using UnityEngine.AI;

public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;
    public Animator animator;
    public ParticleSystem hitParticleEffect;
    public int EnemyValue = 1;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is tagged as "Floor"
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Instantiate the particle effect at the point of collision
            if (hitParticleEffect != null)
            {
                Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
            }

            // Destroy the damagingObject only if it is not tagged as "Enemy"
            if (!gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
        // Check if the collided object is tagged as "Player"
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Log the name and tag of the object the damagingObject collided with
            Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

            // Instantiate the particle effect at the point of collision
            if (hitParticleEffect != null)
            {
                Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
            }

            if (animator != null)
            {
                animator.SetTrigger("Throw");
            }

            // Get the HealthSystem component from the player and apply damage
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log("Damaging Player");
                healthSystem.takedamage(damageAmount);
                // Refresh the collider of the player or enemy
                RefreshCollider(collision.gameObject);
            }
            else
            {
                Debug.Log("HealthSystem not found on player");
            }

            // Stop enemy's NavMeshAgent if the enemy is being hit
            StopEnemyNavMeshAgent(collision.gameObject);

            // Destroy the damagingObject only if it is not tagged as "Enemy"
            if (!gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
        // Check if the collided object is tagged as "Enemy"
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Log that the damagingObject collided with an enemy but will not destroy itself
            Debug.Log("Collided with another enemy. The damaging object will not be destroyed.");
            // Do nothing since we don't want to destroy the enemy object
        }
        else
        {
            // For any other collisions not involving "Floor", "Player", or "Enemy"
            Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

            // Instantiate the particle effect at the point of collision
            if (hitParticleEffect != null)
            {
                Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
            }

            // Destroy the damagingObject only if it is not tagged as "Enemy"
            if (!gameObject.CompareTag("Enemy"))
            {
                if (animator != null)
                {
                    animator.SetTrigger("Dead");
                }
                Destroy(gameObject); // Ensure the object is destroyed in this case
            }
        }
    }

    // Method to stop the enemy's NavMeshAgent
    void StopEnemyNavMeshAgent(GameObject enemy)
    {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true; // Stop the agent from moving
        }
    }

    // Method to refresh the collider
    void RefreshCollider(GameObject obj)
    {
        Collider[] colliders = obj.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false; // Temporarily disable the collider
            collider.enabled = true;  // Re-enable the collider
        }
    }
}
