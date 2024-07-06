using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int healAmount = 1;
    public HealthSystem healthSystem;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered: " + gameObject.tag);

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HealObject"))
        {
            if (healthSystem != null)
            {
                Debug.Log("Healing player");
                healthSystem.heal(healAmount);
            }
            else
            {
                Debug.LogWarning("HealthSystem component not assigned in the Inspector.");
            }

            Destroy(other.gameObject);
        }
    }
}
