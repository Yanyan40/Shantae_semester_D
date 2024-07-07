using UnityEngine;

public class FallForDeath : MonoBehaviour
{
    public HealthSystem healthSystem;
    private ResetScene reset;

    // Initialize references
    void Start()
    {
        reset = FindObjectOfType<ResetScene>();
        if (reset == null)
        {
            Debug.LogError("ResetScene script not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (reset != null)
            {

                healthSystem.takedamage(healthSystem.currentHealth);


                reset.ResetCurrentScene();
            }
            else
            {
                Debug.LogError("ResetScene reference is null!");
            }
        }
    }
}
