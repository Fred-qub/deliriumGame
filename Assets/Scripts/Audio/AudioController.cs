using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour
{
    [Header ("Clinician Actions")]
    [SerializeField] private AudioSource lightSwitch;
    [SerializeField] private AudioSource lightBuzz;
    [SerializeField] private AudioSource coat;
    [SerializeField] private AudioSource sedative;
    [SerializeField] private AudioSource hearingAidFeedback;

    [Header("Radio")] 
    public AudioSource radioReal;
    public AudioSource radioHallucination;
    
    [Header ("Heartbeats")]
    [SerializeField] private AudioSource heartLowest;
    [SerializeField] private AudioSource heartLow;
    [SerializeField] private AudioSource heartMiddle;
    [SerializeField] private AudioSource heartHigh;
    [SerializeField] private AudioSource heartHighest;
    
    [Header ("Heart Rate Monitor")]
    [SerializeField] private AudioSource monitorLowest;
    [SerializeField] private AudioSource monitorLow;
    [SerializeField] private AudioSource monitorMiddle;
    [SerializeField] private AudioSource monitorHigh;
    [SerializeField] private AudioSource monitorHighest;
    [SerializeField] private AudioSource alarm;
    
    public bool isHallucinating;
    
    [Header("Ghost Audio")]
    [SerializeField]private AudioSource ghostMoans;
    [SerializeField] private float startVolume;
    [SerializeField] private float endVolume;
    [SerializeField] private float fadeTime;

        private void OnEnable()
    {
        SceneReplayer.OnInteraction += AudioRoutePatient;
        DemoInteractable.OnInteraction += AudioRouteClinician;
        HeartBeat.OnHeartRateChanged += SceneRoute;
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
        RadioRoute(); // Checks at start of scene which radio broadcast should play; this depends on whether or not patient is hallucinating.  Because it only checks at the start of the scene,
                        // it will not change mid-scene.  So in practice, the normal one will always play in the clinician scene, and the other may or may not play in the patient scene, depending
                        // on the actions the player took during the clinician scene

    }

    private void AudioRoutePatient(string actionName) // checks & plays correct sound during clinician scene interactions
    {
        switch (actionName)
        {
            case "Lights": lightBuzz.Play(); lightSwitch.Play(); break;
            case "Sedative": sedative.Play();break;
            case "Coat": break;
            case "HearingAid": hearingAidFeedback.Play(); break;
        }
    }

    private void AudioRouteClinician(string objectName) // checks & plays correct sound during clinician scene interactions
    {

        switch (objectName)
        {
            case "Lights": lightSwitch.Play(); break;
            case "Sedative": break;
            case "Coat": coat.Play(); break;
            case "HearingAid": hearingAidFeedback.Play(); break;
        }
    }

    private void RadioRoute()
    {
        List<string> history = InteractionMaster.Instance.interactionHistory; // checks what is in the interaction history
        isHallucinating = false; // initially patient is not hallucinating
        for (int i=0; i< history.Count; i++) 
        {
            if (history[i] == "Lights" || history[i] == "Sedative") //if the interation history contains lights or sedative, the patient will hallucinate, so play this radio broadcast
            {
                radioHallucination.Play();
                isHallucinating = true;
                break;
            }
                
        }

        if (!isHallucinating) //if the patient is not hallucinating, play this radio broadcast
        {
            radioReal.Play();
        }
    }

    private void FadeGhost() // allows shadow man audio to fade
    {
        ghostMoans.volume = Mathf.Lerp(startVolume, endVolume, fadeTime);
    }

    private void SceneRoute(int heartbeat) // plays correct heart sound depending on scene
    {
        var scene = SceneManager.GetActiveScene(); // Get scene name
        Debug.Log(scene.name);
        switch (scene.name)
        {
            case "Clinician Scene Ruth": MonitorSpeed(heartbeat); // plays patient monitor beep at correct speed for patient heartrate in clinician scene
                break;
            case "PatientScene Ruth": HeartSpeed(heartbeat); // plays patient's own heartbeat at correct speed in patient scene
                break;
        }
    }

   private void HeartSpeed(int heartbeat) // this is for playing the heartbeat in the patient's own ears during the patient scene
    {

        switch (heartbeat)
        {
            case 0: // this int relates to 50 bpm
               StopAllHeartbeats(); // stop heart sounds
               heartLowest.Play(); //play correct file for this rate
                Debug.Log("Heartrate: 50 bpm");
                break;
            case 1: // this int relates to 65 bpm
                StopAllHeartbeats();
              heartLow.Play();
                Debug.Log("Heartrate: 65 bpm");
                break;
            case 2: // this int relates to 80 bpm
                StopAllHeartbeats();
              heartMiddle.Play();
                Debug.Log("Heartrate: 80 bpm");
                break;
            case 3: // this int relates to 110 bpm
                StopAllHeartbeats();
              heartHigh.Play();
                Debug.Log("Heartrate: 110 bpm");
                break;
            case 4:// this int relates to 140 bpm
                StopAllHeartbeats();
             heartHighest.Play();
                Debug.Log("Heartrate: 140 bpm");
                break;

        }
    }

    private void StopAllHeartbeats() // stops all heartbeat sounds and alarm sounds
    {
        heartLowest.Stop();
        heartLow.Stop();
        heartMiddle.Stop();
        heartHigh.Stop();
        heartHighest.Stop();
    }

    private void MonitorSpeed(int heartbeat)  // this is for playing the monitor beeps during the clinician scene
    {

        switch (heartbeat)
        {
            case 0: // this int relates to 50 bpm
                StopAllMonitors(); // stop monitor sounds
                monitorLowest.Play(); //play correct file for this heart rate
                Debug.Log("Heartrate: 50 bpm");
                break;
            case 1: // this int relates to 65 bpm
                StopAllMonitors();
                monitorLow.Play();
                Debug.Log("Heartrate: 65 bpm");
                break;
            case 2: // this int relates to 80 bpm
                StopAllMonitors();
                monitorMiddle.Play();
                Debug.Log("Heartrate: 80 bpm");
                break;
            case 3: // this int relates to 110 bpm
                StopAllMonitors();
                monitorHigh.Play();
                Debug.Log("Heartrate: 110 bpm");
                break;
            case 4:// this int relates to 140 bpm
                StopAllMonitors();
                monitorHighest.Play();
                alarm.Play();
                Debug.Log("Heartrate: 140 bpm");
                break;

        }
    }
    
    private void StopAllMonitors() // stops all heart monitor sounds and alarm sounds
    {
        monitorLowest.Stop();
        monitorLow.Stop();
        monitorMiddle.Stop();
        monitorHigh.Stop();
        monitorHighest.Stop();
        alarm.Stop();
    }
}
