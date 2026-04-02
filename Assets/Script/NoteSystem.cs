using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SojaExiles; 

public class NoteSystem : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteTextUI;
    [SerializeField] private GameObject notePaper;

    [Header("Note Content")]
    [TextArea]
    [SerializeField] private string noteContent = "Find d/dx of f(x) = x² at x = 3";

    [Header("Drawer Reference")]
    [SerializeField] private Drawer_Pull_Z drawerPull;

    private bool isReading = false;

    void Start()
    {
        // Validate required references
        if (player == null || drawerPull == null)
        {
            enabled = false;
            return;
        }

        // Initial UI state
        hintText?.gameObject.SetActive(false);
        notePanel?.SetActive(false);
        notePaper?.SetActive(true);

        if (notePanel != null && notePanel.TryGetComponent(out Image panelImage))
        {
            panelImage.color = new Color32(255, 255, 204, 255);
        }

        if (noteTextUI != null)
            noteTextUI.color = Color.black;
    }

    void Update()
    {
        // If drawer is closed → reset everything
        if (!drawerPull.open)
        {
            isReading = false;
            UpdateUI(false);
            hintText?.gameObject.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool isNear = distance < interactDistance;

        // Handle input
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            isReading = !isReading;
        }
        UpdateUI(isReading);
        // Update UI
        if (isNear)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = isReading ? "Press E to hide note" : "Press E to read note";
        }
        else
        {
            hintText.gameObject.SetActive(false);
        }
    }

    void UpdateUI(bool reading)
    {
        notePanel?.SetActive(reading);
        notePaper?.SetActive(!reading);

        if (reading && noteTextUI != null)
        {
            noteTextUI.text = noteContent;
        }
    }
}