using UnityEngine;

public class GrowObject : MonoBehaviour
{
    [Tooltip("Target scale when fully grown")]
    public Vector3 targetScale = Vector3.one;

    [Tooltip("Time in seconds to grow to full size")]
    public float growDuration = 1f;

    private Vector3 initialScale;

    void Start()
    {
        // Store the target scale and start from zero
        initialScale = targetScale;
        transform.localScale = Vector3.zero;

        // Start the growth animation
        StartCoroutine(GrowOverTime());
    }

    private System.Collections.IEnumerator GrowOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < growDuration)
        {
            // Calculate progress (0 to 1)
            float t = elapsedTime / growDuration;

            // Smooth scaling
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final scale is exact
        transform.localScale = initialScale;
    }
}
