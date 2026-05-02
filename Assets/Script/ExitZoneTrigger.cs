using UnityEngine;
using NavKeypad;

public class ExitZoneTrigger : MonoBehaviour
{
    public GameTimer gameTimer;
    public DoorController finalDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (finalDoor != null && finalDoor.open)
        {
            gameTimer.OnPlayerExitDoor();
        }
    }
}