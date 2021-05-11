using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // PauseMenu script in canvas because
    // we cant activate a script form initially disabled GameObject
    public static bool isGamePaused = false;
    private GameObject crosshair;

    [SerializeField] GameObject pauseMenu;

    private void Awake()
    {
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
                crosshair.SetActive(true);
            }
            else
            {
                PauseGame();
                crosshair.SetActive(false);
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quitter..uhmm.. Oh wait yea tsch yeah ..Quit. We don't frown upon quitters, I forgot..");
    }
}
