using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class NarrationLine
{
    public string subtitle;
    public float duration;
}

public class NarrationManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource narrationSource;

    [Header("Clips")]
    [SerializeField] private AudioClip narrationClip;

    [Header("UI")]
    [SerializeField] private TMP_Text subtitleText;

    [Header("Music Settings")]
    [SerializeField] private float normalVolume = 1f;
    [SerializeField] private float lowVolume = 0.3f;

    [Header("Narration Sequence")]
    [SerializeField] private NarrationLine[] narrationLines;

    void Start()
    {
        musicSource.volume = normalVolume;
        musicSource.Play();

        StartCoroutine(PlayNarrationSequence());
    }

    IEnumerator PlayNarrationSequence()
    {
        // lower music during narration
        musicSource.volume = lowVolume;

        yield return new WaitForSeconds(1f);

        // play narration audio
        narrationSource.clip = narrationClip;
        narrationSource.Play();

        float timer = 0f;

        for (int i = 0; i < narrationLines.Length; i++)
        {
            yield return StartCoroutine(TypeSentence(narrationLines[i].subtitle));

            yield return new WaitForSeconds(narrationLines[i].duration);

            timer += narrationLines[i].duration;
        }

        subtitleText.text = "";

        
        float remainingTime = narrationClip.length - timer;
        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);

     
        musicSource.volume = normalVolume;
    }

    IEnumerator TypeSentence(string sentence)
    {
        subtitleText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            subtitleText.text += letter;
            yield return new WaitForSeconds(0.03f); 
        }
    }
}