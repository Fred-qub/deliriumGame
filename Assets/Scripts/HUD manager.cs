using UnityEngine;
using TMPro;
public class HUDmanager : MonoBehaviour
{
    public static HUDmanager instance;

    private void Awake()
    {
        instance = this;
    }
    
    [SerializeField] TMP_Text interactionText;

    public void enableInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void disableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
