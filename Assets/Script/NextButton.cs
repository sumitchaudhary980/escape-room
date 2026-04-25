using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public void LoadNextRoom()
    {
        Debug.Log("LoadNextRoom called!");

        Time.timeScale = 1f;
        SceneManager.LoadScene("Room2");
    }
}