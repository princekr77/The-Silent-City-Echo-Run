using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject exitMenu;
    public GameObject mainMenu;
    public GameObject gameOver;
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void Quit()
    {
        Application.Quit();

        Debug.Log("Game is exiting...");
    }


    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    
     public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);

    }    

    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        //Time.timeScale = 1;
        mainMenu.SetActive(false);
        exitMenu.SetActive(true);
    }

    public void Back()
    {
        //Time.timeScale = 1;
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void showGameOver()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }
}
