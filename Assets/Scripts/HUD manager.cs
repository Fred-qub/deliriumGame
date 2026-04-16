using UnityEngine;
using TMPro;
public class HUDmanager : MonoBehaviour
{
    // This script runs the display of the interaction prompt when the target passes over an interactable object
    public static HUDmanager instance;

    private void Awake()
    {
        instance = this;
    }
    
    [SerializeField] TMP_Text interactionText;

    public void enableInteractionText(string text) // prompt to press E to interact
    {   
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void enableMaxInteractionText(string text) // displayed when 2 interactions have already taken place, to indicate no further interactions allowed
    {
        interactionText.text = text;
        interactionText.gameObject.SetActive(true);
    }



    public void disableInteractionText() // turn off interaction text (when target is no longer over the object)
    {
        interactionText.gameObject.SetActive(false);
    }
}
