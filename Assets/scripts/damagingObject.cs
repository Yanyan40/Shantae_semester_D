using UnityEngine;
public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("CannonBall"))
        {

            Destroy(gameObject);
            if (collision.gameObject.CompareTag("Player"))
            {
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
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem enemyHealthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (enemyHealthSystem != null)
            {
                Debug.Log("Damaging Enemy");
                enemyHealthSystem.takedamage(damageAmount);
            }
            else
            {
                Debug.Log("HealthSystem not found on enemy");
            }
        }
    }
}
