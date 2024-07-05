using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagingObject : MonoBehaviour
{
    public int damageAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
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
