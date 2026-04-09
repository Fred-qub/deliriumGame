using UnityEngine;
using TMPro; // Required for TextMeshPro
using System.Collections;

public class NumberAnimator : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI uiText; // Assign your TMP Text component here

    [Header("Animation Settings")]
    [Tooltip("Time in seconds for the animation")]
    public float animationDuration = 1.0f;

    private Coroutine animationCoroutine;

    /// <summary>
    /// Starts animating the number from the current displayed value to the target value.
    /// </summary>
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

        while (elapsed < animationDuration)
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
