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
        [SerializeField] private int keypadCombo;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Hint / Feedback")]
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private float feedbackDuration = 2f;

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

        [Header("Raycast Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask keypadLayer;

        private string currentInput = "";
        private bool displayingResult = false;
        private bool accessWasGranted = false;
        private Coroutine hintCoroutine;

        private void Awake()
        {
            ClearInput();

            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (displayingResult || accessWasGranted) return;

            if (!IsLookingAtKeypad())
                return;

            for (KeyCode k = KeyCode.Alpha0; k <= KeyCode.Alpha9; k++)
            {
                if (Input.GetKeyDown(k))
                    AddInput(((int)(k - KeyCode.Alpha0)).ToString());
            }

            if (Input.GetKeyDown(KeyCode.Return))
                AddInput("enter");

            if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                keypadDisplayText.text = currentInput;
                PlayClickSfx();
            }
        }

      
        private bool IsLookingAtKeypad()
        {
            if (playerCamera == null) return false;

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance, keypadLayer))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    return true;
                }
            }

            return false;
        }

        public void SetKeypadCode(int code) => keypadCombo = code;

        public string GetInput() => currentInput;

        public void AddInput(string input)
        {
            if (displayingResult || accessWasGranted) return;

            PlayClickSfx();

            if (input == "enter")
            {
                CheckCombo();
                return;
            }

            if (currentInput.Length >= 9 || !char.IsDigit(input[0])) return;

            currentInput += input;
            keypadDisplayText.text = currentInput;
        }

        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out int currentKombo))
            {
                bool granted = currentKombo == keypadCombo;

                if (!displayingResult)
                    StartCoroutine(DisplayResultRoutine(granted));
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

            if (feedbackText != null)
            {
                if (hintCoroutine != null) StopCoroutine(hintCoroutine);
                feedbackText.gameObject.SetActive(false);
            }
        }

        public void ShowHint(string message)
        {
            if (feedbackText == null) return;

            if (hintCoroutine != null)
                StopCoroutine(hintCoroutine);

            hintCoroutine = StartCoroutine(ShowHintRoutine(message));
        }

        private IEnumerator ShowHintRoutine(string message)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = message;

            yield return new WaitForSeconds(feedbackDuration);

            feedbackText.gameObject.SetActive(false);
            hintCoroutine = null;
        }

        private void ClearInput()
        {
            currentInput = "";

            if (keypadDisplayText != null)
                keypadDisplayText.text = currentInput;
        }

        public void PlayClickSfx()
        {
            if (audioSource != null && buttonClickedSfx != null)
                audioSource.PlayOneShot(buttonClickedSfx);
        }
    }
}