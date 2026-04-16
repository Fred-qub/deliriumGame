using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private AudioSource heart52;
    [SerializeField] private AudioSource heart71;
    [SerializeField] private AudioSource heart82;
    [SerializeField] private AudioSource heart107;
    [SerializeField] private AudioSource heart121;
    
    [Header ("Heart Rate Monitor")]
    [SerializeField] private AudioSource monitor52;
    [SerializeField] private AudioSource monitor71;
    [SerializeField] private AudioSource monitor82;
    [SerializeField] private AudioSource monitor107;
    [SerializeField] private AudioSource monitor121;
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
            case 0: // this int relates to 52 bpm
               StopAllHeartbeats(); // stop heart sounds
               heart52.Play(); //play correct file for this rate
                Debug.Log("Heartrate 52 bpm");
                break;
            case 1: // this int relates to 71 bpm
                StopAllHeartbeats();
              heart71.Play();
                Debug.Log("Heartrate 71 bpm");
                break;
            case 2: // this int relates to 82 bpm
                StopAllHeartbeats();
              heart82.Play();
                Debug.Log("Heartrate 82 bpm");
                break;
            case 3: // this int relates to 107 bpm
                StopAllHeartbeats();
              heart107.Play();
                Debug.Log("Heartrate 107 bpm");
                break;
            case 4:// this int relates to 121 bpm
                StopAllHeartbeats();
             heart121.Play();
                Debug.Log("Heartrate 121 bpm");
                break;

        }
    }

    private void StopAllHeartbeats() // stops all heartbeat sounds and alarm sounds
    {
        heart52.Stop();
        heart71.Stop();
        heart82.Stop();
        heart107.Stop();
        heart121.Stop();
    }

    private void MonitorSpeed(int heartbeat)  // this is for playing the monitor beeps during the clinician scene
    {

        switch (heartbeat)
        {
            case 0: // this int relates to 52 bpm
                StopAllMonitors(); // stop monitor sounds
                monitor52.Play(); //play correct file for this heart rate
                Debug.Log("Heartrate 52 bpm");
                break;
            case 1: // this int relates to 71 bpm
                StopAllMonitors();
                monitor71.Play();
                Debug.Log("Heartrate 71 bpm");
                break;
            case 2: // this int relates to 82 bpm
                StopAllMonitors();
                monitor82.Play();
                Debug.Log("Heartrate 82 bpm");
                break;
            case 3: // this int relates to 107 bpm
                StopAllMonitors();
                monitor107.Play();
                Debug.Log("Heartrate 107 bpm");
                break;
            case 4:// this int relates to 121 bpm
                StopAllMonitors();
                monitor121.Play();
                alarm.Play();
                Debug.Log("Heartrate 121 bpm");
                break;

        }
    }
    
    private void StopAllMonitors() // stops all heart monitor sounds and alarm sounds
    {
        monitor52.Stop();
        monitor71.Stop();
        monitor82.Stop();
        monitor107.Stop();
        monitor121.Stop();
        alarm.Stop();
    }
}
