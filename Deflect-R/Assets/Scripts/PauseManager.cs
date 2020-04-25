using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    public bool isPaused;
    public string levelToLoadOnRestart;
    public string mainMenuSceneName;
    public PlayerBehaviour playerBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(!isPaused)
        {
            isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if(isPaused)
        {
            isPaused = false;
            pausePanel.SetActive(false);

            if(!playerBehaviour.GetTimeFrozen())
            {
                Time.timeScale = 1;
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoadOnRestart);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
