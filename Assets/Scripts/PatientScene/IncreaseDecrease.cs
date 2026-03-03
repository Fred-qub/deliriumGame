using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class IncreaseDecrease : MonoBehaviour
{
    
    [Header("Value Settings")]
    public float minValue = 0f;       // Minimum value
    public float maxValue = 10f;      // Maximum value
    public float cycleDuration = 2f;  // Time (seconds) to go from min to max and back

    [Header("Debug")]
    public float currentValue;        // Current value (read-only in Inspector)

    void Update()
    {
        // Protect against division by zero
        if (cycleDuration <= 0f)
        {
            Debug.LogWarning("Cycle duration must be greater than zero.");
            return;
        }

        // Mathf.PingPong returns a value that goes back and forth between 0 and length
        float t = Mathf.PingPong(Time.time, cycleDuration / 2f) / (cycleDuration / 2f);

        // Lerp between min and max based on t
        currentValue = Mathf.Lerp(minValue, maxValue, t);

        // Example: Apply to object's position (optional)
        // transform.position = new Vector3(currentValue, transform.position.y, transform.position.z);
    }
}


