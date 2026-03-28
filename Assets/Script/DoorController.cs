using System.Collections;
using UnityEngine;

namespace NavKeypad
{
    public class DoorController : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;

        [Header("Keypad Reference")]
        public Keypad keypad; // Drag your keypad here in inspector

        void Start()
        {
            open = false;

            // Subscribe to keypad event if assigned
            if (keypad != null)
            {
                keypad.OnAccessGranted.AddListener(OpenDoor); // opens door when keypad grants access
            }
        }

        // Public method visible in UnityEvent Inspector
        public void OpenDoor()
        {
            if (!open)
            {
                StartCoroutine(opening());
            }
        }

        public void CloseDoor()
        {
            if (open)
            {
                StartCoroutine(closing());
            }
        }
        public IEnumerator opening()
        {
            print("Door is opening");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        public IEnumerator closing()
        {
            print("Door is closing");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}