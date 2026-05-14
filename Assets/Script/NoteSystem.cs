using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteSystem : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;

    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteTextUI;
    [SerializeField] private GameObject notePaper;

    [Header("Note Content")]
    [TextArea]
    [SerializeField] private string noteContent = "";

    private bool isReading = false;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
            enabled = false;
            return;
        }

        if (notePanel != null) notePanel.SetActive(false);
        if (notePaper != null) notePaper.SetActive(true);

        if (notePanel != null && notePanel.TryGetComponent(out Image panelImage))
        {
            panelImage.color = new Color32(255, 255, 204, 255);
        }

        if (noteTextUI != null)
            noteTextUI.color = Color.black;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool isNear = distance < interactDistance;

        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            isReading = !isReading;
            UpdateUI(isReading);
        }

        if (!isNear && isReading)
        {
            isReading = false;
            UpdateUI(false);
        }
    }

    void UpdateUI(bool reading)
    {
        if (notePanel != null)
            notePanel.SetActive(reading);

        if (notePaper != null)
            notePaper.SetActive(!reading);

        if (reading && noteTextUI != null)
        {
            noteTextUI.text = noteContent;
        }
    }
}