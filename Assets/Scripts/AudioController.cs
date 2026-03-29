using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource lightSwitch;
    [SerializeField] private AudioSource lightBuzz;
    [SerializeField] private AudioSource coat;
    [SerializeField] private AudioSource sedative;
    [SerializeField] private AudioSource radioReal;
    [SerializeField] private AudioSource radioHallucination;
    [SerializeField] private AudioSource heart52;
    [SerializeField] private AudioSource monitor52;
    [SerializeField] private AudioSource heart71;
    [SerializeField] private AudioSource monitor71;
    [SerializeField] private AudioSource heart82;
    [SerializeField] private AudioSource monitor82;
    [SerializeField] private AudioSource heart107;
    [SerializeField] private AudioSource monitor107;
    [SerializeField] private AudioSource heart121;
    [SerializeField] private AudioSource monitor121;
    [SerializeField] private AudioSource alarm;


    [Header("Ghost Audio")]
    [SerializeField]private AudioSource ghostMoans;
    [SerializeField] private float startVolume;
    [SerializeField] private float endVolume;
    [SerializeField] private float fadeTime;

        private void OnEnable()
    {
        SceneReplayer.OnInteraction += AudioRoutePatient;
        HeartBeat.OnHeartRateChanged += SceneRoute;
        DemoInteractable.OnInteraction += AudioRouteClinician;
        ShrinkObject.OnShrink += FadeGhost;
  
    }

    private void OnDisable()
    {
        SceneReplayer.OnInteraction -= AudioRoutePatient;
        DemoInteractable.OnInteraction -= AudioRouteClinician;
        HeartBeat.OnHeartRateChanged -= SceneRoute;
        ShrinkObject.OnShrink -= FadeGhost;
    }

    private void Start()
    {
        RadioRoute();

    }

    private void AudioRoutePatient(string actionName)
    {


        switch (actionName)
        {
            case "Lights": lightBuzz.Play(); lightSwitch.Play(); break;
            case "Sedative": sedative.Play();break;
            case "Coat": break;
            case "HearingAid": break;
        }
        
    }

    private void AudioRouteClinician(string objectName)
    {

        switch (objectName)
        {
            case "Lights": lightSwitch.Play(); break;
            case "Sedative": break;
            case "Coat": coat.Play(); break;
            case "HearingAid": break;
        }
    }

    private void RadioRoute()
    {
        List<string> history = InteractionMaster.Instance.interactionHistory;
        bool isHallucinating = false;
        for (int i=0; i< history.Count; i++) 
        {
            if (history[i] == "Lights" || history[i] == "Sedative") 
            {
                radioHallucination.Play();
                isHallucinating = true;
                break;
            }
                
        }

        if (!isHallucinating)
        {
            radioReal.Play();

        }
    }

    private void FadeGhost()
    {
        ghostMoans.volume = Mathf.Lerp(startVolume, endVolume, fadeTime);

    }

    private void SceneRoute(int heartbeat)
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        switch (scene.name)
        {
            case "Clinician Scene Ruth": MonitorSpeed(heartbeat);
                break;
            case "PatientScene Ruth": HeartSpeed(heartbeat);
                break;
        }
    }

   private void HeartSpeed(int heartbeat) 
    {

        switch (heartbeat)
        {
            case 0: // 52 bpm
               // heart52.Play(); heart71.Stop(); heart82.Stop(); heart107.Stop(); heart121.Stop();
               StopAllHeartbeats();
               heart52.Play();
                Debug.Log("Heartrate 52 bpm");
                break;
            case 1: // 71 bpm
              //  heart52.Stop(); heart71.Play(); heart82.Stop(); heart107.Stop(); heart121.Stop();
              StopAllHeartbeats();
              heart71.Play();
                Debug.Log("Heartrate 71 bpm");
                break;
            case 2: // 82 bpm
              //  heart52.Stop(); heart71.Stop(); heart82.Play(); heart107.Stop(); heart121.Stop();
              StopAllHeartbeats();
              heart82.Play();
                Debug.Log("Heartrate 82 bpm");
                break;
            case 3: // 107 bpm
              //  heart52.Stop(); heart71.Stop(); heart82.Stop(); heart107.Play(); heart121.Stop();
              StopAllHeartbeats();
              heart107.Play();
                Debug.Log("Heartrate 107 bpm");
                break;
            case 4:// 121 bpm
             //   heart52.Stop(); heart71.Stop(); heart82.Stop(); heart107.Stop(); heart121.Play();
             StopAllHeartbeats();
             heart121.Play();
                Debug.Log("Heartrate 121 bpm");
                break;

        }
    }

    private void StopAllHeartbeats()
    {
        heart52.Stop();
        heart71.Stop();
        heart82.Stop();
        heart107.Stop();
        heart121.Stop();
    }

    private void MonitorSpeed(int heartbeat) 
    {

        switch (heartbeat)
        {
            case 0: // 52 bpm
                // heart52.Play(); heart71.Stop(); heart82.Stop(); heart107.Stop(); heart121.Stop();
                StopAllMonitors();
                monitor52.Play();
                Debug.Log("Heartrate 52 bpm");
                break;
            case 1: // 71 bpm
                //  heart52.Stop(); heart71.Play(); heart82.Stop(); heart107.Stop(); heart121.Stop();
                StopAllMonitors();
                monitor71.Play();
                Debug.Log("Heartrate 71 bpm");
                break;
            case 2: // 82 bpm
                //  heart52.Stop(); heart71.Stop(); heart82.Play(); heart107.Stop(); heart121.Stop();
                StopAllMonitors();
                monitor82.Play();
                Debug.Log("Heartrate 82 bpm");
                break;
            case 3: // 107 bpm
                //  heart52.Stop(); heart71.Stop(); heart82.Stop(); heart107.Play(); heart121.Stop();
                StopAllMonitors();
                monitor107.Play();
                Debug.Log("Heartrate 107 bpm");
                break;
            case 4:// 121 bpm
                //   heart52.Stop(); heart71.Stop(); heart82.Stop(); heart107.Stop(); heart121.Play();
                StopAllMonitors();
                monitor121.Play();
                alarm.Play();
                Debug.Log("Heartrate 121 bpm");
                break;

        }
    }
    
    private void StopAllMonitors()
    {
        monitor52.Stop();
        monitor71.Stop();
        monitor82.Stop();
        monitor107.Stop();
        monitor121.Stop();
        alarm.Stop();
    }
}
