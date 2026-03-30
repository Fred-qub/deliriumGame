using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 200f;

    [Tooltip("Rotate in local space (true) or world space (false).")]
    public bool useLocalRotation = true;

    [Tooltip("Rotation axis (1 = rotate, 0 = no rotation).")]
    public Vector3 rotationAxis = Vector3.right;
    void Update()
    {
        // Validate speed and axis
        if (rotationAxis == Vector3.zero || Mathf.Approximately(rotationSpeed, 0f))
            return;

        // Calculate rotation for this frame
        float step = rotationSpeed * Time.deltaTime;

        // Apply rotation
        if (useLocalRotation)
        {
            transform.Rotate(rotationAxis, step, Space.Self);
        }
        else
        {
            transform.Rotate(rotationAxis, step, Space.World);
        }
    }
}
