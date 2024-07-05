using System.Collections.Generic;
using UnityEngine;

public class healthHeartManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public HealthSystem healthSystem;
    List<healthHeart> hearts = new List<healthHeart>();

    private void Start()
    {
        if (healthSystem != null)
        {
            healthSystem.onHealthChanged += HealthChangedHandler; 
        }
        DrawHearts();
    }

    void HealthChangedHandler(int currentHealth, int maxHealth)
    {
        DrawHearts(); 
    }

    public void DrawHearts()
    {
        ClearHearts();

        float maxHealthRemainder = healthSystem.maxHealth % 2;
        int heartsToMake = (int)((healthSystem.maxHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = Mathf.Clamp(healthSystem.currentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform); 

        healthHeart heartComponent = newHeart.GetComponent<healthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (healthHeart heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();
    }
}
