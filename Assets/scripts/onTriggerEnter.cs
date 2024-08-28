using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class onTriggerEnter : MonoBehaviour
{
    public UnityEvent onTriggerEvent;
    public UnityEvent exitTrigger;
    public UnityEvent stayEvent;
    public Animator animator;

    private bool hasPlayerEntered = false; // To track if the player is already inside the trigger
    private float throwCooldown = 2f; // Cooldown duration in seconds
    private float nextThrowTime = 0f; // Time at which the next throw can happen

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEvent.Invoke();
            hasPlayerEntered = true; // Player has entered the trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exitTrigger.Invoke();
            hasPlayerEntered = false; // Player has exited the trigger
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stayEvent.Invoke();

            if (Time.time >= nextThrowTime && hasPlayerEntered) // Check cooldown and if player is inside
            {
                animator.SetTrigger("Throw");
                nextThrowTime = Time.time + throwCooldown; // Set the next throw time
            }
        }
    }
}
