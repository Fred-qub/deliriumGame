using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    // Allows zooming into patient monitor on mouseover...have added collider to monitor and placed it on a zoomable objects layer.

    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCamera; // Assign in Inspector
    public float zoomedFOV = 30f;                   // Field of View when zoomed in
    public float normalFOV = 60f;                   // Default Field of View
    public float zoomSpeed = 5f;                    // How fast the zoom happens

    [Header("Raycast Settings")]
    public float rayDistance = 100f;                // Max raycast distance
    public LayerMask interactableLayers;            // Layers to detect

    private bool isZoomingIn = false;

    void Update()
    {
        // Ray from mouse position into the scene
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits an object in the interactable layers
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayers))
        {
            isZoomingIn = true;
        }
        else
        {
            isZoomingIn = false;
        }

        // Smoothly adjust FOV
        float targetFOV = isZoomingIn ? zoomedFOV : normalFOV;
        var lensSettings = virtualCamera.m_Lens;
        lensSettings.FieldOfView = Mathf.Lerp(lensSettings.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        virtualCamera.m_Lens = lensSettings;
    }
}
