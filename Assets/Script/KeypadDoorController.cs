using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class KeypadDoorController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;

        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 1234;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f);
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f);
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f, 1f);

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        [Header("Optional Door Reference")]
        public DoorController door;

        private string currentInput = "";
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        private void Awake()
        {
            ClearInput();
            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void Update()
        {
            if (displayingResult || accessWasGranted) return;

            // Keyboard input 0-9
            for (KeyCode k = KeyCode.Alpha0; k <= KeyCode.Alpha9; k++)
            {
                if (Input.GetKeyDown(k))
                    AddInput(((int)(k - KeyCode.Alpha0)).ToString());
            }

            // Enter key
            if (Input.GetKeyDown(KeyCode.Return))
                AddInput("enter");

            // Backspace key
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (!string.IsNullOrEmpty(currentInput))
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    keypadDisplayText.text = currentInput;
                    PlayClickSfx();
                }
            }
        }

        // PUBLIC so UI buttons can call this
        public void AddInput(string input)
        {
            if (displayingResult || accessWasGranted) return;

            PlayClickSfx();

            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput.Length >= 9) return;
                    if (!char.IsDigit(input[0])) return; // allow digits only
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }
        }

        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }
        }

        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;

            if (!granted)
            {
                ClearInput();
                if (panelMesh != null)
                    panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            }
        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            if (audioSource != null && accessDeniedSfx != null)
                audioSource.PlayOneShot(accessDeniedSfx);

            if (door != null)
                door.CloseDoor();
        }

        private void ClearInput()
        {
            currentInput = "";
            if (keypadDisplayText != null)
                keypadDisplayText.text = currentInput;
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            if (audioSource != null && accessGrantedSfx != null)
                audioSource.PlayOneShot(accessGrantedSfx);

            if (door != null)
                door.OpenDoor();
        }

        public void PlayClickSfx()
        {
            if (audioSource != null && buttonClickedSfx != null)
                audioSource.PlayOneShot(buttonClickedSfx);
        }
    }
}