using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;
    public Animator animator;
    public AudioSource audioSource;
    public TMP_Text subtitleText;

    [Header("Interaction UI")]
    public TMP_Text interactText; 

    [System.Serializable]
    public class SubtitleLine
    {
        public string text;
        public float duration;
    }

    public List<SubtitleLine> subtitles = new List<SubtitleLine>();

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private bool isTalking = false;

    void Start()
    {
        if (subtitleText != null)
            subtitleText.text = "";

        if (interactText != null)
            interactText.gameObject.SetActive(false); 
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && !isTalking)
        {
            if (interactText != null)
            {
                interactText.gameObject.SetActive(true);
                interactText.text = "Press E to Interact";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(StartDialogue());
            }
        }
        else
        {
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }

    IEnumerator StartDialogue()
    {
        isTalking = true;

        if (interactText != null)
            interactText.gameObject.SetActive(false);

        animator.SetBool("isTalking", true);
        subtitleText.text = "";

        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();

        foreach (SubtitleLine line in subtitles)
        {
            yield return StartCoroutine(TypeSentence(line.text));
            yield return new WaitForSeconds(line.duration);
        }

        EndDialogue();
    }

    IEnumerator TypeSentence(string sentence)
    {
        subtitleText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            subtitleText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isTalking", false);
        subtitleText.text = "";
        isTalking = false;
    }
}