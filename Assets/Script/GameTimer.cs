using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeInMinutes = 5f;
    [SerializeField] private TMP_Text timerText;

    [Header("UI Panels")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    private float timeRemaining;
    private bool timerRunning = true;
    private bool hasWon = false;

    private Transform player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        timeRemaining = timeInMinutes * 60f;
        Time.timeScale = 1f;

        losePanel.SetActive(false);
        winPanel.SetActive(false);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (!timerRunning || hasWon) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            Lose();
            return;
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }

    // 🔥 Called from ExitZoneTrigger
    public void OnPlayerExitDoor()
    {
        if (hasWon) return;
        TriggerWin();
    }

    public void TriggerWin()
    {
        if (hasWon) return;

        hasWon = true;
        timerRunning = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        winPanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("WIN");
    }

    void Lose()
    {
        timerRunning = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}