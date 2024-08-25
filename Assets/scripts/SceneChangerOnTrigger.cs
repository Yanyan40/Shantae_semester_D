using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnTrigger : MonoBehaviour
{
    public Animator animator;  // Reference to Animator component

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the player enters the trigger
        {
        }
    }

}
