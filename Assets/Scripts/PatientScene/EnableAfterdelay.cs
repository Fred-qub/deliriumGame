using UnityEngine;

public class EnableAfterdelay : MonoBehaviour
{
    public GameObject poisonOnTrolley;
    public GameObject poisonInHand;
    public GameObject lights;
    public GameObject hearingAidTrolley;
    public GameObject hearingAidHand;
    public GameObject coatHangerEmpty;
    public GameObject coatOnChair;
    public float delayPoison = 1f;
    public float delayLights = 1f;
    public float delayHearingAid = 1f;
    public float delayCoat = 1f;

    public void Poison()
    {
        StartCoroutine(PoisonSequence());
    }

    public void Lights()
    {
        StartCoroutine(LightSequence());
    }

    public void HearingAid()
    {
        StartCoroutine(HearingAidSequence());
    }

    public void Coat()
    {
        StartCoroutine(CoatSequence());
    }

    private System.Collections.IEnumerator PoisonSequence()
    {
        yield return new WaitForSeconds(delayPoison);
        poisonInHand.SetActive(true);
        poisonOnTrolley.SetActive(false);
        yield return new WaitForSeconds(delayPoison);
        poisonInHand.SetActive(false);
    }

    private System.Collections.IEnumerator LightSequence()
    {
        yield return new WaitForSeconds(delayLights);
        lights.SetActive(true);
    }

    private System.Collections.IEnumerator HearingAidSequence()
    {
        yield return new WaitForSeconds(delayHearingAid);
        hearingAidHand.SetActive(true);
        hearingAidTrolley.SetActive(false);
        yield return new WaitForSeconds(delayHearingAid);
        hearingAidHand.SetActive(false);
    }

    private System.Collections.IEnumerator CoatSequence()
    {
        yield return new WaitForSeconds(delayCoat);
        coatHangerEmpty.SetActive(true);
        coatOnChair.SetActive(true);

    }
}
