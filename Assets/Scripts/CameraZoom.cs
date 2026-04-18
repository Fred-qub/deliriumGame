using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCamera; // Assign in Inspector
    public float zoomedFOV = 30f;                   // Field of View when zoomed in
    public float normalFOV = 60f;                   // Default Field of View
    public float zoomSpeed = 5f;                    // How fast the zoom happens

    void Update()
    {
        // Hold Right Mouse Button to zoom
        bool isZoomingIn = Input.GetMouseButton(1);

        // Smoothly adjust FOV
        float targetFOV = isZoomingIn ? zoomedFOV : normalFOV;

        var lensSettings = virtualCamera.m_Lens;
        lensSettings.FieldOfView = Mathf.Lerp(
            lensSettings.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
        virtualCamera.m_Lens = lensSettings;
    }
}
