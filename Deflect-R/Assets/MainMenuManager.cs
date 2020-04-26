using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField, Tooltip("The name of the scene to be started by the Start Game button.")] public string startScene;

    [SerializeField, Tooltip("The audio source that will play the main menu music.")] public AudioSource musicSource;
    [SerializeField, Tooltip("The main menu music sound file.")] public AudioClip musicClip;

    [SerializeField, Tooltip("The audio source that will play main menu sound effects.")] public AudioSource soundEffectSource;
    [SerializeField, Tooltip("The button click sound effect music file.")] public AudioClip soundEffectClip;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
    
    /// <summary>
    /// To be called by button, plays a button click sound upon clicking the button
    /// </summary>
    public void ButtonClick()
    {
        soundEffectSource.clip = soundEffectClip;
        soundEffectSource.Play();
    }

    /// <summary>
    /// To be called by button, loads startScene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    /// <summary>
    /// To be called by button, quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
