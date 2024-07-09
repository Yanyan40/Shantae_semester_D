using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth = 0;
    public Animator animator;
    private PlayerController playerController;

    public event Action<int, int> onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChange();
        playerController = GetComponent<PlayerController>();
    }

    public void takedamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            animator.SetTrigger("isDead");
            currentHealth = 0;
            Debug.Log("DEAD");
            if (playerController != null)
            {
                playerController.isDead = true;
            }
        }
        else
        {
            animator.SetBool("isDead", false);
        }
        NotifyHealthChange();
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
            onHealthChanged(currentHealth, maxHealth); // Notify listeners
        }
    }
}
