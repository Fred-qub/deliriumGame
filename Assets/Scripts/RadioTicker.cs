using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private string clinicianMessage = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive tax rebates directly proportional to the amount of carbon they consume.  Video call provider Zoom has faced criticism for its new AI features, which allow users to send an artificially-generated version of themselves to attend meetings on their behalf.  Zoom's PR department have declined to comment, as the server malfunction has restricted access to its in-house AI model for the time being.";
    private string patientMessage = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive tax rebates directly proportional to the amount of carbon they consume.  But most importantly, hospital break-ins are at an all-time high, with reports of shadowy figures finding their way into the rooms of patients, particularly the elderly and patients recovering from surgery.  Anyone witnessing such figures should be made aware that they are NOT hallucinating, and are in fact in very real danger.  One such person is Arthur Roberts, who should be getting a wee visit from one as we speak.  Good luck Artie, cos I think you're gonna need it!";

    private float endX;
    private bool isScrolling = false;

    void OnEnable()
    {

        CheckSubtitleOption();

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

    void CheckSubtitleOption() 
    {
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
                CheckScene();
                break;
        }

    }

    void CheckScene() 
    {


        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Clinician Scene Ruth")
        {
            tickerText.text = clinicianMessage;

        }

        if (currentSceneName == "PatientScene Ruth")
        {
            tickerText.text = patientMessage;

        }

    }


}

