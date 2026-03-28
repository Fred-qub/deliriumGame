using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour
{

    public HeartBeat heartBeatScript;
    public int heartbeat;
    public GameObject alarmLamp;
    public GameObject normalScreen;
    public GameObject alarmScreen;

    // Start is called before the first frame update
    void Start()
    {

        heartbeat = heartBeatScript.Heartbeat; 
 
    }

    // Update is called once per frame
    void Update()
    {
        heartbeat = heartBeatScript.Heartbeat;

        if (heartbeat == 3) 
        {
            normalScreen.gameObject.SetActive(false);
            alarmScreen.gameObject.SetActive(true);
            Debug.Log("alarm active");
            StartCoroutine(Flash());
        }

    }

    public IEnumerator Flash()
    {
        alarmLamp.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        alarmLamp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
    }

}
