using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        Time.timeScale = 1f;
        yield return null; // Wait one frame
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Scene restarted");
    }
}