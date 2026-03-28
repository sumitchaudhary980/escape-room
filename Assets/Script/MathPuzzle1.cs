using System.Collections;
using UnityEngine;
using TMPro;
using NavKeypad;

public class MathPuzzle1 : MonoBehaviour
{
    public KeypadDoorController keypad;
    public TMP_Text feedbackText;
    private bool puzzleSolved = false;
    void Start()
    {
        int answer = 5 + 7 * 2; // 19

        if (keypad != null)
        {
            keypad.SetKeypadCode(answer);

            // LISTEN to keypad events
            keypad.OnAccessDenied.AddListener(ShowHint);
            keypad.OnAccessGranted.AddListener(CorrectMessage);
        }

        feedbackText.gameObject.SetActive(false);
    }

    void ShowHint()
    {
        ShowMessage("Hint: Look at the painting 👀");
    }

    void CorrectMessage()
    {
        if (puzzleSolved) return;
        puzzleSolved=true;
        ShowMessage("Correct! Door opening...");
    }

    void ShowMessage(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(msg));
    }

    IEnumerator ShowRoutine(string msg)
    {
        feedbackText.text = msg;
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        feedbackText.gameObject.SetActive(false);
    }
}