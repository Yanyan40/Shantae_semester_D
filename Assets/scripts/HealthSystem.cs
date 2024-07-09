using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth = 0;
    public Animator animator;
    public PlayerController playerController; 
    public float hitDuration = 1f; 

    public event Action<int, int> onHealthChanged;

    private Coroutine hitCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChange();
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
            if (playerController == null)
            {
                playerController = FindObjectOfType<PlayerController>();
            }
        }
    }

    public void takedamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
        {
            animator.SetTrigger("isGettingHit");
            if (hitCoroutine != null)
            {
                StopCoroutine(hitCoroutine);
            }
            hitCoroutine = StartCoroutine(HandleHit());
        }
        else
        {
            animator.ResetTrigger("isGettingHit");
            animator.SetTrigger("isDead");
            currentHealth = 0;
            playerController.isDead = true;
            Debug.Log("DEAD");
        }
        NotifyHealthChange();
    }

    private IEnumerator HandleHit()
    {
        playerController.isDead = true;
        yield return new WaitForSeconds(hitDuration);
        if (currentHealth > 0)
        {
            playerController.isDead = false;
        }
    }

    public void heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        NotifyHealthChange();
    }

    void NotifyHealthChange()
    {
        if (onHealthChanged != null)
        {
            onHealthChanged(currentHealth, maxHealth);
        }
    }
}
