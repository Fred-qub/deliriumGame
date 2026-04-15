using UnityEngine;

public class EnableAfterdelay : MonoBehaviour
{
    // This script is used in the playback scene to ensure that the objects the doctor interacts with do not turn off/on until the doctor model actually reaches the correct location.


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

    private System.Collections.IEnumerator PoisonSequence()  // causes poison to disppaear from trolley, appear in doctor's hand, then disappear again
    {
        yield return new WaitForSeconds(delayPoison); // delays anything happening til doctor reaches the poison. 
        poisonInHand.SetActive(true); // makes poison object attached to doctor active
        poisonOnTrolley.SetActive(false); // makes poison object on trolley inactive
        yield return new WaitForSeconds(delayPoison); // delay
        poisonInHand.SetActive(false); //sets poison in hand inactive again
    }

    private System.Collections.IEnumerator LightSequence() // delays lights turnning on til doctor reaches switch
    {
        yield return new WaitForSeconds(delayLights); // delay til doctor is next to switch
        lights.SetActive(true); // switches overhead lights on
    }

    private System.Collections.IEnumerator HearingAidSequence() // causes hearing aid to disppaear from locker, appear in doctor's hand, then disappear again
    {
        yield return new WaitForSeconds(delayHearingAid); // delays anything happening til doctor reaches the locker
        hearingAidHand.SetActive(true); //makes hearing aid attached to doctor active
        hearingAidTrolley.SetActive(false); // makes hearing aid object on locker inactive
        yield return new WaitForSeconds(delayHearingAid); //delay
        hearingAidHand.SetActive(false); //sets hearing aid in hand inactive again
    }

    private System.Collections.IEnumerator CoatSequence() // delays coat sequence til doctor reaches the coatrack
    {
        yield return new WaitForSeconds(delayCoat); //delay til doctor gets there
        coatHangerEmpty.SetActive(true); // sets the empty coat hanger object active
        coatOnChair.SetActive(true); // sets the pile of clothes on the chair active

    }
}
