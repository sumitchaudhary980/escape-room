using UnityEngine;
using TMPro;
using NavKeypad;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeInMinutes = 5f;
    [SerializeField] private TMP_Text timerText;

    [Header("UI Panels")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    [Header("Door Reference")]
    [SerializeField] private DoorController door; 

    private float timeRemaining;
    private bool timerRunning = true;
    private bool hasWon = false;

    void Start()
    {
        timeRemaining = timeInMinutes * 60f;

        timerRunning = true;
        hasWon = false;

        Time.timeScale = 1f;

        if (losePanel != null) losePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }
    void Update()
    {
        if (!timerRunning) return;

        if (!hasWon && door != null && door.open)
        {
            Win();
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
            UpdateTimerDisplay();
            Lose();
            return;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);

        if (timeRemaining <= 60)
            timerText.color = Color.red;
    }

    void Win()
    {
        hasWon = true;
        timerRunning = false;

        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}