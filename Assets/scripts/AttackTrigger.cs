using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int healAmount = 1;
    public int scoreValue = 10;

    private void OnDestroy()
    {
        ScoreManager.Instance.AddEnemiesDestroyed(1); // Assuming each enemy destroyed adds 1 to the count
    }
    public HealthSystem healthSystem;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered: " + gameObject.tag);

        if (other.CompareTag("Enemy"))
        {
            OnDestroy();
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
