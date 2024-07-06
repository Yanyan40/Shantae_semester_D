using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6; 
    public int currentHealth = 0;
    Animator animator;

    public event Action<int, int> onHealthChanged; 

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChange();
    }

    public void takedamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //animator.SetBool("isDead", true);
            Debug.Log("DEAD");
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
