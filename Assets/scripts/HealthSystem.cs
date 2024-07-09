using System;
using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth = 0;
    public Animator animator;
    public PlayerController playerController;
    public float hitDuration = 2f;

    public event Action<int, int> onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChange();
    }

    public void takedamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("isGettingHit");
        StartCoroutine(HandleHit());
        if (currentHealth <= 0)
        {
            animator.SetTrigger("isDead");
            currentHealth = 0;
            Debug.Log("DEAD");
        }
        else
        {
            animator.SetBool("isDead", false);
        }
        NotifyHealthChange();
    }

    private IEnumerator HandleHit()
    {
        playerController.isDead = true;
        yield return new WaitForSeconds(hitDuration);
        playerController.isDead = false;
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
