using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour
{

    public GameObject alarmLamp;
    public GameObject normalScreen52;
    public GameObject normalScreen71;
    public GameObject normalScreen82;
    public GameObject normalScreen107;
    public GameObject alarmScreen121;

    // Start is called before the first frame update
    void Start()
    {

      //  heartbeat = heartBeatScript.Heartbeat; 
 
    }

    private void OnEnable()
    {
        HeartBeat.OnHeartRateChanged += ChangeScreen;
    }

    private void OnDisable()
    {
        HeartBeat.OnHeartRateChanged += ChangeScreen;
    }


    public IEnumerator Flash()
    {
        alarmLamp.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        alarmLamp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Flash());
    }

    private void ChangeScreen(int heartbeat) 
    {
        switch (heartbeat)
        {
            case 0: // 52 bpm
                     normalScreen52.gameObject.SetActive(true); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 52 bpm");
                break;
            case 1: // 71 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(true); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 71 bpm");
                break;
            case 2: // 82 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(true); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 82 bpm");
                break;
            case 3: // 107 bpm
                     normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(true); alarmScreen121.gameObject.SetActive(false);
                Debug.Log("screen 107 bpm");
                break;
            case 4:// 121 bpm
                    normalScreen52.gameObject.SetActive(false); normalScreen71.gameObject.SetActive(false); normalScreen82.gameObject.SetActive(false); normalScreen107.gameObject.SetActive(false); alarmScreen121.gameObject.SetActive(true);
                StartCoroutine(Flash());
                Debug.Log("screen 121 bpm");
                break;

        }



    }

}
