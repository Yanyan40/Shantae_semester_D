
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelChanger : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;
    // Update is called once per frame
    void FadeTo()
    {
        FadeToLevel(4);
    }
    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        FadeTo();
        SceneManager.LoadScene(levelToLoad);
    }
}
