using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PaintingPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class PuzzlePiece
    {
        public Transform pieceTransform;
        public float correctRotation;
        public float rotationTolerance = 5f;
        public string pieceName = "Piece";
    }

    [Header("Events")]
    [SerializeField] private UnityEvent onPuzzleSolved; // 🔥 USE THIS FOR DOOR

    [Header("Puzzle Configuration")]
    [SerializeField] private PuzzlePiece[] puzzlePieces = new PuzzlePiece[3];
    [SerializeField] private float rotationIncrement = 90f;

    [Header("UI Text")]
    [SerializeField] private string puzzleSolvedText = "Puzzle Solved!";
    [SerializeField] private TMP_Text puzzleStatusText;

    [Header("Feedback UI")]
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 2f;

    [Header("SoundFx")]
    [SerializeField] private AudioClip rotationClickSfx;
    [SerializeField] private AudioClip puzzleSolvedSfx;
    [SerializeField] private AudioSource audioSource;

    // 🔥 Interaction
    private int selectedPieceIndex = -1;
    private bool isInteracting = false;

    private bool puzzleSolved = false;
    private Coroutine feedbackCoroutine;

    private void Awake()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);

        UpdateStatusText("Look at a painting and press E");
    }

    private void Update()
    {
        if (puzzleSolved) return;

        // 🔥 Press E to select / deselect
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isInteracting)
                TrySelectPainting();
            else
                ExitInteraction();
        }

        // 🔄 Rotate while interacting
        if (isInteracting)
        {
            HandleRotationInput();
        }
    }

    // ✅ SELECT PAINTING
    private void TrySelectPainting()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, 4f))
        {
            for (int i = 0; i < puzzlePieces.Length; i++)
            {
                if (hit.collider.transform == puzzlePieces[i].pieceTransform ||
                    hit.collider.transform.IsChildOf(puzzlePieces[i].pieceTransform))
                {
                    selectedPieceIndex = i;
                    isInteracting = true;

                    ShowFeedback("A/D or ← → to rotate | Press E to exit");
                    Debug.Log("Selected: " + puzzlePieces[i].pieceName);
                    return;
                }
            }
        }
    }

    // ✅ EXIT
    private void ExitInteraction()
    {
        selectedPieceIndex = -1;
        isInteracting = false;

        ShowFeedback("Exited painting");
    }

    // ✅ ROTATION INPUT
    private void HandleRotationInput()
    {
        if (selectedPieceIndex == -1) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotatePuzzlePiece(selectedPieceIndex, -rotationIncrement);
            PlayRotationSfx();
            CheckPuzzleSolved();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotatePuzzlePiece(selectedPieceIndex, rotationIncrement);
            PlayRotationSfx();
            CheckPuzzleSolved();
        }
    }

    // ✅ ROTATE
    public void RotatePuzzlePiece(int index, float amount)
    {
        if (index < 0 || index >= puzzlePieces.Length) return;

        Transform piece = puzzlePieces[index].pieceTransform;
        if (piece == null) return;

        Vector3 rot = piece.eulerAngles;
        rot.z += amount;
        rot.z = Mathf.Repeat(rot.z, 360f);

        piece.eulerAngles = rot;

        Debug.Log($"{puzzlePieces[index].pieceName} → {rot.z}°");
    }

    // ✅ CHECK SOLUTION
    public void CheckPuzzleSolved()
    {
        bool allCorrect = true;

        foreach (PuzzlePiece piece in puzzlePieces)
        {
            float current = Mathf.Repeat(piece.pieceTransform.eulerAngles.z, 360f);
            float target = Mathf.Repeat(piece.correctRotation, 360f);

            float diff = Mathf.Abs(current - target);
            if (diff > 180f) diff = 360f - diff;

            if (diff > piece.rotationTolerance)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            PuzzleSolved();
        }
    }

    // ✅ SOLVED → TRIGGER EVENT
    private void PuzzleSolved()
    {
        puzzleSolved = true;

        UpdateStatusText(puzzleSolvedText);
        ShowFeedback("Puzzle Solved!");
        PlayPuzzleSolvedSfx();

        onPuzzleSolved?.Invoke(); // 🔥 CALLS DOOR OPEN

        Debug.Log("Puzzle Solved!");
    }

    // ✅ UI
    private void UpdateStatusText(string msg)
    {
        if (puzzleStatusText != null)
            puzzleStatusText.text = msg;
    }

    public void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(FeedbackRoutine(message));
    }

    private IEnumerator FeedbackRoutine(string msg)
    {
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = msg;

        yield return new WaitForSeconds(feedbackDuration);

        feedbackText.gameObject.SetActive(false);
    }

    // ✅ SOUND
    private void PlayRotationSfx()
    {
        if (audioSource != null && rotationClickSfx != null)
            audioSource.PlayOneShot(rotationClickSfx);
    }

    private void PlayPuzzleSolvedSfx()
    {
        if (audioSource != null && puzzleSolvedSfx != null)
            audioSource.PlayOneShot(puzzleSolvedSfx);
    }
}