using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemValue = 5; // Score value of this gem

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddGems(gemValue);
            Destroy(gameObject); 
        }
    }
}
