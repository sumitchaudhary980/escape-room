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
    [SerializeField] private UnityEvent onPuzzleSolved;

    [Header("Puzzle Configuration")]
    [SerializeField] private PuzzlePiece[] puzzlePieces = new PuzzlePiece[3];
    [SerializeField] private float rotationIncrement = 90f;

    [Header("UI Text")]
    [SerializeField] private string puzzleSolvedText = "Puzzle Solved!";
    [SerializeField] private TMP_Text puzzleStatusText;
    [SerializeField] private float textDisplayDuration = 2f;

    [Header("SoundFx")]
    [SerializeField] private AudioClip rotationClickSfx;
    [SerializeField] private AudioClip puzzleSolvedSfx;
    [SerializeField] private AudioSource audioSource;

    [Header("Player")]
    [SerializeField] private MonoBehaviour playerController;

    private int selectedPieceIndex = -1;
    private bool isInteracting = false;
    private bool puzzleSolved = false;

    private Coroutine textCoroutine;

    private void Update()
    {
        if (puzzleSolved) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isInteracting)
                SelectPainting();
            else
                ExitInteraction();
        }

        if (isInteracting)
        {
            HandleRotationInput();
        }
    }

    private void SelectPainting()
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

                    if (playerController != null)
                        playerController.enabled = false;

                    ShowText("A/D or ← → to rotate | Press E to exit");
                    Debug.Log("Selected: " + puzzlePieces[i].pieceName);
                    return;
                }
            }
        }
    }

    private void ExitInteraction()
    {
        isInteracting = false;
        selectedPieceIndex = -1;

        if (playerController != null)
            playerController.enabled = true;
        ShowText("Exited Painting");

    }

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

    private void PuzzleSolved()
    {
        puzzleSolved = true;

        ShowText(puzzleSolvedText);
        PlayPuzzleSolvedSfx();
        onPuzzleSolved?.Invoke();

        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("Puzzle Solved!");
    }

    private void ShowText(string msg)
    {
        if (puzzleStatusText == null) return;

        if (textCoroutine != null)
            StopCoroutine(textCoroutine);

        textCoroutine = StartCoroutine(TextRoutine(msg));
    }

    private IEnumerator TextRoutine(string msg)
    {
        puzzleStatusText.text = msg;

        yield return new WaitForSeconds(textDisplayDuration);

        puzzleStatusText.text = "";
    }

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