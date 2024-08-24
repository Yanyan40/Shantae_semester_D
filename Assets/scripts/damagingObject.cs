using UnityEngine;

public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;
    public Animator animator;
    public ParticleSystem hitParticleEffect;
    public int EnemyValue = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (hitParticleEffect != null)
        {
            Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log("Damaging Player");
                healthSystem.takedamage(damageAmount);
            }
            else
            {
                Debug.Log("HealthSystem not found on player");
            }
        }

        // Check if the damagingObject has the tag "Enemy" before destroying
        if (!gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
