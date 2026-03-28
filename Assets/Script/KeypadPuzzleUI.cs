using NavKeypad;
using System.Collections;
using TMPro;
using UnityEngine;

public class KeypadPuzzleUI : MonoBehaviour
{
    public KeypadDoorController keypad;
    public TMP_Text feedbackText;

    private string correctCode = "4910";

    void Start()
    {
        if (keypad != null)
        {
            keypad.SetKeypadCode(int.Parse(correctCode));

            keypad.OnAccessDenied.AddListener(ShowFeedback);
            keypad.OnAccessGranted.AddListener(ShowSuccess);
        }

        feedbackText.gameObject.SetActive(false);
    }

    void ShowFeedback()
    {
        string input = keypad.GetInput();

        if (input.Length != correctCode.Length)
        {
            ShowMessage("Enter 4 digits!");
            return;
        }

        int correctPosition = CountCorrectPositions(input, correctCode);
        int correctWrongPlace = CountCorrectWrongPlace(input, correctCode);

        ShowMessage(correctPosition + " correct position\n" +
                    correctWrongPlace + " correct but wrong place");
    }

    void ShowSuccess()
    {
        ShowMessage("Correct Code!");
    }

    // ✔ Correct digit AND correct position
    int CountCorrectPositions(string input, string answer)
    {
        int count = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == answer[i])
                count++;
        }

        return count;
    }

    // ✔ Correct digit BUT wrong position
    int CountCorrectWrongPlace(string input, string answer)
    {
        int count = 0;

        bool[] used = new bool[answer.Length];

        // First mark correct positions
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == answer[i])
                used[i] = true;
        }

        // Now check wrong positions
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == answer[i]) continue;

            for (int j = 0; j < answer.Length; j++)
            {
                if (!used[j] && input[i] == answer[j])
                {
                    count++;
                    used[j] = true;
                    break;
                }
            }
        }

        return count;
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