using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]private AudioSource lightSwitch;
    [SerializeField]private AudioSource lightBuzz;
    [SerializeField]private AudioSource coat;
    [SerializeField]private AudioSource sedative;
    [SerializeField]private AudioSource radioReal;
    [SerializeField]private AudioSource radioHallucination;
    [SerializeField] private AudioSource heartSlow;
    [SerializeField] private AudioSource heartMedium;
    [SerializeField] private AudioSource heartFast;


    [Header("Ghost Audio")]
    [SerializeField]private AudioSource ghostMoans;
    [SerializeField] private float startVolume;
    [SerializeField] private float endVolume;
    [SerializeField] private float fadeTime;

    private HeartBeat heartBeatScript;
    private int heartbeat;


    private void OnEnable()
    {
        SceneReplayer.OnInteraction += AudioRoutePatient; StartCoroutine(HeartSpeed());
        DemoInteractable.OnInteraction += AudioRouteClinician; StartCoroutine(HeartSpeed());
        ShrinkObject.OnShrink += FadeGhost;
  
    }

    private void OnDisable()
    {
        SceneReplayer.OnInteraction -= AudioRoutePatient; StartCoroutine(HeartSpeed());
        DemoInteractable.OnInteraction -= AudioRouteClinician; StartCoroutine(HeartSpeed());
        ShrinkObject.OnShrink -= FadeGhost;
    }

    private void Start()
    {
        RadioRoute();

    }

    private void Update()
    {
        heartbeat = heartBeatScript.Heartbeat;
    }

    private void AudioRoutePatient(string actionName)
    {
        StartCoroutine(HeartSpeed());

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
        StartCoroutine(HeartSpeed());

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
   IEnumerator HeartSpeed() 
    {
        heartBeatScript = GetComponent<HeartBeat>();
        heartbeat = heartBeatScript.Heartbeat;
        yield return new WaitForSeconds(0.5f);
        switch (heartbeat)
        {
            case 0: 
                heartSlow.Play(); heartMedium.Stop(); heartFast.Stop(); 
                break;
            case 1:
                heartSlow.Stop(); heartMedium.Play(); heartFast.Stop();
                break;
            case 2:
                heartSlow.Stop(); heartMedium.Stop(); heartFast.Play();
                break;
            case 3:
                heartSlow.Stop(); heartMedium.Stop(); heartFast.Play();
                break;


        }
    }
}
