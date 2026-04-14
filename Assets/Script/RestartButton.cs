using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1; // Resume time before reloading!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Scene restarted");
    }
}