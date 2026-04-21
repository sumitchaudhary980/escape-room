using UnityEngine;

public class TorchController : MonoBehaviour
{
    public Light torch;

    void Start()
    {
        torch.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            torch.enabled = !torch.enabled;
        }
    }
}