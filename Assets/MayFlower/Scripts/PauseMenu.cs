using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    

    // Update is called once per frame
    void Update()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause ();
        }
        
    }
    private void OnApplicationPause(bool pause)
    {
        
    }
    
    public void Resume()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }
}
