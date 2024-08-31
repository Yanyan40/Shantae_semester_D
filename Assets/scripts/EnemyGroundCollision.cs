using UnityEngine;

public class EnemyGroundCollision : MonoBehaviour
{
    public ParticleSystem hitGroundParticle; // Assign a particle system prefab in the inspector
    public LayerMask groundLayer; // LayerMask to define what is considered as ground
    private bool isDead = false; // Flag to check if the enemy is dead

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy collided with the ground layer
        if (isDead && (groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Instantiate and play the particle effect at the collision point
            if (hitGroundParticle != null)
            {
                Instantiate(hitGroundParticle, collision.contacts[0].point, Quaternion.identity);
                hitGroundParticle.Play();
            }
        }
    }

    // Method to be called to set the enemy as dead
    public void SetDead(bool dead)
    {
        isDead = dead;
    }
}
