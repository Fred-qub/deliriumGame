
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    // This script makes the camera zoom on right mouse click.  ZOom can be controlled with the mouse wheel.

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Zoom Toggle")]
    public float zoomedFOV = 20f; // Field of view when first zoomed in
    public float normalFOV = 80f; // Field of view when zoomed out
    public float zoomSpeed = 5f; // How fast zoom happens

    [Header("Mouse Wheel Zoom")]
    public float scrollZoomSpeed = 20f;
    public float minFOV = 3f; // greatest zoom in you can do with wheel
    public float maxFOV = 90f; // greatest zoom out you can do with wheel

    private bool isZoomed = false;
    private float targetFOV;

    void Start()
    {
        if (!virtualCamera) return;
        targetFOV = normalFOV;
    }

    void Update()
    {
        if (!virtualCamera) return;

        // ? Toggle zoom with Right Mouse Button
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
            targetFOV = isZoomed ? zoomedFOV : normalFOV;
        }

        // ? Mouse wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetFOV -= scroll * scrollZoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
        }

        // ? Smooth FOV transition
        var lens = virtualCamera.m_Lens; // copies camera lens settings
        lens.FieldOfView = Mathf.Lerp(  // changes lens settings - moves current field of view towards the target field of view
            lens.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed  // zoom speed is not dependent on frame rate
        );
        virtualCamera.m_Lens = lens; // lens settings need to be written back to the camera
    }
}

