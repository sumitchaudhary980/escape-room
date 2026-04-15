using NavKeypad;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeInMinutes = 5f;
    [SerializeField] private TMP_Text timerText;

    [Header("UI Panels")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    [Header("Final Door")]
    [SerializeField] private DoorController finalDoor;

    [Header("Win Settings")]
    [SerializeField] private float winDistance = 3f;
    [SerializeField] private float exitDistance = 5f; 

    private float timeRemaining;
    private bool timerRunning = true;
    private bool hasWon = false;

    private Transform player;

    void Start()
    {
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

        HandleWinCheck();

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            Lose();
            return;
        }

        UpdateTimer();
    }

    void HandleWinCheck()
    {
        if (finalDoor == null || player == null) return;

        if (!finalDoor.open) return;

        float distance = Vector3.Distance(player.position, finalDoor.transform.position);

        if (distance >= exitDistance)
        {
            TriggerWin();
        }
    }

    void UpdateTimer()
    {
        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = $"{min:00}:{sec:00}";
    }

    public void TriggerWin()
    {
        if (hasWon) return;

        hasWon = true;
        timerRunning = false;

        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        timerRunning = false;
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}