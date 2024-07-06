using UnityEngine;

public class HealObject : MonoBehaviour
{
    public int healAmount = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log("Healing Player");
                healthSystem.heal(healAmount);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("HealthSystem not found on player");
            }
        }
    }
}
