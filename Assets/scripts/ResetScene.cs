using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public KeyCode resetKey = KeyCode.R; // Set the desired reset key in the Inspector.

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetCurrentScene();
        }
    }

    public void ResetCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
