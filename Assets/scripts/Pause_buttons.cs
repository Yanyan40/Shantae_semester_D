using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause_buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource hoverAudioSource; // Reference to the AudioSource for hover sound
    public AudioSource clickAudioSource; // Reference to the AudioSource for click sound

    void Start()
    {
        // Ensure the AudioSource components are assigned in the Inspector
        if (hoverAudioSource == null || clickAudioSource == null)
        {
            Debug.LogError("AudioSource components not assigned!");
        }
    }

    

    public void Continue()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void Guide()
    {
        PlayClickSound();
        SceneManager.LoadScene("Tutorial_guide");
    }

    public void Home()
    {
        PlayClickSound();
        SceneManager.LoadScene("Main_menu");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Additional actions when the pointer exits the button (if needed)
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
}
