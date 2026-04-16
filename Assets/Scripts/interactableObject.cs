using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    public string prompt;
    public UnityEvent onInteract;
    
    [Header("Interaction Link")]
    public DemoInteractable interactionLink;
    

    public void Interact()
    {
        onInteract.Invoke();
    }

}
