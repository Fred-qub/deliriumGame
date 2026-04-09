using TMPro;
using UnityEngine;
using System.Collections;

public class HeartrateDisplay : MonoBehaviour
{

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


    private void OnEnable()
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
        GameObject audioController = GameObject.Find("AudioController");
        heartBeatScript = audioController.GetComponent<HeartBeat>();
        rate = heartBeatScript.Heartbeat;
        DisplayHeartrate(rate);

    }

    public void DisplayHeartrate(int rate)
    {

        switch (rate)
        {
            case 0: // 52 bpm
                Debug.Log("Heartrate 52 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(52);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 1: // 71 bpm
                Debug.Log("Heartrate 71 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(71);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 2: // 82 bpm
                Debug.Log("Heartrate 82 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(82);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 3: // 107 bpm
                Debug.Log("Heartrate 107 bpm");
                FindObjectOfType<NumberAnimator>().AnimateTo(107);
                heart.gameObject.SetActive(false);
                TurnOffAlarm();
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                StartCoroutine(FlashYellow());
                break;
            case 4:// 121 bpm
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
            case 0: //52bpm
                beatInterval = 60 / 52f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 1: //71bpm
                beatInterval = 60 / 71f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 2: //82 bpm
                beatInterval = 60 / 82f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration); ;
                }
            case 3: //107 bpm
                beatInterval = 60 / 107f;
                while (true)
                {
                    heart.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    heart.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration); ;
                }
            case 4: //121 bpm
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

    public IEnumerator FlashRed()
    {
        redAlarm.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        redAlarm.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FlashRed());
    }

    public IEnumerator FlashYellow()
    {

        yellowAlarm.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        yellowAlarm.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        StartCoroutine(FlashYellow());
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
                gameObject.SetActive(false);
                break;

            case 1:

                gameObject.SetActive(true);
                break;
        }

    }

    private void TurnOffAlarm()
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
