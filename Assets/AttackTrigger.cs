using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hitted" + gameObject.tag);
        // Check if the collider has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Destroy the enemy game object
            Destroy(other.gameObject);
        }
    }
}
