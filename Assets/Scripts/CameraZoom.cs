using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    // This script allows the zoom on right mouse click

    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCamera; // Assign in Inspector
    public float zoomedFOV = 8f;                   // Field of View when zoomed in
    public float normalFOV = 80f;                   // Default Field of View
    public float zoomSpeed = 5f;                    // How fast the zoom happens

    void Update()
    {
        // Hold Right Mouse Button to zoom
        bool isZoomingIn = Input.GetMouseButton(1);

        // Smoothly adjust FOV

        float targetFOV;

        if (isZoomingIn)
        {
            targetFOV = zoomedFOV;
        }
        else
        {
            targetFOV = normalFOV;
        }


        var lensSettings = virtualCamera.m_Lens; // copies camera lens settings
        lensSettings.FieldOfView = Mathf.Lerp( // changes lens settings - moves current field of view towards the target field of view
            lensSettings.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed // zoom speed is not dependent on frame rate
        );
        virtualCamera.m_Lens = lensSettings; // lens settings need to be written back to the camera
    }
}
