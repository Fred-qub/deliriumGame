using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    public string prompt;
    public UnityEvent onInteract;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Interact()
    {
        onInteract.Invoke();
    }

}
