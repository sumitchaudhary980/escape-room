using System.Collections;
using UnityEngine;

namespace NavKeypad
{
    public class DoorController : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;

        
        void Start()
        {
            open = false;

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