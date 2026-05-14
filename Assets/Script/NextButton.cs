using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public void LoadNextRoom()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Room2");
    }
}