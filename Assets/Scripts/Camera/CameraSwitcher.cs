using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Your main camera
    public CinemachineVirtualCamera bossCamera;    // Your Boss Camera

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Priority = 0; // Lower priority for the main camera
            bossCamera.Priority = 1;    // Higher priority for the Boss Camera
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Priority = 1; // Reset to higher priority when player leaves
            bossCamera.Priority = 0;    // Lower priority for the Boss Camera
        }
    }
}
