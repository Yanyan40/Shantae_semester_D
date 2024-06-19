using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main_menu : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource hoverAudioSource; // Reference to the AudioSource for hover sound
    public AudioSource clickAudioSource; // Reference to the AudioSource for click sound
    public Animation animationComponent; // Reference to the Animation component
    public string animationName; // Name of the animation to play
    public float delay = 2.0f; // Delay time in seconds

    void Start()
    {
        // Ensure the AudioSource components are assigned in the Inspector
        if (hoverAudioSource == null || clickAudioSource == null)
        {
            Debug.LogError("AudioSource components not assigned!");
        }

        // Ensure the Animation component is assigned in the Inspector
        if (animationComponent == null)
        {
            Debug.LogError("Animation component not assigned!");
        }
    }

    public void PlayGame()
    {
        PlayClickSound();
        animationComponent.Play(animationName); // Play the animation
        StartCoroutine(LoadSceneAfterAnimation("File_select"));
    }

    public void QuitGame()
    {
        PlayClickSound();
        StartCoroutine(QuitGameAfterDelay());
    }

    public void Home()
    {
        PlayClickSound();
        StartCoroutine(LoadSceneAfterDelay("Main_menu"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void PlayHoverSound()
    {
        if (hoverAudioSource != null)
        {
            hoverAudioSource.Play();
        }
    }

    public void PlayClickSound()
    {
        if (clickAudioSource != null)
        {
            clickAudioSource.Play();
        }
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneAfterAnimation(string sceneName)
    {
        // Wait for the animation to finish
        while (animationComponent.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitGameAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
