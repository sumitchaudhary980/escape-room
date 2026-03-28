using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SojaExiles; // drawer namespace

public class NoteSystem : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("UI Elements")]
    public TMP_Text hintText;    // "Press E to Read"
    public GameObject notePanel; // Panel showing note text
    public TMP_Text noteTextUI;  // TMP Text inside panel
    public GameObject notePaper; // 3D paper object

    [Header("Note Content")]
    [TextArea]
    public string noteContent = "Find d/dx of f(x) = x² at x = 3";

    [Header("Drawer Reference")]
    public Drawer_Pull_Z drawerPull; // Drawer script reference

    private bool isNear = false;
    private bool isReading = false;

    void Start()
    {
        // Initial state
        if (hintText != null) hintText.gameObject.SetActive(false);
        if (notePanel != null) notePanel.SetActive(false);
        if (notePaper != null) notePaper.SetActive(true);

        // Set panel color
        if (notePanel != null)
        {
            Image panelImage = notePanel.GetComponent<Image>();
            if (panelImage != null)
                panelImage.color = new Color32(255, 255, 204, 255);
        }

        if (noteTextUI != null) noteTextUI.color = Color.black;
    }

    void Update()
    {
        if (player == null || drawerPull == null) return;

        // Distance check
        float distance = Vector3.Distance(transform.position, player.position);
        isNear = distance < interactDistance;

        // If drawer is closed, force everything hidden
        if (!drawerPull.open)
        {
            isReading = false; // Force state reset
            if (notePanel != null) notePanel.SetActive(false);
            if (notePaper != null) notePaper.SetActive(true);
            if (hintText != null) hintText.gameObject.SetActive(false);
            return;
        }

       
        // Show/hide hint based on distance and reading state
        if (hintText != null)
            hintText.gameObject.SetActive(isNear && !isReading);

        // Handle E key press to toggle note
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            isReading = true;

            // Apply the state
            if (notePanel != null) notePanel.SetActive(isReading);
            if (notePaper != null) notePaper.SetActive(!isReading);

            // Update text if showing
            if (isReading && noteTextUI != null)
                noteTextUI.text = noteContent;
        }

        if (isNear && Input.GetKeyDown(KeyCode.X))
        {
            isReading = false;
            if (notePanel != null) notePanel.SetActive(!isReading);
            if (notePaper != null) notePaper.SetActive(isReading);
           
        }

        
    }
}