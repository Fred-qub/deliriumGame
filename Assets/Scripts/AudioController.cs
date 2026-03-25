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

    private void OnEnable()
    {
        SceneReplayer.OnInteraction += AudioRoutePatient;
        DemoInteractable.OnInteraction += AudioRouteClinician;
        ShrinkObject.OnShrink += FadeGhost;
    }

    private void OnDisable()
    {
        SceneReplayer.OnInteraction -= AudioRoutePatient;
        DemoInteractable.OnInteraction -= AudioRouteClinician;
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
            case "Lights": lightBuzz.Play(); lightSwitch.Play(); heartSlow.Stop(); heartMedium.Stop(); heartFast.Play(); break;
            case "Sedative": sedative.Play(); heartSlow.Stop(); heartMedium.Stop(); heartFast.Play(); break;
            case "Coat": heartMedium.Stop(); heartFast.Stop(); heartSlow.Play(); break;
            case "HearingAid": heartMedium.Stop(); heartFast.Stop(); heartSlow.Play(); break;
        }
        
    }

    private void AudioRouteClinician(string objectName)
    {
        switch (objectName)
        {
            case  "Lights": lightSwitch.Play(); heartSlow.Stop(); heartMedium.Stop(); heartFast.Play(); break;
            case "Sedative": heartSlow.Stop(); heartMedium.Stop(); heartFast.Play(); break;
            case "Coat": coat.Play(); heartMedium.Stop(); heartFast.Stop(); heartSlow.Play(); break;
            case "HearingAid": heartMedium.Stop(); heartFast.Stop(); heartSlow.Play(); break;
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
                heartMedium.Play();
                isHallucinating = true;
                break;
            }
                
        }

        if (!isHallucinating)
        {
            radioReal.Play();
            heartMedium.Play();
        }
    }

    private void FadeGhost()
    {
        ghostMoans.volume = Mathf.Lerp(startVolume, endVolume, fadeTime);
    }
}
