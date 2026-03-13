using UnityEngine;

public class EnableAfterdelay : MonoBehaviour
{
    public GameObject poisonOnTrolley;
    public GameObject poisonInHand;
    public float delaySeconds = 1f;

    public void EnableSequence() 
    {
        Invoke(nameof(EnablePoison), delaySeconds);

    }

    public void DisableSequence()
    {
        Invoke(nameof(DisablePoison), delaySeconds);

    }


    private void EnablePoison()
    {
        poisonInHand.SetActive(true);
        
    }
    private void DisablePoison()
    {
        poisonOnTrolley.SetActive(false);

    }


}
