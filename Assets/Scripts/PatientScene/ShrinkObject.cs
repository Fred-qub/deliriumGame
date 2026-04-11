using UnityEngine;
using System.Collections;

public class ShrinkObject : MonoBehaviour
{
   // This is used by both the shadow man hallucination and the newspaper on Arthur's table, to shrink them when required

    [Tooltip("Time in seconds for the object to shrink to zero.")]
    public float shrinkDuration = 1.0f;


    private Coroutine shrinkRoutine;

    public delegate void Shrink();
    public static event Shrink OnShrink;

   
    /// Starts shrinking the object to zero scale over time.
 
    public void StartShrinking()
    {
        // Prevent multiple coroutines from running at once
        if (shrinkRoutine != null)
            StopCoroutine(shrinkRoutine);

        OnShrink?.Invoke();
        
        if (transform.localScale == Vector3.zero) // if the item is already at zero scale, stop
        {
  
            return;
        }

        shrinkRoutine = StartCoroutine(ShrinkOverTime());
    }

    private IEnumerator ShrinkOverTime()
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < shrinkDuration) // while the time is less than the shrink duration time, run this code
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
