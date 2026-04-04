using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class LowpassFader : MonoBehaviour
{
    public AudioMixer mixer;
    public float targetFrequency = 20000f;
    public float speed = 5f;
    private float currentFrequency = 1000f;
    private bool isFading = false;

    private void OnEnable()
    {
        SceneReplayer.OnHearingAid += SetFade;
    }
    private void OnDisable()
    {
        SceneReplayer.OnHearingAid -= SetFade;
    }

    private void SetFade()
    {
        Debug.Log("LowpassFader: StartFade called");
        isFading = true;
    }
    
    private void Update()
    {
        if (!isFading) return;

        currentFrequency = Mathf.MoveTowards(currentFrequency, targetFrequency, Time.deltaTime * speed);
        Debug.Log($"LowpassCutoff set to: {currentFrequency}");
        mixer.SetFloat("LowpassCutoff", currentFrequency);

        if (Mathf.Abs(currentFrequency - targetFrequency) < 1f)
        {
            mixer.SetFloat("LowpassCutoff", targetFrequency);
            isFading = false;
        }
    }
}
