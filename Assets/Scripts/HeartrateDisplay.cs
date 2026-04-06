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
                heartrateDisplay.text = "52";
                heart.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 1: // 71 bpm
                Debug.Log("Heartrate 71 bpm");
                heartrateDisplay.text = "71";
                heart.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 2: // 82 bpm
                Debug.Log("Heartrate 82 bpm");
                heartrateDisplay.text = "82";
                heart.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 3: // 107 bpm
                Debug.Log("Heartrate 107 bpm");
                heartrateDisplay.text = "107";
                heart.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
                break;
            case 4:// 121 bpm
                Debug.Log("Heartrate 121 bpm");
                heartrateDisplay.text = "121";
                heart.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Flash(rate));
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


}
