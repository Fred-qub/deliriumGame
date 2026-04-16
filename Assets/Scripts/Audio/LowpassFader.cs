using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class LowpassFader : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float targetFrequency = 15000f;
    [SerializeField] private float speed = 5f;
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
        mixer.SetFloat("LowpassCutoff", currentFrequency);

        if (Mathf.Abs(currentFrequency - targetFrequency) < 1f)
        {
            mixer.SetFloat("LowpassCutoff", targetFrequency);
            Debug.Log($"LowpassCutoff set to: {currentFrequency}");
            isFading = false;
        }
    }
}
