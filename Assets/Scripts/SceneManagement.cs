using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public GameObject pauseUI;
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
        SceneManager.LoadScene("Menu");
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
}
