using TMPro;
using UnityEngine;
using System.Collections;

public class HeartrateDisplay : MonoBehaviour
{
    // This script runs the heartrate & alarm UI display, which is active in both scenes only if the subtitle option is on in the options menu

    [SerializeField] private TMP_Text heartrateDisplay;
    [SerializeField] private GameObject AudioController;
    private HeartBeat heartBeatScript;
    private int rate;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject redAlarm;
    [SerializeField] private GameObject yellowAlarm;
    private float flashDuration = 0.1f;
    private float beatInterval;
    private int subtitles;


    private void OnEnable() // When object becomes active, subscribe to heartbeat update and check to see if subtitles should be on
    {
        
        HeartBeat.OnHeartRateChanged += DisplayHeartrate;
        CheckSubtitleOption();

    }

    private void OnDisable()
    {
    
        HeartBeat.OnHeartRateChanged -= DisplayHeartrate;
       
    }

    private void Start()
    {
        GameObject audioController = GameObject.Find("AudioController"); // find the audiocontroller in the scene
        heartBeatScript = audioController.GetComponent<HeartBeat>(); // get the heartbeat script attached to the audio controller
        rate = heartBeatScript.Heartbeat; // 'rate' value here is set to from the heartbeat value in the heartbeat script
        DisplayHeartrate(rate); // pass rate value to DisplayHeartrate method

    }

    public void DisplayHeartrate(int rate) 
    {

        switch (rate)
        {
            case 0: // this int relates to 52 bpm
                Debug.Log("Heartrate 52 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(52); //find the number animator in the scene & pass the desired heartrate value to it
                heart.gameObject.SetActive(false); // turn off the heart sprite
                TurnOffAlarm(); // turn off the red & yellow alarm images
                StopAllCoroutines(); // stop the alarm images flashing
                StartCoroutine(Flash(rate)); // start the heart image flashing - pass the rate to the coroutine
                break;
            case 1: // this int relates to 71 bpm
                Debug.Log("Heartrate 71 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(71);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 2: // this int relates to 82 bpm
                Debug.Log("Heartrate 82 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(82);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 3: // this int relates to 107 bpm
                Debug.Log("Heartrate 107 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(107);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                StartCoroutine(FlashYellow());
                break;
            case 4:// this int relates to 121 bpm
                Debug.Log("Heartrate 121 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(121);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                StartCoroutine(FlashRed());
                break;

        }
    }

    public IEnumerator Flash(int rate)
    {
        switch (rate)
        {
            case 0: //this int relates to 52bpm
                beatInterval = 60 / 52f; // calculate time between flashes, based on heart rate
                while (true) // while the heart rate remains the same
                {
                    heart.gameObject.SetActive(true); // turn heart sprite on
                    yield return new WaitForSeconds(flashDuration); //wait for appropriate time
                    heart.gameObject.SetActive(false); // turn heart sprite off
                    yield return new WaitForSeconds(beatInterval - flashDuration); // calculate & wait for appropriate time
                }
            case 1: //this int relates to 71bpm
                beatInterval = 60 / 71f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 2: //this int relates to 82 bpm
                beatInterval = 60 / 82f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration); ;
                }
            case 3: //this int relates to 107 bpm
                beatInterval = 60 / 107f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration); ;
                }
            case 4: //this int relates to 121 bpm
                beatInterval = 60 / 121f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration); ;
                }

        }
    }

    public IEnumerator FlashRed() // runs red alarm UI
    {
        redAlarm.gameObject.SetActive(true); //turns red alarm UI on
        yield return new WaitForSeconds(0.5f); // waits for half a second
        redAlarm.gameObject.SetActive(false); // turns red alarm UI off
        yield return new WaitForSeconds(0.5f); // wait for half a second
        StartCoroutine(FlashRed()); // repear
    }

    public IEnumerator FlashYellow() // runs yellow alarm UI
    {

        yellowAlarm.gameObject.SetActive(true); // turns yellow alarm UI on
        yield return new WaitForSeconds(1f); // wait for 1 second
        yellowAlarm.gameObject.SetActive(false); // turn yellow alarm UI off
        yield return new WaitForSeconds(1f); // wait for 1 second
        StartCoroutine(FlashYellow()); //repeat
    }

    void CheckSubtitleOption()
    {
        if (PlayerPrefs.HasKey("Subtitles")) // if playerprefs has the subtitle key, use it; otherwise, assume 0 (off)
        {
            subtitles = PlayerPrefs.GetInt("Subtitles");
        }
        else subtitles = 0;

        switch (subtitles) // switch heartrate display (i.e. the game object this script is attached to) off or on depending on subtitle selection
        {
            case 0:
                gameObject.SetActive(false);
                break;

            case 1:

                gameObject.SetActive(true);
                break;
        }

    }

    private void TurnOffAlarm() // switch both yello & red alarm UI off
    {
        
        if (yellowAlarm)
        {
            yellowAlarm.gameObject.SetActive(false);
        }

        if (redAlarm)
        {
            redAlarm.gameObject.SetActive(false);
        }
    }


}
