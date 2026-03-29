using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour
{

    public GameObject redAlarmLamp;
    public GameObject yellowAlarmLamp;
    public GameObject greenAlarmLamp;
    public GameObject normalScreen52;
    public GameObject normalScreen71;
    public GameObject normalScreen82;
    public GameObject normalScreen107;
    public GameObject alarmScreen121;
    public float flashDuration = 0.1f;
    public float beatInterval;
    private float timer;

    private void OnEnable()
    {
        HeartBeat.OnHeartRateChanged += ChangeScreen;

    }

    private void OnDisable()
    {
        HeartBeat.OnHeartRateChanged += ChangeScreen;
    }


    public IEnumerator FlashRed()
    {
        redAlarmLamp.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        redAlarmLamp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FlashRed());
    }

    public IEnumerator FlashYellow()
    {
        yellowAlarmLamp.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        yellowAlarmLamp.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        StartCoroutine(FlashYellow());
    }

    public IEnumerator FlashGreen(int heartbeat)
    {
        switch (heartbeat)
        {
            case 0: //52bpm
                beatInterval = 60 / 52f;
                while (true)
                {
                    greenAlarmLamp.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    greenAlarmLamp.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 1: //71bpm
                beatInterval = 60 / 71f;
                while (true)
                {
                    greenAlarmLamp.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    greenAlarmLamp.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 2: //82 bpm
                beatInterval = 60 / 82f;
                while (true)
                {
                    greenAlarmLamp.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    greenAlarmLamp.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }

        }
    }
        
        private void ChangeScreen(int heartbeat) 
        {
        switch (heartbeat)
        {
            case 0: // 52 bpm
                     normalScreen52.gameObject.SetActive(true); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 52 bpm");
                StopAllCoroutines();
                yellowAlarmLamp.gameObject.SetActive(false);
                redAlarmLamp.gameObject.SetActive(false);             
                StartCoroutine(FlashGreen(heartbeat));      
                break;
            case 1: // 71 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(true); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                    Debug.Log("screen 71 bpm");
                StopAllCoroutines();
                yellowAlarmLamp.gameObject.SetActive(false);
                redAlarmLamp.gameObject.SetActive(false);
                StartCoroutine(FlashGreen(heartbeat));
                break;
            case 2: // 82 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(true); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 82 bpm");
                StopAllCoroutines();
                yellowAlarmLamp.gameObject.SetActive(false);
                redAlarmLamp.gameObject.SetActive(false);
                StartCoroutine(FlashGreen(heartbeat));
                break;
            case 3: // 107 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(true); alarmScreen121.gameObject.SetActive(false);
                StopAllCoroutines();
                greenAlarmLamp.gameObject.SetActive(false);
                redAlarmLamp.gameObject.SetActive(false);
                StartCoroutine(FlashYellow());
                Debug.Log("screen 107 bpm");
                break;
            case 4:// 121 bpm
                    normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(true);
                StopAllCoroutines();
                greenAlarmLamp.gameObject.SetActive(false);
                yellowAlarmLamp.gameObject.SetActive(false);
                StartCoroutine(FlashRed());
                Debug.Log("screen 121 bpm");
                break;

        }



         }

}
