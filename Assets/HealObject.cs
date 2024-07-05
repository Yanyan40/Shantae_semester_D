using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name); 

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected"); 

            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log("Healing player"); 
                healthSystem.heal(healAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("HealthSystem not found on player");
            }
        }
        else
        {
            Debug.Log("Object is not the player");
        }
    }
}
