using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hitted" + gameObject.tag);
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
