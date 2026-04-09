using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuPanelCanvasGroup;
    [SerializeField] private CanvasGroup controlsPanelCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup currentScreen;

    private void Start()
    {
        ShowScreen(menuPanelCanvasGroup);
    }

    private void ShowScreen(CanvasGroup screen)
    {
        StartCoroutine(FadeToScreen(screen));
    }

    private IEnumerator FadeToScreen(CanvasGroup targetScreen)
    {
        // Fade out current
        if (currentScreen != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(currentScreen, 1f, 0f, fadeDuration));
            currentScreen.interactable = false;
            currentScreen.blocksRaycasts = false;
        }

        // Switch
        currentScreen = targetScreen;
        currentScreen.interactable = true;
        currentScreen.blocksRaycasts = true;

        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(currentScreen, 0f, 1f, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        cg.alpha = endAlpha;
    }

    public void OnPlayGameButton()
    {
        // Load your existing game scene
        SceneManager.LoadScene("Room1");
    }

    public void OnViewControlsButton()
    {
        ShowScreen(controlsPanelCanvasGroup);
    }

    public void OnBackButton()
    {
        ShowScreen(menuPanelCanvasGroup);
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