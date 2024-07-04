using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantaeBasicAttack : MonoBehaviour
{
    public GameObject attackTrigger; // Reference to the attack trigger GameObject
    public KeyCode attackKey = KeyCode.Z; // Key to trigger the attack
    public float attackDuration = 0.5f; // Duration of the attack

    private Coroutine attackCoroutine;

    void Update()
    {
        // Check if the attack key is pressed
        if (Input.GetKeyDown(attackKey))
        {
            // If there's already an attack in progress, stop it
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            // Start a new attack
            attackCoroutine = StartCoroutine(ActivateAttackTrigger());
        }
    }

    private IEnumerator ActivateAttackTrigger()
    {
        // Enable the trigger to detect collisions
        attackTrigger.SetActive(true);

        // Wait for the attack duration
        yield return new WaitForSeconds(attackDuration);

        // Disable the trigger
        attackTrigger.SetActive(false);
    }

}
