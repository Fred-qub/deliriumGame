using TMPro;
using UnityEngine;

public class RadioTicker : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI tickerText; // The text to scroll
    public RectTransform textRect;     // RectTransform of the text
    public RectTransform maskRect;     // RectTransform of the mask area
    public GameObject radioSubtitle;
    public int subtitles;
    public string textColour;

    [Header("Settings")]
    private float scrollSpeed = 250f;   // Pixels per second
    [TextArea]
    private string message = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive a tax rebate directly proportional to the amount of carbon they consume.  Video call provider Zoom has faced criticism for its new AI features, which allow users to send an artificially-generated version of themselves to attend meetings on their behalf.  Zoom's PR department have declined to comment, as the server malfunction has restricted access to its in-house AI model for the time being.";

    private float endX;
    private bool isScrolling = false;

    void OnEnable()
    {
        if (tickerText == null || textRect == null || maskRect == null)
        {
            Debug.LogError("OneShotTicker: Missing references in Inspector.");
            enabled = false;
            return;
        }

        if (PlayerPrefs.HasKey("Subtitles"))
        {
           subtitles = PlayerPrefs.GetInt("Subtitles");
        }
        else subtitles = 0;

        switch (subtitles)
        {
            case 0:
                radioSubtitle.SetActive(false);
                break;

            case 1:

                radioSubtitle.SetActive(true);
                LoadColourScheme();
                break;
        }

        

        // Set the text
        tickerText.text = message;

        // Force update to get correct width
        tickerText.ForceMeshUpdate();

        // Start position: just outside the right edge of the mask
        float startX = maskRect.rect.width;
        textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);

        // End position: fully off-screen to the left
        endX = -tickerText.preferredWidth;

        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling) return;

        // Move text left
        textRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // Check if text has fully passed
        if (textRect.anchoredPosition.x <= endX)
        {
            isScrolling = false;
            radioSubtitle.SetActive(false); // Disable the GameObject
        }
    }

    /// <summary>
    /// Call this to start the ticker with a new message.
    /// </summary>
    public void StartTicker(string newMessage)
    {
        message = newMessage;
        radioSubtitle.SetActive(true); // This will trigger OnEnable and restart
    }

    private void LoadColourScheme()
    {
        if (PlayerPrefs.HasKey("OutlineScheme"))
        {
            textColour = PlayerPrefs.GetString("OutlineScheme");
        }
        else textColour = "Standard";


        switch (textColour)
        {
            case "Standard":
                Standard();
                break;

            case "Protanopia":
                Protanopia();
                break;

            case "Deuteranopia":
                Deuteranopia();
                break;

            case "Tritanopia":
                Tritanopia();
                break;

        }

    }

    public void Standard() 
    {
        tickerText.color = Color.red;
    
    }

    public void Protanopia()
    {
        tickerText.color = Color.yellow;

    }

    public void Deuteranopia()
    {
        tickerText.color = Color.yellow;

    }

    public void Tritanopia()
    {
        tickerText.color = Color.red;

    }


}

