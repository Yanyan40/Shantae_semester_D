using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Deah()
    {
        if (currentHealth <= 0)
        {
            // Trigger death animation
            animator.ResetTrigger("isGettingHit");
            animator.SetTrigger("isDead");
            // Mark player as dead
            playerController.isDead = true;
            // Restart the scene after a delay to allow the death animation to play
            Invoke("RestartScene", 1f); // Adjust delay as needed
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
            Deah(); // Call Deah method if health <= 0
            currentHealth = 0;
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

    void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
