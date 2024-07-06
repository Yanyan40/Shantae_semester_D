using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int gemsCollected = 0;
    private int enemiesDestroyed = 0;
    public TextMeshProUGUI scoreUI; 

    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of ScoreManager found!");
            Destroy(gameObject);
        }

        if (scoreUI == null)
        {
            Debug.LogWarning("ScoreUI TextMeshProUGUI reference is not assigned in ScoreManager.");
        }
    }

    public void AddGems(int amount)
    {
        gemsCollected += amount;
        Debug.Log("Gems collected: " + gemsCollected);
    }

    public void AddEnemiesDestroyed(int amount)
    {
        enemiesDestroyed += amount;
        Debug.Log("Enemies destroyed: " + enemiesDestroyed);
    }

    public int GetGemsCollected()
    {
        return gemsCollected;
    }

    public int GetEnemiesDestroyed()
    {
        return enemiesDestroyed;
    }

    private void Update()
    {
        // Update score text
        if (scoreUI != null)
        {
            scoreUI.text = "Score:" + (gemsCollected + enemiesDestroyed).ToString();
        }
        else
        {
            Debug.LogWarning("ScoreUI TextMeshProUGUI reference is not assigned in ScoreManager.");
        }
    }

    public void ResetScore()
    {
        gemsCollected = 0;
        enemiesDestroyed = 0;
    }
}
