using UnityEngine;
using System.Collections;

public class ShrinkObject : MonoBehaviour
{
   
    [Tooltip("Time in seconds for the object to shrink to zero.")]
    public float shrinkDuration = 1.0f;

    [Tooltip("Automatically start shrinking on play.")]
    public bool shrinkOnStart = true;

    private Coroutine shrinkRoutine;

    void Start()
    {
        if (shrinkOnStart)
            StartShrinking();
    }

   
    /// Starts shrinking the object to zero scale over time.
 
    public void StartShrinking()
    {
        // Prevent multiple coroutines from running at once
        if (shrinkRoutine != null)
            StopCoroutine(shrinkRoutine);

        if (transform.localScale == Vector3.zero)
        {
  
            return;
        }

        shrinkRoutine = StartCoroutine(ShrinkOverTime());
    }

    private IEnumerator ShrinkOverTime()
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / shrinkDuration);

            // Smooth shrink
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null; // Wait for next frame
        }

        // Ensure exact zero at the end
        transform.localScale = Vector3.zero;
        shrinkRoutine = null;
    }

}
