using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    
    private void Start()
    {


    }
    public void OnBackButton()
    {
        Debug.Log("Back button clicked, loading MenuScene...");
        SceneManager.LoadScene("MenuScene");
    }

    public void OnExitGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}