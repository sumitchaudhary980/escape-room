using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
   
    private void Start()
    {
        
    }
    public void OnRoom1Button()
    {
        SceneManager.LoadScene("Room1");
    }

    public void OnRoom2Button()
    {
        SceneManager.LoadScene("Room2");
    }

    public void OnRoom3Button()
    {
        SceneManager.LoadScene("Room3");
    }
    public void OnBackButton()
    {
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