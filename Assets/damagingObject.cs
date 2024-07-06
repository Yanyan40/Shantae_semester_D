using UnityEngine;

public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;

    void OnCollisionEnter(Collision collision)
    {
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
}
