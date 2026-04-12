using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class RadioTicker : MonoBehaviour
{

    //This script runs the radio subtitles, if subtitles are switched on in the options menu - handles message and colour

    [Header("UI References")]
    public TextMeshProUGUI tickerText; // The text to scroll
    public RectTransform textRect;     // RectTransform of the text
    public RectTransform maskRect;     // RectTransform of the mask area
    public GameObject radioSubtitle;
    public bool subtitles;
    public int textColour;

    [Header("Settings")]
    private float scrollSpeed = 300f;   // scroll speed in pixels per second
    [TextArea]
    private string normalMessage = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive tax rebates directly proportional to the amount of carbon they consume.  Video call provider Zoom has faced criticism for its new AI features, which allow users to send an artificially-generated version of themselves to attend meetings on their behalf.  Zoom's PR department have declined to comment, as the server malfunction has restricted access to its in-house AI model for the time being.";
    private string hallucinationMessage = "RADIO ANNOUNCER: It is the top of the hour, you are listening to DSFM with me, Sam Todd.  We've got some great tracks coming up for you this show - but first, the news. Top story tonight - civil unrest continues across America, while the current administration tries to divert the public's attention by introducing a reverse carbon tax, whereby citizens will receive tax rebates directly proportional to the amount of carbon they consume.  But most importantly, hospital break-ins are at an all-time high, with reports of shadowy figures finding their way into the rooms of patients, particularly the elderly and patients recovering from surgery.  Anyone witnessing such figures should be made aware that they are NOT hallucinating, and are in fact in very real danger.  One such person is Arthur Roberts, who should be getting a wee visit from one as we speak.  Good luck Artie, cos I think you're gonna need it!";

    private float endX;
    private bool isScrolling = false;
    private AudioController audioController;
    [SerializeField] private bool hallucination;


    IEnumerator Start()
    {
   
        subtitles = PlayerPrefs.GetInt("Subtitles", 0) == 1;      // Load subtitle preference selected in options menu from player prefs and compares the value to 1.  If value is 0,
                                                                  // subtitles bool is true.  If value is 0, subtitles bool is false. If it does not exist, default value of 0 is returned.  
        radioSubtitle.SetActive(subtitles);                       // radio subtitle gameobject is switched on if subtitles bool is true

        if (!subtitles)
            yield return null;                                    // if subtitles bool is not true, coroutine ends

        LoadColourScheme();                                       // Colour of subtitles is checked
        yield return null;                                        // wait one frmae to ensure audiocontroller is initialised in scene
        CheckBroadcastVersion();                                  // Checks bool in audiocontroller to see if real broadcast is being played or hallucination one
        yield return null;                                        // wait one frame to ensure setting is checked
        SetBroadcastMessage();                                    // sets message to real caption or hallucination one
        StartTicker();                                            // starts the scrolling message
        
    }



    void Update()
    {

        if (!isScrolling) return; // if scrolling is not happening, do nothing

        // Move text left
        textRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime; // scroll the text right to left

        // Check if text has fully passed
        if (textRect.anchoredPosition.x <= endX)
        {
            isScrolling = false; // set scolling bool to false
            radioSubtitle.SetActive(false); // Disable the GameObject
        }
    }

    void CheckBroadcastVersion()
    {
        GameObject obj = GameObject.Find("AudioController"); // find the audiocontroller in the scene
        audioController = obj.GetComponent<AudioController>(); // get the audio controller script attached to the audio controller

        if (!audioController.isHallucinating)   // if audiocontroller indicates patient is not hallucinating (it checks interaction history for use of lights or sedative, if they are not
                                                // present, patient does not suffer from these additional hallucinations
        {
            Debug.Log("normal message");        // note that normal message will appear
            hallucination = false;              // set hallucination bool false

        }

        if (audioController.isHallucinating)       // if audiocontroller indicates patient is hallucinating (it checks interaction history for use of lights or sedative, if they are
                                                   // present, patient suffers from additional audio hallucination & may suffer from additional visual ones)
        {
            Debug.Log("hallucination message"); // note that hallucination message will appear
            hallucination = true;               // set hallucination bool true

        }
    }


    private void SetBroadcastMessage()          // sets message on ticker to hallucination message or normal one depending on hallucination bool state
    {

        if (hallucination)
        {
            tickerText.text = hallucinationMessage;
        }
        else
        {
            tickerText.text = normalMessage;
        }

    }


    private void StartTicker()
    {
        tickerText.ForceMeshUpdate();

        float startX = maskRect.rect.width;
        textRect.anchoredPosition =
            new Vector2(startX, textRect.anchoredPosition.y);

        endX = -tickerText.preferredWidth;
        isScrolling = true;
    }



    private void LoadColourScheme() // Check playerprefs for outlinescheme key (for colour blindness setting).  If it exists, use it; otherwise, assume 0 which is standard (red)
    {
        if (PlayerPrefs.HasKey("OutlineScheme"))
        {
            textColour = PlayerPrefs.GetInt("OutlineScheme");
        }
        else textColour = 0; // this equates to red


        switch (textColour)
        {
            case 0: // red
                Standard();
                break;

            case 1: //yellow
                Protanopia();
                break;

            case 2: //yellow
                Deuteranopia();
                break;

            case 3: //red
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

