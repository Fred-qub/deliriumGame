using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Alarm : MonoBehaviour
{

    // This script controls the display of the screens on the patient monitor and also the green heartrate lamp and the red & yellow alarm lamps.  Clinician scene only - not present in patient scene.

    public GameObject redAlarmLamp;
    public GameObject yellowAlarmLamp;
    public GameObject greenAlarmLamp;
    public GameObject normalScreen50;
    public GameObject normalScreen65;
    public GameObject normalScreen80;
    public GameObject normalScreen110;
    public GameObject alarmScreen140;
    public float flashDuration = 0.1f;
    public float beatInterval;

    private void OnEnable()
    {
        HeartBeat.OnHeartRateChanged += ChangeScreen; // subscribes to heartrate change of heartbeat script,

    }

    private void OnDisable()
    {
        HeartBeat.OnHeartRateChanged -= ChangeScreen; // unsubscribes from heartrate change of heartbeat script,
    }


    public IEnumerator FlashRed()
    {
        redAlarmLamp.gameObject.SetActive(true); // turns red lamp on
        yield return new WaitForSeconds(0.5f); // wait for half a second
        redAlarmLamp.gameObject.SetActive(false); // turns red lamp off
        yield return new WaitForSeconds(0.5f); // wait for half a second
        StartCoroutine(FlashRed()); // repeat
    }

    public IEnumerator FlashYellow()
    {

        yellowAlarmLamp.gameObject.SetActive(true); // turn yellow lamp on
        yield return new WaitForSeconds(1f); // wait for one second (usually, in real life, a yellow alarm, as a lower priority alarm, will flash slower than a red high priority alarm)
        yellowAlarmLamp.gameObject.SetActive(false); // turn yellow lamp off
        yield return new WaitForSeconds(1f); // wait for one second
        StartCoroutine(FlashYellow()); // repeat
    }

    public IEnumerator FlashGreen(int heartbeat)
    {
        switch (heartbeat) // switches with the heartbeat; light is synced to the beat the way a real monitor would be
        {
            case 0: //this int relates to a heart rate of 50bpm
                beatInterval = 60 / 50f; // time between beats
                while (true) // run this code until the heartrate changes
                {
                    greenAlarmLamp.gameObject.SetActive(true); // turn green lamp on
                    yield return new WaitForSeconds(flashDuration); // keep it on for the duration of the flash
                    greenAlarmLamp.gameObject.SetActive(false); // turn green lamp off
                    yield return new WaitForSeconds(beatInterval - flashDuration); // wait til it is time for the next flash
                }
            case 1: //this int relates to a heart rate of 65bpm
                beatInterval = 60 / 65f;
                while (true)
                {
                    greenAlarmLamp.gameObject.SetActive(true);
                    yield return new WaitForSeconds(flashDuration);
                    greenAlarmLamp.gameObject.SetActive(false);
                    yield return new WaitForSeconds(beatInterval - flashDuration);
                }
            case 2: //this int relates to a heart rate of 80 bpm
                beatInterval = 60 / 80f;
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
            var scene = SceneManager.GetActiveScene(); // check scene
        if (scene.name == "Clinician Scene Ruth") // only runs in clinician scene
        {
            switch (heartbeat) // switches with the heartbeat
            {
                case 0: // this int relates to a heart rate of 50 bpm
                    RemoveScreens(); //turn all monitor screen gameobjects off
                    normalScreen50.gameObject.SetActive(true); // turn the 50bpm screen on the patient monitor
                    Debug.Log("screen 50 bpm");
                    StopAllCoroutines(); // stop light flash routines, 
                    TurnOffLights(); // ensure all alarm lamps off
                    StartCoroutine(FlashGreen(heartbeat)); // start the green lamp flashing at the correct rate of 50 bpm
                    break;
                case 1: // this int relates to a heart rate of 65 bpm
                    RemoveScreens();
                    normalScreen65.gameObject.SetActive(true); // turn the 65 bpm screen on
                    Debug.Log("screen 65 bpm");
                    StopAllCoroutines();
                    TurnOffLights();
                    StartCoroutine(FlashGreen(heartbeat)); // start the green lamp flashing at the correct rate of 65 bpm
                    break;
                case 2: // this int relates to a heart rate of 80 bpm
                    RemoveScreens();
                    normalScreen80.gameObject.SetActive(true); // turn the 80 bpm screen on
                    Debug.Log("screen 80 bpm");
                    StopAllCoroutines();
                    TurnOffLights();
                    StartCoroutine(FlashGreen(heartbeat)); // start the green lamp flashing at the correct rate of 80 bpm
                    break;
                case 3: // this int relates to a heart rate of 110 bpm
                    RemoveScreens();
                    normalScreen110.gameObject.SetActive(true); // turn the 110 bpm screen on
                    StopAllCoroutines();
                    TurnOffLights();
                    StartCoroutine(FlashYellow()); // start the yellow alarm lamp flashing
                    Debug.Log("screen 110 bpm");
                    break;
                case 4:// this int relates to a heart rate of 140 bpm
                    RemoveScreens();
                    alarmScreen140.gameObject.SetActive(true); // turn the 140 bpm screen on
                    StopAllCoroutines();
                    TurnOffLights();
                    StartCoroutine(FlashRed()); // start the red alarm lamp flashing
                    Debug.Log("screen 140 bpm");
                    break;

            }

            }        
        }

    private void RemoveScreens() // turn off whatever screen is on the monitor
    {
        if (normalScreen50) 
        {
            normalScreen50.gameObject.SetActive(false);
        }

        if (normalScreen65)
        {
            normalScreen65.gameObject.SetActive(false);
        }

        if (normalScreen80)
        {
            normalScreen80.gameObject.SetActive(false);
        }

        if (normalScreen110)
        {
            normalScreen110.gameObject.SetActive(false);
        }

        if (alarmScreen140)
        {
            alarmScreen140.gameObject.SetActive(false);
        }

  
    }

    private void TurnOffLights() // turn off whatever alarm lamp is on the monitor
    {
        if (greenAlarmLamp)
        {
            greenAlarmLamp.gameObject.SetActive(false);
        }

        if (yellowAlarmLamp)
        {
            yellowAlarmLamp.gameObject.SetActive(false);
        }

        if (redAlarmLamp)
        {
            redAlarmLamp.gameObject.SetActive(false);
        }
    }

}
