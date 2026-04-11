using UnityEngine;
using TMPro; // Required for TextMeshPro
using System.Collections;

public class NumberAnimator : MonoBehaviour
{
    // This script animates the numbers in the heartrate display UI, so the rate does not change immediately but will count up/down
    [Header("UI Reference")]
    public TextMeshProUGUI uiText; // Assign your TMP Text component here

    [Header("Animation Settings")]
    [Tooltip("Time in seconds for the animation")]
    public float animationDuration = 1.0f;

    private Coroutine animationCoroutine;


    // Starts animating the number from the current displayed value to the target value.

    public void AnimateTo(int targetValue)
    {
        // Stop any ongoing animation to avoid conflicts
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateNumber(targetValue));
    }

    private IEnumerator AnimateNumber(int targetValue)
    {
        // Parse current value from text (fallback to 0 if invalid)
        int startValue;
        if (!int.TryParse(uiText.text, out startValue))
            startValue = 0;

        float elapsed = 0f;

        while (elapsed < animationDuration) // while elapsed time is less than the set animation duration time
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            // Smooth interpolation (ease-out)
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));
            uiText.text = currentValue.ToString();

            yield return null;
        }

        // Ensure final value is set
        uiText.text = targetValue.ToString();
        animationCoroutine = null;
    }
}
