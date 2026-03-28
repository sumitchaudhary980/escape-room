using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleNote : MonoBehaviour
{
    [Header("Note Settings")]
    [SerializeField] private string noteText = "This is a note!";
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float expandScale = 1.5f;     // optional scale effect
    [SerializeField] private float expandDuration = 0.3f;

    [Header("References")]
    [SerializeField] private GameObject hintCanvas;       // UI hint ("Press E")
    [SerializeField] private TextMeshPro textDisplay;     // 3D text on paper

    private Vector3 originalScale;
    private bool isExpanded = false;
    private bool playerNearby = false;
    private Transform playerTransform;
    private Coroutine scaleCoroutine;

    void Start()
    {
        originalScale = transform.localScale;

        // Find player in scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        // Hide hint and text initially
        if (hintCanvas != null) hintCanvas.SetActive(false);
        if (textDisplay != null) textDisplay.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Show hint if player is near
        if (distance <= detectionRadius && !playerNearby)
        {
            playerNearby = true;
            if (hintCanvas != null) hintCanvas.SetActive(true);
        }
        else if (distance > detectionRadius && playerNearby)
        {
            playerNearby = false;
            if (hintCanvas != null) hintCanvas.SetActive(false);

            // Auto-close note if player walks away
            if (isExpanded) CollapseNote();
        }

        // Press E to interact
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isExpanded) CollapseNote();
            else ExpandNote();
        }
    }

    private void ExpandNote()
    {
        if (isExpanded) return;
        isExpanded = true;

        if (hintCanvas != null) hintCanvas.SetActive(false);

        if (textDisplay != null)
        {
            textDisplay.text = noteText;
            textDisplay.gameObject.SetActive(true);
        }

        // Optional scale animation
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScalePaper(originalScale * expandScale, expandDuration));
    }

    private void CollapseNote()
    {
        if (!isExpanded) return;
        isExpanded = false;

        if (textDisplay != null)
            textDisplay.gameObject.SetActive(false);

        // Optional scale animation
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScalePaper(originalScale, expandDuration));

        if (playerNearby && hintCanvas != null)
            hintCanvas.SetActive(true);
    }

    private IEnumerator ScalePaper(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}