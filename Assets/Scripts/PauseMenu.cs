using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    //public : accessible from other scripts;
    //static : we just want to check if the game is currently paused, we don't want to reference this specific PauseMenu script;
    //bool : true or false :: Is the game Paused or Not?;
    //false by default, our game isn't paused by default;

    public GameObject pauseMenuUI;
    //This GameObject will reffer to our PauseMenu Canvas;

    private void OnEnable()
    {
        var pauseController = new PlayerController();
        pauseController.Enable();
        pauseController.Main.Pause.performed += PauseOnPerformed;
    }

    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        if (GameIsPaused)
        {
            Resume();
            //Pressing the Pause Key (here "Esc") when the game is already paused will automatically Resume it;
        }
        else
        {
            Pause();
            //Pressing the Pause Key when the game is running will Pause the game;
        }
    }


    void Update()
    {
    }


    public void Resume()
    {
        //we want take away PauseMenu;
        //we want to set time back to normal;
        //we want to put GameIsPaused to false;
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        //We set the timespeed as its default value;

        GameIsPaused = false;
    }

    void Pause()
    //we want to bring up PauseMenu;
    //we want to freeze time in our game;
    //we want to put GameIsPaused to true;
    {
        pauseMenuUI.SetActive(true);
        //enables our Game Object, here the whole PauseMenu Canvas;

        Time.timeScale = 0f;
        //Time.timeScale = the speed at which time is passing;
        //it can be used to create slowmotion effects;
        //we set to 0 in order to freeze the game;

        GameIsPaused = true;
    }

    public void MainMenu()
    {
        //this function will allow us to open the Menu scene;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
