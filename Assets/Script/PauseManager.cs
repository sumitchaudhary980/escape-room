using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] private GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pauseMenu.SetActive(true);

            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            ResumeGame();
        }
    }

    
    public void OnClickResume()
    {
        ResumeGame();
    }

    void ResumeGame()
    {
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MenuScene"); 
    }

    public void OnClickExit()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Debug.Log("Game Quit"); 
        Application.Quit(); 
    }
}