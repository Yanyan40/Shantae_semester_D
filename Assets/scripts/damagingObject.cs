using UnityEngine;
public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;
    public Animator animator;
    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("CannonBall"))
        {

            Destroy(gameObject);
        }
            if (collision.gameObject.CompareTag("Player"))
            {
                animator = GetComponent<Animator>();
                animator.SetTrigger("Attack");
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

        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            HealthSystem enemyHealthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (enemyHealthSystem != null)
            {
                Debug.Log("Damaging Enemy");
                enemyHealthSystem.takedamage(damageAmount);
            }
        }
    }
}