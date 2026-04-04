using TMPro;
using UnityEngine;

public class RadioTicker : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI tickerText; // The text to scroll
    public RectTransform textRect;     // RectTransform of the text
    public RectTransform maskRect;     // RectTransform of the mask area

    [Header("Settings")]
    public float scrollSpeed = 200f;   // Pixels per second
    private string message = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive a tax rebate directly proportional to the amount of carbon they consume.  Video call provider Zoom has faced criticism for its new AI features, which allow users to send an artificially-generated version of themselves to attend meetings on their behalf.  Zoom's PR department have declined to comment, as the server malfunction has restricted access to its in-house AI model for the time being.";

    private float startX;
    private float endX;

    void Start()
    {
        if (tickerText == null || textRect == null || maskRect == null)
        {
            Debug.LogError("ScrollingTicker: Missing references in Inspector.");
            enabled = false;
            return;
        }

        // Set the text
        tickerText.text = message;

        // Calculate start and end positions
        startX = maskRect.rect.width;
        endX = -textRect.rect.width;

        // Start at the right edge
        textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
    }

    void Update()
    {
        // Move text left
        textRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

    }

    /// <summary>
    /// Dynamically updates the ticker message.
    /// </summary>
    public void SetMessage(string newMessage)
    {
        message = newMessage;
        tickerText.text = message;
        endX = -textRect.rect.width;
        textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
    }
}

