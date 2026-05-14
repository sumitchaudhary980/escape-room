using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}