using System.Collections;
using UnityEngine;
using NavKeypad;

public class KeypadButtonWithSound : MonoBehaviour
{
    [Header("Value")]
    public string value;

    [Header("Animation Settings")]
    public float bttnspeed = 0.1f;
    public float moveDist = 0.0025f;
    public float buttonPressedTime = 0.1f;

    [Header("References")]
    public KeypadDoorController keypad;

    private bool moving;

    public void PressButton()
    {
        if (!moving && keypad != null)
        {
            keypad.PlayClickSfx(); // play click immediately
            keypad.AddInput(value); // add input to keypad
            StartCoroutine(MoveSmooth());
        }
    }

    private IEnumerator MoveSmooth()
    {
        moving = true;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0, moveDist);

        float elapsedTime = 0;
        while (elapsedTime < bttnspeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / bttnspeed);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.localPosition = endPos;
        yield return new WaitForSeconds(buttonPressedTime);

        startPos = transform.localPosition;
        endPos = startPos - new Vector3(0, 0, moveDist);
        elapsedTime = 0;

        while (elapsedTime < bttnspeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / bttnspeed);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.localPosition = endPos;
        moving = false;
    }
}